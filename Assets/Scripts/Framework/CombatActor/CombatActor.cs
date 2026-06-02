using System;
using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;
using Dajunctic.SkillSystem.Gambits;
using Dajunctic.SkillSystem.Logic;

namespace Dajunctic
{
    public class CombatActor : BaseView, ICombatActor, ISkillOwner, ITeamMember
    {
        public static readonly List<CombatActor> ActiveActors = new();

        [SerializeField, Child] protected Animator animator;
        [SerializeField, Child(Flag.Optional | Flag.IncludeInactive)] protected MidPoint midPoint;
        [SerializeField, Child(Flag.Optional | Flag.IncludeInactive)] protected HeadPoint headPoint;
        [SerializeField] protected CombatActorData combatActorData;
        public CombatActorData CombatActorData => combatActorData;
        
        public void SetCombatData(CombatActorData data)
        {
            combatActorData = data;
            if (Initialized && data != null)
            {
                Stats = new ChampionStats(combatActorData);
                InitDamageTaker();
                OnHpChanged?.Invoke(MaxHp > 0 ? Hp / MaxHp : 1f);
            }
        }
        [SerializeField] private Team team;
        public Team CombatTeam => team;
        public int OwnerID { get; set; } = 0;

        public void SetTeam(Team newTeam) => team = newTeam;

        public ICombatTeam EnemyTeam { get; private set; }

        public void SetEnemyTeam(ICombatTeam enemyTeam)
        {
            EnemyTeam = enemyTeam;
        }

        public virtual Vector3 Position { get; protected set; }
        public virtual Vector3 Forward { get; protected set; }

        public bool IsViewLoaded => _viewLoaded;
        public virtual string DataId => string.Empty;
        public virtual bool CanBeTarget => true;

        public bool ActiveInHierarchy => gameObject.activeInHierarchy;
        public bool ActiveSelf => gameObject.activeSelf;

        void OnValidate() => this.ValidateRefs();
        public CombatActor CurrentTarget { get; private set; }
        public Vector3 MidPoint => midPoint != null ? midPoint.Position : CachedTransform.position + Vector3.up * 1f;
        public Vector3 HeadPoint => headPoint != null ? headPoint.Position : CachedTransform.position + Vector3.up * 2f;
        public float CombatRadius => combatActorData.movement.radius;
        public float Speed => combatActorData.movement.moveSpeed;
        public float RotateSpeed => combatActorData.movement.rotateSpeed;
        public float AtkSpd => Stats.AttackSpeed.Value;
        public ChampionStats Stats { get; private set; }
        public event Action<float> OnHpChanged;
        public event Action<CalculatedDamage> OnDamageTakenEvent;
        public event Action OnHpChangedEvent;

        bool _viewLoaded;

        List<Gambit> _activeGambits = new();
        private Vector3 _lastPosition;

        public override void Initialize()
        {
            if (Initialized) return;
            base.Initialize();
            _viewLoaded = true;

            if (!ActiveActors.Contains(this))
            {
                ActiveActors.Add(this);
            }

            Position = CachedTransform.position;
            _lastPosition = Position;
            Forward = CachedTransform.forward;

            InitializeMoveAgent();
            Stats = new ChampionStats(combatActorData);
            InitDamageTaker();
            InitGambits();
        }

        void InitGambits()
        {
            if (combatActorData == null || combatActorData.gambits == null || combatActorData.gambits.Count == 0)
            {
                return;
            }

            _activeGambits.Clear();
            foreach (var g in combatActorData.gambits)
            {
                if (g == null) continue;
                var instance = g.CreateCopy();
                instance.Initialize(this);
                _activeGambits.Add(instance);
            }
        }

        public override void Tick()
        {
            base.Tick();

            if (MoveAgent != null && !MoveAgent.Initialized)
            {
                MoveAgent = null;

                Position = CachedTransform.position;
                Forward = CachedTransform.forward;
                InitializeMoveAgent();
            }

            if (Hp > 0 && !IsCasting)
            {
                EvaluateGambits();
            }

            SyncTransform();

            float realSpeed = 0f;
            if (Time.deltaTime > 0f)
            {
                realSpeed = (Position - _lastPosition).magnitude / Time.deltaTime;
            }
            _lastPosition = Position;

            if (MoveAgentAlive && MoveAgent.Velocity.magnitude > realSpeed)
            {
                realSpeed = MoveAgent.Velocity.magnitude;
            }

            float normalizedSpeed = Mathf.Clamp01(realSpeed / Speed);
            animator.SetFloat("Speed", normalizedSpeed);

            SyncEntity();
        }

        protected virtual void EvaluateGambits()
        {
            if (_activeGambits.Count == 0)
            {
                return;
            }

            foreach (var gambit in _activeGambits)
            {
                var target = gambit.condition?.Check();

                if (target == null)
                {
                    continue;
                }

                if (gambit.action == null)
                {
                    continue;
                }

                if (!gambit.action.CheckCanPlay())
                {
                    continue;
                }

                SetTarget(target as CombatActor);
                SetCasting(true);
                gambit.action.Play(target);
                break;
            }
        }

        public override void Cleanup()
        {
            ActiveActors.Remove(this);
            base.Cleanup();
        }

        protected virtual void OnDestroy()
        {
            ActiveActors.Remove(this);
            this.Raise(new DespawnHpViewEvent { owner = this });
        }

        protected virtual void SetupTree()
        {
        }

        public void SetTarget(CombatActor target)
        {
            CurrentTarget = target;
        }

        public bool HasValidTarget()
        {
            return CurrentTarget != null && CurrentTarget.gameObject.activeInHierarchy && CurrentTarget.CanBeTarget;
        }

        #region Transform
        public Vector3 TransformPoint(Vector3 point)
        {
            return CachedTransform.TransformPoint(point);
        }

        public Vector3 TransformDirection(Vector3 direction)
        {
            return CachedTransform.TransformDirection(direction);
        }

        #endregion

        #region Movement
        public IMoveAgent MoveAgent;
        public virtual bool CanMove => IsViewLoaded && MoveAgentAlive && MoveAgent.CanMove;
        public bool IsMoving => MoveAgentAlive && MoveAgent.IsMoving;
        public Vector3 Velocity => MoveAgentAlive ? MoveAgent.Velocity : Vector3.zero;

        bool MoveAgentAlive => MoveAgent != null && MoveAgent.Initialized;
        protected virtual ActorMovementType ActorMovementType => combatActorData.movement.movementType;
        public virtual MovementPriority AvoidancePriority
        {
            get
            {
                switch (ActorMovementType)
                {
                    case ActorMovementType.Navmesh:
                        return GetDynamicAvoidancePriority();
                    case ActorMovementType.Obstacle:
                        return MovementPriority.Obstacle;
                    case ActorMovementType.Transform:
                        return GetDynamicAvoidancePriority();
                }
                return GetDynamicAvoidancePriority();
            }
        }

        protected virtual MovementPriority GetDynamicAvoidancePriority()
        {
            return MovementPriority.None;
        }

        void InitializeMoveAgent()
        {
            if (MoveAgent != null || !IsViewLoaded) return;

            switch (ActorMovementType)
            {
                case ActorMovementType.Navmesh:
                    MoveAgent = NavMeshMoveAgent.Pool.GetOrCreate($"na_{DataId}");
                    break;
                case ActorMovementType.Obstacle:
                    MoveAgent = NavMeshMoveAgent.Pool.GetOrCreate($"no_{DataId}");
                    break;
                case ActorMovementType.Transform:
                    MoveAgent = new TransformMoveAgent();
                    break;
                case ActorMovementType.HexGrid:
                    MoveAgent = gameObject.AddComponent<HexGridMoveAgent>();
                    break;
                default:
                    MoveAgent = new TransformMoveAgent();
                    break;
            }
            MoveAgent.Initialize();
            MoveAgent.SetEnable(false);
            MoveAgent.SetType("Humanoid");
            MoveAgent.SetSize(combatActorData.movement.height, combatActorData.movement.radius);
            MoveAgent.ChangePriority((int)AvoidancePriority);
            MoveAgent.SetOffset(0);
            MoveAgent.SetAcceleration(combatActorData.movement.acceleration);
            MoveAgent.ToggleMoveCollision(true);

            InitMoveAgent();

        }

        void InitMoveAgent()
        {
            if (MoveAgent == null) return;
            MoveAgent.Warp(Position);
            MoveAgent.SetEnable(true);
            MoveAgent.RotateDirection(Forward, 0, 0, true);
        }

        public void MovePosition(Vector3 position, float moveSpeed, float rotateSpeed, float stoppingDistance = 0.1f)
        {
            if (MoveAgent != null)
            {
                MoveAgent.MovePosition(position, moveSpeed, rotateSpeed, stoppingDistance);
            }
        }

        public void RotatePosition(Vector3 position, float rotateSpeed, float deltaTime, bool immediately)
        {
            RotateDirection(position - Position, rotateSpeed, deltaTime, immediately);
        }

        public virtual void Teleport(Vector3 position, bool checkNavMesh, bool fx = false)
        {
            if (MoveAgent != null)
            {
                ForceStop();
                if (checkNavMesh && NavMesh.SamplePosition(position, out var hit, 5f, NavMesh.AllAreas))
                {
                    position = hit.position;
                }

                MoveAgent.Warp(position);
                Position = position;
                _lastPosition = position;
            }

            if (fx)
            {

            }
        }

        public void RotateDirection(Vector3 direction, float rotateSpeed, float deltaTime, bool immediately)
        {
            direction.y = 0;
            direction.Normalize();

            if (direction != Vector3.zero)
            {
                if (MoveAgent != null)
                {
                    MoveAgent.RotateDirection(direction, rotateSpeed, deltaTime, immediately);
                }
                else
                {
                    if (immediately)
                    {
                        Forward = direction;
                    }
                    else
                    {
                        Forward = Quaternion.Slerp(Quaternion.LookRotation(Forward), Quaternion.LookRotation(direction), deltaTime * rotateSpeed) * Vector3.forward;
                    }
                }

                SyncTransform();
            }
        }

        public void ForceStop()
        {
            if (MoveAgent != null)
            {
                MoveAgent.ForceStop();
            }
        }

        void SyncTransform()
        {
            if (MoveAgentAlive && MoveAgent.IsEnabled)
            {
                Position = MoveAgent.Position;
                Forward = MoveAgent.Forward;
            }
        }

        protected virtual void SyncEntity()
        {
            CachedTransform.position = Position;
            if (Forward != Vector3.zero)
            {
                CachedTransform.rotation = Quaternion.LookRotation(Forward);
            }
        }

        #endregion

        #region DamageTaker

        private float _hp;
        private float _energy;

        public float Hp => _hp;
        public virtual float MaxHp => Stats?.Health?.Value > 0 ? Stats.Health.Value : (combatActorData != null ? combatActorData.stats.maxHp : 500f);
        public float Energy => _energy;
        public float MaxEnergy => Stats?.MaxMana?.Value > 0 ? Stats.MaxMana.Value : (combatActorData != null ? combatActorData.stats.maxMana : 100f);

        public float GetTotalAtk() => Stats?.AttackDamage?.Value > 0 ? Stats.AttackDamage.Value : (combatActorData != null ? combatActorData.stats.attackDamage : 50f);
        public float GetTotalAtkSpd() => Stats?.AttackSpeed?.Value > 0 ? Stats.AttackSpeed.Value : (combatActorData != null ? combatActorData.stats.attackSpeed : 0.65f);

        public void InitDamageTaker()
        {
            _hp = MaxHp;
            _energy = Stats.StartingMana.Value;
        }

        public virtual void TakeDamage(CombineDamage combineDamage)
        {
            float finalDamage = 0f;

            switch (combineDamage.damageType)
            {
                case DamageType.PhysicalDamage:
                    finalDamage = combineDamage.damage * (100f / (100f + Stats.Armor.Value));
                    break;

                case DamageType.MagicalDamage:
                    finalDamage = combineDamage.damage * (100f / (100f + Stats.MagicResist.Value));
                    break;

                case DamageType.TrueDamage:
                    finalDamage = combineDamage.damage;
                    break;
            }

            Debug.Log(DataId + $" take {finalDamage} damage");

            _hp = Mathf.Clamp(_hp - finalDamage, 0f, MaxHp);
            OnHpChanged?.Invoke(_hp / MaxHp);

            if (_hp <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log($"{gameObject.name} is death!");
        }

        #endregion

        #region Animation
        public void PlayAnim(string animName, float transitionDuration = 0.1f)
        {
            animator.CrossFadeInFixedTime(animName, transitionDuration);
        }
        public void SetAnimSpeed(float multiplier)
        {
            animator.speed = multiplier;
        }
        public bool IsAnimFinished { get; private set; }
        public bool IsCasting { get; private set; }
        public int ActionSessionId { get; private set; }

        object ICombatActor.Stats => Stats;

        public float HpRatio => MaxHp > 0f ? _hp / MaxHp : 0f;

        public bool Alive => _hp > 0f;

        public Vector3 MoveDirectionPerFrame { get; set; }
        public Vector3 MovePositionPerFrame { get; set; }

        public void ResetAnim()
        {
            IsAnimFinished = false;
            animator.speed = 1f;
        }

        public void SetCasting(bool value)
        {
            IsCasting = value;
        }
        public void OnAnimFinished()
        {
            IsAnimFinished = true;
        }

        public Vector3 GetAnchorPosition(AnchorType anchorType)
        {
            switch (anchorType)
            {
                case AnchorType.HeadPoint:
                    return headPoint.Position;
                case AnchorType.MidPoint:
                    return midPoint.Position;
                default:
                    return CachedTransform.position;
            }

        }
        public void InterruptAction()
        {
            StopAllCoroutines();
            ActionSessionId++;
            SetCasting(false);
            SetAnimSpeed(1f);
            ResetAnim();
            SetTarget(null);
            PlayAnim("Locomotion");
        }

        public void SetStaggerReduction(float v)
        {
            throw new NotImplementedException();
        }

        public void ClearTarget()
        {
            throw new NotImplementedException();
        }

        public float GetHit(CalculatedDamage damage)
        {
            if (damage == null) return 0f;

            float baseDmg = damage.FloatTrueDamage > 0f ? damage.FloatTrueDamage : damage.FloatNormalDamage;
            DamageType type = damage.FloatTrueDamage > 0f ? DamageType.TrueDamage : damage.DamageType;

            CombineDamage combineDamage = new CombineDamage(type, baseDmg);

            float finalDamage = baseDmg;
            switch (type)
            {
                case DamageType.PhysicalDamage:
                    finalDamage = baseDmg * (100f / (100f + Stats.Armor.Value));
                    break;

                case DamageType.MagicalDamage:
                    finalDamage = baseDmg * (100f / (100f + Stats.MagicResist.Value));
                    break;

                case DamageType.TrueDamage:
                    finalDamage = baseDmg;
                    break;
            }

            TakeDamage(combineDamage);

            // Invoke C# event and raise global damage event
            OnDamageTakenEvent?.Invoke(damage);
            this.Raise(new DamageTakenGlobalEvent { Target = this, Damage = damage, FinalDamage = finalDamage });

            return finalDamage;
        }

        public void Heal(IDamageDealer dealer, float amount, bool extra1 = false, bool extra2 = false, bool extra3 = false)
        {
            _hp = Mathf.Clamp(_hp + amount, 0f, MaxHp);
            OnHpChanged?.Invoke(MaxHp > 0f ? _hp / MaxHp : 1f);
        }

        public void ForceSetHp(float hp)
        {
            _hp = Mathf.Clamp(hp, 0f, MaxHp);
            OnHpChanged?.Invoke(MaxHp > 0f ? _hp / MaxHp : 0f);
        }

        void IDamageTaker.Die()
        {
            Die();
        }

        public IVariableOwner AsVariableOwner()
        {
            return this as IVariableOwner;
        }

        public IStatusEffectOwner AsStatusEffectOwner()
        {
            return this as IStatusEffectOwner;
        }

        public ITransform AsTransform()
        {
            return this;
        }

        public SkillGroup UltimateGroup => null;
        public ISkillEntity GetSkill(object val) => null;
        public int Skin => 0;
        public IDamageTaker AsDamageTaker() => this;
        public ICombatStatOwner AsCombatStatOwner() => null;
        public IAreaActor AsAreaActor() => null;
        public ICombatActor AsCombatActor() => this;
        public DamageSource GetDamageSource() => new DamageSource(this);
        public IDamageDealer AsDamageDealer() => this;
        public ITeamMember AsTeamMember() => this;
        public IHexMovable AsHexMovable() => null;
        public IMovable AsMovable() => this;
        public ISummoner AsSummoner() => null;
        public ISkillOwner AsSkillOwner() => this;
        public IPassiveOwner AsPassiveOwner() => null;
        public object AsAnimationPlayer() => this;
        public float GetHitBoxRadius() => combatActorData != null ? combatActorData.movement.radius : 0.25f;
        public float GetPushBoxRadius() => combatActorData != null ? combatActorData.movement.radius : 0.25f;

        public Transform GetTransform()
        {
            return CachedTransform;
        }

        public Transform GetTransform(object obj)
        {
            return CachedTransform;
        }

        public void ToggleMoveAgent(bool v)
        {
            if (MoveAgent != null) MoveAgent.SetEnable(v);
        }

        public void Teleport(Vector3 v)
        {
            Teleport(v, false, false);
        }

        public void Teleport(Vector3 v, bool b)
        {
            Teleport(v, b, false);
        }
        #endregion

    }

    [Serializable]
    public enum ActorMovementType
    {
        Navmesh,
        Obstacle,
        Transform,
        HexGrid,
    }

    public enum MovementPriority
    {
        Obstacle = 1000,
        Boss = 500,
        Elite = 300,
        Preserver = 60,
        Bruiser = 50,
        Executioner = 40,
        Striker = 30,
        Invoker = 20,
        Controller = 10,
        Controlled = 5,
        None = 0,
    }

    public enum AnchorType
    {
        HeadPoint,
        MidPoint,
        FootPoint,
    }

    [Serializable]
    public struct FirePointEntry
    {
        public string id;
        public Transform point;
    }
}

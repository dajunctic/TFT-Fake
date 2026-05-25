using System;
using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;
using Dajunctic.SkillSystem.Gambits;

namespace Dajunctic
{
    public class CombatActor : BaseView, ICombatActor
    {
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

        /// <summary>Set team runtime — dùng khi spawn PvE enemy để đảm bảo team đúng.</summary>
        public void SetTeam(Team newTeam) => team = newTeam;

        /// <summary>Team của kẻ địch — BT dùng để tìm target. Set bởi PveWaveSpawner (PvE) hoặc TravelSystem (PvP).</summary>
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

        public override void Initialize()
        {
            if (Initialized) return;
            base.Initialize();
            _viewLoaded = true;

            Position = CachedTransform.position;
            Forward = CachedTransform.forward;

            InitializeMoveAgent();
            Stats = new ChampionStats(combatActorData);
            InitDamageTaker();
            InitGambits();
        }

        void InitGambits()
        {
            _activeGambits.Clear();
            if (combatActorData != null && combatActorData.gambits != null)
            {
                foreach (var g in combatActorData.gambits)
                {
                    var instance = g.CreateCopy();
                    instance.Initialize(this);
                    _activeGambits.Add(instance);
                }
            }
        }

        public override void Tick()
        {
            base.Tick();

            // Clear stale MoveAgent reference if the pooled GameObject was destroyed (e.g. scene reload)
            // or if NavMeshAgent couldn't place itself on NavMesh (e.g. client spawned at wrong position)
            if (MoveAgent != null && !MoveAgent.Initialized)
            {
                MoveAgent = null;
                // Re-initialize immediately at the CURRENT transform position
                // (fixes client-side: prefab spawned at default pos, NavMesh failed, now actor is at real pos)
                Position = CachedTransform.position;
                Forward = CachedTransform.forward;
                InitializeMoveAgent();
            }

            if (Hp > 0 && !IsCasting)
            {
                EvaluateGambits();
            }

            float realSpeed = MoveAgentAlive ? MoveAgent.Velocity.magnitude : 0f;
            float normalizedSpeed = Mathf.Clamp01(realSpeed / Speed);

            animator.SetFloat("Speed", normalizedSpeed);

            SyncTransform();
            SyncEntity();
        }

        void EvaluateGambits()
        {
            foreach (var gambit in _activeGambits)
            {
                var target = gambit.condition?.Check();
                if (target != null && gambit.action != null && gambit.action.CheckCanPlay())
                {
                    SetTarget(target as CombatActor);
                    SetCasting(true);
                    gambit.action.Play(target);
                    break;
                }
            }
        }

        public override void Cleanup()
        {
            base.Cleanup();
        }

        protected virtual void OnDestroy()
        {
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

        // Safe guard: checks MoveAgent is not null AND its underlying GameObject wasn't destroyed
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
        public virtual float MaxHp => Stats.Health.Value;
        public float Energy => _energy;
        public float MaxEnergy => Stats.MaxMana.Value;

        public float GetTotalAtk() => Stats?.AttackDamage.Value ?? 0;
        public float GetTotalAtkSpd() => Stats.AttackSpeed.Value;

        public void InitDamageTaker()
        {
            _hp = MaxHp;
            _energy = Stats.StartingMana.Value;
        }

        public void TakeDamage(CombineDamage combineDamage)
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

            // Compute mitigated damage
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
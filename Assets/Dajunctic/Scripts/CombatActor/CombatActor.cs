using System;
using System.Collections.Generic;
using Dajunctic.SkillSystem.Graph;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;

namespace Dajunctic
{
    public class CombatActor : BaseView, ICombatActor
    {
        [SerializeField, Child] protected Animator animator;
        [SerializeField, Child] protected MidPoint midPoint;
        [SerializeField, Child] protected HeadPoint headPoint;
        [SerializeField] protected CombatActorData combatActorData;
        public CombatActorData CombatActorData => combatActorData;
        [SerializeField] private Team team;
        public Team CombatTeam => team;
        public int OwnerID { get; set; } = 0;

        public virtual Vector3 Position { get; private set; }
        public virtual Vector3 Forward { get; private set; }

        public bool IsViewLoaded => _viewLoaded;
        public virtual string DataId => string.Empty;
        public virtual bool CanBeTarget => true;

        public bool ActiveInHierarchy => gameObject.activeInHierarchy;
        public bool ActiveSelf => gameObject.activeSelf;

        void OnValidate() => this.ValidateRefs();
        public CombatActor CurrentTarget { get; private set; }
        public Vector3 MidPoint => midPoint.Position;
        public Vector3 HeadPoint => headPoint.Position;
        public float CombatRadius => combatActorData.movement.radius;
        public float Speed => combatActorData.movement.moveSpeed;
        public float RotateSpeed => combatActorData.movement.rotateSpeed;
        public float AtkSpd => Stats.AttackSpeed.Value;
        public ChampionStats Stats { get; private set; }
        public event Action<float> OnHpChanged;

        protected Node root = null;

        bool _viewLoaded;

        Dictionary<SkillSlot, RuntimeSkill> _skillBook = new Dictionary<SkillSlot, RuntimeSkill>();
        public override void Initialize()
        {
            if (Initialized) return;
            base.Initialize();
            _viewLoaded = true;

            Position = CachedTransform.position;
            Forward = CachedTransform.forward;

            InitializeMoveAgent();
            Stats = new ChampionStats(combatActorData);
            InitializeSkills();
            InitDamageTaker();

            SetupTree();
        }

        public override void Tick()
        {
            base.Tick();
            if (root != null)
            {
                root.Evaluate();
            }

            float realSpeed = MoveAgent != null ? MoveAgent.Velocity.magnitude : 0f;
            float normalizedSpeed = Mathf.Clamp01(realSpeed / Speed);

            animator.SetFloat("Speed", normalizedSpeed);

            SyncTransform();
            SyncEntity();
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
        public virtual bool CanMove => IsViewLoaded && MoveAgent != null && MoveAgent.CanMove;
        public bool IsMoving => MoveAgent != null && MoveAgent.IsMoving;
        public Vector3 Velocity => MoveAgent != null ? MoveAgent.Velocity : Vector3.zero;
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

        public void MoveDirection(Vector3 direction, float moveSpeed, float rotateSpeed, float deltaTime)
        {
            if (MoveAgent != null)
            {
                MoveAgent.MoveDirection(direction, moveSpeed, rotateSpeed, deltaTime);
            }
        }

        public void RotatePosition(Vector3 position, float rotateSpeed, float deltaTime, bool immediately)
        {
            RotateDirection(position - Position, rotateSpeed, deltaTime, immediately);
        }

        public void Teleport(Vector3 position, bool checkNavMesh, bool fx = false)
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
            if (MoveAgent != null && MoveAgent.Initialized && MoveAgent.IsEnabled)
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

        #region SkillGraph
        private SkillGraphRunner _skillGraphRunner;
        public SkillGraphRunner GetSkillGraphRunner()
        {
            if (_skillGraphRunner == null)
            {
                _skillGraphRunner = GetComponentInChildren<SkillGraphRunner>();
                if (_skillGraphRunner == null)
                {
                    var go = new GameObject("SkillGraphRunner");
                    go.transform.SetParent(CachedTransform);
                    _skillGraphRunner = go.AddComponent<SkillGraphRunner>();
                    _skillGraphRunner.actor = this;
                }
            }
            return _skillGraphRunner;
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
        public void ResetAnim()
        {
            IsAnimFinished = false;
            animator.speed = 1f;
        }

        public void SetCasting(bool value, SkillSlot slot)
        {
            IsCasting = value;
            if (value)
            {
                CurrentActiveSlot = slot;
            }
        }
        public void OnAnimFinished()
        {
            IsAnimFinished = true;
        }
        #endregion

        #region Skill
        public SkillSlot CurrentActiveSlot { get; private set; }
        void InitializeSkills()
        {
            _skillBook.Clear();
            if (combatActorData != null && combatActorData.skills != null)
            {
                foreach (var skillData in combatActorData.skills)
                {
                    if (skillData != null && !_skillBook.ContainsKey(skillData.slot))
                    {
                        _skillBook.Add(skillData.slot, new RuntimeSkill(skillData));
                    }
                }
            }
        }
        public RuntimeSkill GetSkill(SkillSlot slot)
        {
            return _skillBook.GetValueOrDefault(slot);
        }

        protected virtual Node CreateCombatBranch()
        {
            List<Node> skillNodes = new List<Node>();
            AddSkillNodeIfAvailable(skillNodes, SkillSlot.Ultimate);
            AddSkillNodeIfAvailable(skillNodes, SkillSlot.Skill);
            AddSkillNodeIfAvailable(skillNodes, SkillSlot.BasicAttack);

            var attackRange = Mathf.Max(Stats.AttackRange.Value, GetSkill(SkillSlot.BasicAttack)?.Data.castRange ?? 0);
            skillNodes.Add(new ChaseTargetNode(this, attackRange));

            return new Sequence(new List<Node>()
            {
                new FindTargetNode(this, attackRange),

                new SelectorWithMemory(skillNodes)
            });
        }

        protected void AddSkillNodeIfAvailable(List<Node> nodes, SkillSlot slot)
        {
            Node node = CreateSkillNode(slot);
            if (node != null)
            {
                nodes.Add(node);
            }
        }

        protected Node CreateSkillNode(SkillSlot slot)
        {
            var skill = GetSkill(slot);
            if (skill == null) return null;

            return new BaseSkillNode(this, slot);
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
            SetCasting(false, SkillSlot.BasicAttack);
            SetAnimSpeed(1f);
            ResetAnim();
            SetTarget(null);
            PlayAnim("Locomotion");
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
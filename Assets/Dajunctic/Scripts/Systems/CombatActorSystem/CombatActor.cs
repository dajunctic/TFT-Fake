using System;
using System.Collections.Generic;
using Dajunctic;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Dajunctic;

namespace Dajunctic
{
    public class CombatActor: BaseView, ICombatActor
    {
        [SerializeField, Child] protected Animator animator;
        [SerializeField] private List<FirePointEntry> firePoints;
        [SerializeField] protected LayerMask targetLayer;
        [SerializeField, Child] protected MidPoint midPoint;
        [SerializeField, Child] protected HeadPoint headPoint;
        [SerializeField] protected CombatActorData combatActorData;
        [SerializeField] private float patrolSpeed = 1f;
        [Header("Vfx")]
        public GameObject vfxFollowPrefab;
        [SerializeField] private string debugCurrentNodeName;
        public float PatrolSpeed => patrolSpeed;

        public virtual Vector3 Position {get; private set;}
        public virtual Vector3 Forward {get; private set;}

        public bool IsViewLoaded => _viewLoaded;
        public virtual string DataId => string.Empty;


        void OnValidate() => this.ValidateRefs();
        public CombatActor CurrentTarget { get; private set; }
        public Transform FirePoint { get; private set; }
        public MidPoint MidPoint => midPoint;
        public HeadPoint HeadPoint => headPoint;
        public LayerMask TargetLayer => targetLayer;
        public float Speed => combatActorData.movement.moveSpeed;
        public float RotateSpeed => combatActorData.movement.rotateSpeed;
        public float AtkSpd => combatActorData.combatStat.atkSpd;
        public event Action<float> OnHpChanged;
        
        protected Node root = null;

        bool _viewLoaded;
        VFXFollowTarget _vfxFollowInstance;
        
        Dictionary<SkillSlot, RuntimeSkill> _skillBook = new Dictionary<SkillSlot, RuntimeSkill>();
        private Dictionary<string, Transform> _firePointMap;
        public override void Initialize()
        {
            base.Initialize();
            _viewLoaded = true;
            InstantiateVfx();
            InitializedFirePoints();
            
            Position = CachedTransform.position;
            Forward = CachedTransform.forward;

            InitializeMoveAgent();
            InitializeSkills();
            
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

        protected virtual void SetupTree()
        {
        }
        
        public void SetTarget(CombatActor target)
        {
            CurrentTarget = target;
        }

        public bool HasValidTarget()
        {
            return CurrentTarget != null && CurrentTarget.gameObject.activeInHierarchy;
        }

        private void InitializedFirePoints()
        {
            _firePointMap = new Dictionary<string, Transform>();
            foreach (var entry in firePoints)
            {
                if (entry.point != null)
                {
                    if (!_firePointMap.ContainsKey(entry.id))
                    {
                        _firePointMap.Add(entry.id, entry.point);
                    }
                }
            }
        }

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
            RotateRotation(position - Position, rotateSpeed, deltaTime, immediately);
        }

        public void Teleport(Vector3 position, bool checkNavMesh, bool fx=false)
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
            
        public void RotateRotation(Vector3 direction, float rotateSpeed, float deltaTime, bool immediately)
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
            if (MoveAgent != null && MoveAgent.IsInitialized && MoveAgent.IsEnabled)
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
        private float _physicalArmor;
        private float _magicalArmor;
        private float _maxHp;
        
        public float Hp => _hp;
        public float MaxHp => _maxHp;

        public void InitDamageTaker()
        {
            _maxHp = combatActorData.attribute.maxHp;
            _hp = _maxHp;
            _physicalArmor = combatActorData.attribute.physicalArmor;
            _magicalArmor = combatActorData.attribute.magicalArmor;
        }

        public void TakeDamage(CombineDamage combineDamage)
        {
            float finalDamage = 0f;

            switch (combineDamage.damageType)
            {
                case DamageType.PhysicalDamage:
                    finalDamage = combineDamage.damage * (100f / (100f + _physicalArmor));
                    break;

                case DamageType.MagicalDamage:
                    finalDamage = combineDamage.damage * (100f / (100f + _magicalArmor));
                    break;

                case DamageType.TrueDamage:
                    finalDamage = combineDamage.damage;
                    break;
            }

            Debug.Log(DataId + $" take {finalDamage} damage");


            _hp = Mathf.Clamp(_hp - finalDamage, 0f, _maxHp);

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

        #region Timeline
        public TimelineSkill timelineSkill = new TimelineSkill();
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
        public Transform GetFirePoint(string param)
        {
            return _firePointMap.TryGetValue(param, out var tf) ? tf : transform;
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
        void InstantiateVfx()
        {
            if (vfxFollowPrefab != null)
            {
                var obj = Instantiate(vfxFollowPrefab, null); 
                _vfxFollowInstance = obj.GetComponent<VFXFollowTarget>();
                
                if (_vfxFollowInstance != null)
                {
                    _vfxFollowInstance.SetTarget(transform);
                }
            }
        }
        #endregion

        #region Skill
        public SkillSlot CurrentActiveSlot { get; private set; }
        private float _inputBufferTimer;
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

            var attackRange = Mathf.Max(combatActorData.combatStat.atkRange, GetSkill(SkillSlot.BasicAttack)?.Data.castRange ?? 0);
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

            BaseSkillNode logicNode = null;
            
            switch (skill.Data.executionType)
            {
                
                case SkillExecutionType.Timeline:
                    logicNode = new TimelineSkillNode(this, slot);
                    break;
                
                default:
                    logicNode = new BaseSkillNode(this, slot);
                    break;
            }
            return logicNode;
        }
        public void PlayFx(string fxId, PlayFxData playFxData, AnchorType anchorType)
        {
            var position = GetAnchorPosition(anchorType);
            playFxData.Position = position;
            GameManager.Instance.PlayFx(fxId, playFxData);
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
            PlayAnim("locomotion");
        }
        #endregion
        
    }

    
    [Serializable]
    public enum ActorMovementType
    {
        Navmesh,
        Obstacle,
        Transform,
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
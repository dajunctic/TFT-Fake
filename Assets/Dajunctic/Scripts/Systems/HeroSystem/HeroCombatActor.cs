using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic
{
    public class HeroCombatActor: CombatActor, IDraggable
    {
        [Header("Hero")]
        public Vector2Int CurrentBenchCoord { get; set; } = new Vector2Int(-1, -1);
        private Vector3 originalPosition;
        private Vector3 _targetPosition;
        private Vector3 _moveVelocity;
        private bool _isDragging = false;

        public void OnDragStart()
        {
            _isDragging = true;
            originalPosition = CachedTransform.position;
            _targetPosition = originalPosition;

            // Interrupt current actions (moving, attacking, etc.)
            InterruptAction();
            ForceStop();
            if (MoveAgent != null) MoveAgent.SetEnable(false);
        }

        public void OnDragUpdate(Vector3 worldPos)
        {
            _targetPosition = worldPos;
        }

        public void OnDrop(Vector3 finalPos)
        {
            _isDragging = false;
            _targetPosition = finalPos;

            // If we move from bench to field, notify BenchManager
            if (CurrentBenchCoord.x != -1)
            {
                BenchManager.Instance.RemoveHeroFromTile(CurrentBenchCoord);
                CurrentBenchCoord = new Vector2Int(-1, -1);
            }
            
            // Update the core CombatActor position state so it doesn't snap back
            Teleport(finalPos, false);
            
            if (MoveAgent != null)
            {
                MoveAgent.SetEnable(true);
                MoveAgent.Warp(finalPos);
            }
        }

        public void ResetPosition()
        {
            _isDragging = false;
            Teleport(originalPosition, false);
            
            CachedTransform.position = originalPosition;
            _targetPosition = originalPosition;
            if (MoveAgent != null)
            {
                MoveAgent.SetEnable(true);
                MoveAgent.Warp(originalPosition);
            }
        }

        protected override void SyncEntity()
        {
            // IMPORTANT: Disable the base class position sync while dragging
            // to prevent the jittering/fighting between mice position and actor logic
            if (_isDragging) return;
            base.SyncEntity();
        }

        private void Update()
        {
            if (_isDragging)
            {
                // Instant follow for responsive "sticky" feel
                Vector3 targetWithHeight = _targetPosition + Vector3.up * 0.5f;
                CachedTransform.position = targetWithHeight;
                _moveVelocity = Vector3.zero;
            }
        }

        public Transform GetTransform() => CachedTransform;

        public override string DataId => name;
        public bool IsMovingByInput { get; set; }

        Transform _cameraTransform;


        public override void Initialize()
        {
            base.Initialize();
           
            if (Camera.main != null)
            {
                _cameraTransform = Camera.main.transform;
            }      
        }

        public override MovementPriority AvoidancePriority
        {
            get
            {
                return MovementPriority.Controlled;
            }
        }
        
        protected override void SetupTree()
        {
            List<Node> rootNodes = new List<Node>();

            // 1. Combat Branch (Only active in Combat phase)
            // In Planning phase, the BT returns Failure, which is equivalent to "Idle"
            rootNodes.Add(new Sequence(new List<Node>()
            {
                new IsInPhaseNode(GameplayPhase.Combat),
                CreateCombatBranch()
            }));

            root = new Selector(rootNodes);
        }

        protected override Node CreateCombatBranch()
        {
            List<Node> skillNodes = new List<Node>();
            AddSkillNodeIfAvailable(skillNodes, SkillSlot.Ultimate);
            AddSkillNodeIfAvailable(skillNodes, SkillSlot.Skill);
            AddSkillNodeIfAvailable(skillNodes, SkillSlot.BasicAttack);
        
            List<Node> targetingNodes = new List<Node>();
            targetingNodes.Add(new FindTargetNode(this, combatActorData.combatStat.atkRange));
        
            return new Sequence(new List<Node>()
            {
                new Selector(targetingNodes),
                new SelectorWithMemory(skillNodes)
            });
        }


        public override void ListenEvents()
        {
            base.ListenEvents();

            InputManager.OnTestFirstSkill += OnTestFirstSkill;
        }

        [SerializeField, GuidReference("tl", typeof(IDummyId))] List<string> testSkillIds;

        public void OnTestFirstSkill()
        {
            this.Raise(new PlayTimelineEvent{ Id= testSkillIds[0]});
        }
    }
    
    
}

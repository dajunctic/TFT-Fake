using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic
{
    public class HeroCombatActor: CombatActor, IDraggable
    {
        [Header("Hero")]
        public Vector2Int CurrentBenchCoord { get; set; } = new Vector2Int(-1, -1);
        public Vector2Int CurrentFieldCoord { get; set; } = new Vector2Int(-1, -1);
        [field: SerializeField] public int StarLevel { get; private set; } = 1;

        private Vector3 originalPosition;
        private Vector3 _targetPosition;
        private Vector3 _moveVelocity;
        private bool _isDragging = false;
        public bool IsOnBench => CurrentBenchCoord.x != -1;
        public bool IsOnField => CurrentFieldCoord.x != -1;
        public override bool IsTargetable => IsOnField;

        private Vector2Int _originalBenchCoord;
        private Vector2Int _originalFieldCoord;

        public void SetStarLevel(int level)
        {
            StarLevel = level;
            // Visual feedback for star level (e.g., scaling up)
            float scale = 1f + (level - 1) * 0.2f;
            CachedTransform.localScale = Vector3.one * scale;
            
            // In a real project, we would also update stats here
            // combatActorData.combatStat.hp *= 1.8f; etc.
        }

        public void OnDragStart()
        {
            _isDragging = true;
            originalPosition = CachedTransform.position;
            _targetPosition = originalPosition;
            _originalBenchCoord = CurrentBenchCoord;
            _originalFieldCoord = CurrentFieldCoord;

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

            // 1. Try to snap to Bench
            if (BenchManager.Instance.TrySnapToBench(finalPos, out Vector2Int newBenchCoord))
            {
                HandleBenchDrop(newBenchCoord);
            }
            // 2. Try to snap to Field
            else if (FieldManager.Instance != null && FieldManager.Instance.TrySnapToField(finalPos, out Vector2Int newFieldCoord))
            {
                HandleFieldDrop(newFieldCoord);
            }
            // 3. Invalid drop -> Return to original
            else
            {
                ResetPosition();
            }
        }

        private void HandleBenchDrop(Vector2Int newBenchCoord)
        {
            // If dropping on same spot, just teleport
            if (IsOnBench && CurrentBenchCoord == newBenchCoord)
            {
                FinalizePlacement(BenchManager.Instance.GetWorldPosition(newBenchCoord));
                return;
            }

            var occupant = BenchManager.Instance.GetHeroAtTile(newBenchCoord);
            
            // Swap logic
            if (occupant != null && occupant != this)
            {
                // Move occupant to our old spot
                if (IsOnBench)
                {
                    SwapWithBench(occupant, CurrentBenchCoord);
                }
                else if (IsOnField)
                {
                    SwapWithField(occupant, CurrentFieldCoord);
                }
                else // From shop or somewhere else? Usually shouldn't happen here
                {
                    ResetPosition();
                    return;
                }
            }
            else
            {
                // Clear old spot
                ClearPreviousPlacement();
            }

            // Move to new spot
            CurrentBenchCoord = newBenchCoord;
            CurrentFieldCoord = new Vector2Int(-1, -1);
            BenchManager.Instance.RegisterHeroToTile(this, newBenchCoord);
            FinalizePlacement(BenchManager.Instance.GetWorldPosition(newBenchCoord));
        }

        private void HandleFieldDrop(Vector2Int newFieldCoord)
        {
            // If dropping on same spot, just teleport
            if (IsOnField && CurrentFieldCoord == newFieldCoord)
            {
                FinalizePlacement(FieldManager.Instance.GetWorldPosition(newFieldCoord));
                return;
            }

            var occupant = FieldManager.Instance.GetHeroAtTile(newFieldCoord);

            // Unit Limit Check: If moving from Bench to Field AND Field is full AND target is empty
            if (IsOnBench && !FieldManager.Instance.CanAddUnit() && occupant == null)
            {
                Debug.LogWarning("Unit limit reached! Swap with an existing unit or pull one back.");
                ResetPosition();
                return;
            }

            // Swap logic
            if (occupant != null && occupant != this)
            {
                if (IsOnBench)
                {
                    SwapWithBench(occupant, CurrentBenchCoord);
                }
                else if (IsOnField)
                {
                    SwapWithField(occupant, CurrentFieldCoord);
                }
                else
                {
                    ResetPosition();
                    return;
                }
            }
            else
            {
                // Clear old spot
                ClearPreviousPlacement();
            }

            // Move to new spot
            CurrentFieldCoord = newFieldCoord;
            CurrentBenchCoord = new Vector2Int(-1, -1);
            FieldManager.Instance.RegisterHeroToTile(this, newFieldCoord);
            FinalizePlacement(FieldManager.Instance.GetWorldPosition(newFieldCoord));
        }

        private void ClearPreviousPlacement()
        {
            if (BenchManager.Instance != null) BenchManager.Instance.UnregisterHero(this);
            if (FieldManager.Instance != null) FieldManager.Instance.UnregisterHero(this);
        }

        private void SwapWithBench(HeroCombatActor occupant, Vector2Int benchCoord)
        {
            // Occupant moves to where this unit WAS
            if (IsOnBench) BenchManager.Instance.RegisterHeroToTile(occupant, CurrentBenchCoord);
            else if (IsOnField) FieldManager.Instance.RegisterHeroToTile(occupant, CurrentFieldCoord);

            occupant.Teleport(IsOnBench ? BenchManager.Instance.GetWorldPosition(CurrentBenchCoord) : FieldManager.Instance.GetWorldPosition(CurrentFieldCoord), true);
        }

        private void SwapWithField(HeroCombatActor occupant, Vector2Int fieldCoord)
        {
            // Occupant moves to where this unit WAS
            if (IsOnBench) BenchManager.Instance.RegisterHeroToTile(occupant, CurrentBenchCoord);
            else if (IsOnField) FieldManager.Instance.RegisterHeroToTile(occupant, CurrentFieldCoord);

            occupant.Teleport(IsOnBench ? BenchManager.Instance.GetWorldPosition(CurrentBenchCoord) : FieldManager.Instance.GetWorldPosition(CurrentFieldCoord), true);
        }

        private void FinalizePlacement(Vector3 pos)
        {
            Teleport(pos, false);
            if (MoveAgent != null)
            {
                MoveAgent.SetEnable(true);
                MoveAgent.Warp(pos);
            }
        }

        public void ResetPosition()
        {
            _isDragging = false;
            // Restore original IDs
            CurrentBenchCoord = _originalBenchCoord;
            CurrentFieldCoord = _originalFieldCoord;
            
            Teleport(originalPosition, false);
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
                new ConditionNode(() => IsOnField, CreateCombatBranch())
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
            this.Raise(new PlayTimelineEvent{ Id= testSkillIds[0], Actor = this});
        }
    }
    
    
}

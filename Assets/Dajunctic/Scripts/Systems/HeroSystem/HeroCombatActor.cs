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
            if (GameSystemManager.Instance.Bench.TrySnapToBench(finalPos, out Vector2Int newBenchCoord))
            {
                HandleBenchDrop(newBenchCoord);
            }
            // 2. Try to snap to Field
            else if (GameSystemManager.Instance.Field != null && GameSystemManager.Instance.Field.TrySnapToField(finalPos, out Vector2Int newFieldCoord))
            {
                HandleFieldDrop(newFieldCoord);
            }
            // 3. Dropped outside both areas -> Sell the hero
            else
            {
                SellSelf();
            }
        }

        private void HandleBenchDrop(Vector2Int newBenchCoord)
        {
            // If dropping on same spot, just teleport
            if (IsOnBench && CurrentBenchCoord == newBenchCoord)
            {
                FinalizePlacement(GameSystemManager.Instance.Bench.GetWorldPosition(newBenchCoord));
                return;
            }

            var occupant = GameSystemManager.Instance.Bench.GetHeroAtTile(newBenchCoord);
            
            // Swap logic
            if (occupant != null && occupant != this)
            {
                SwapOccupant(occupant);
            }
            else
            {
                ClearPreviousPlacement();
            }

            // Move to new spot (RegisterHeroToTile handles coord cleanup)
            GameSystemManager.Instance.Bench.RegisterHeroToTile(this, newBenchCoord);
            FinalizePlacement(GameSystemManager.Instance.Bench.GetWorldPosition(newBenchCoord));
        }

        private void HandleFieldDrop(Vector2Int newFieldCoord)
        {
            // If dropping on same spot, just teleport
            if (IsOnField && CurrentFieldCoord == newFieldCoord)
            {
                FinalizePlacement(GameSystemManager.Instance.Field.GetWorldPosition(newFieldCoord));
                return;
            }

            var occupant = GameSystemManager.Instance.Field.GetHeroAtTile(newFieldCoord);

            // Unit Limit Check: If moving from Bench to Field AND Field is full AND target is empty
            if (IsOnBench && !GameSystemManager.Instance.Field.CanAddUnit() && occupant == null)
            {
                Debug.LogWarning("Unit limit reached! Swap with an existing unit or pull one back.");
                ResetPosition();
                return;
            }

            // Swap logic
            if (occupant != null && occupant != this)
            {
                SwapOccupant(occupant);
            }
            else
            {
                ClearPreviousPlacement();
            }

            // Move to new spot (RegisterHeroToTile handles coord cleanup)
            GameSystemManager.Instance.Field.RegisterHeroToTile(this, newFieldCoord);
            FinalizePlacement(GameSystemManager.Instance.Field.GetWorldPosition(newFieldCoord));
        }

        private void ClearPreviousPlacement()
        {
            if (GameSystemManager.Instance.Bench != null) GameSystemManager.Instance.Bench.UnregisterHero(this);
            if (GameSystemManager.Instance.Field != null) GameSystemManager.Instance.Field.UnregisterHero(this);
        }

        /// <summary>
        /// Move the occupant to where THIS unit was before dragging.
        /// RegisterHeroToTile handles cross-zone coord cleanup automatically.
        /// </summary>
        private void SwapOccupant(HeroCombatActor occupant)
        {
            if (IsOnBench)
            {
                GameSystemManager.Instance.Bench.RegisterHeroToTile(occupant, CurrentBenchCoord);
                occupant.Teleport(GameSystemManager.Instance.Bench.GetWorldPosition(CurrentBenchCoord), true);
            }
            else if (IsOnField)
            {
                GameSystemManager.Instance.Field.RegisterHeroToTile(occupant, CurrentFieldCoord);
                occupant.Teleport(GameSystemManager.Instance.Field.GetWorldPosition(CurrentFieldCoord), true);
            }
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

        /// <summary>
        /// Sell this hero: unregister, refund gold, destroy.
        /// </summary>
        private void SellSelf()
        {
            if (CombatActorData is HeroData heroData)
            {
                int refundGold = GetSellValue(heroData);

                // Unregister from all managers
                if (GameSystemManager.Instance.Bench != null) GameSystemManager.Instance.Bench.UnregisterHero(this);
                if (GameSystemManager.Instance.Field != null) GameSystemManager.Instance.Field.UnregisterHero(this);

                // Cleanup MoveAgent to prevent navigation errors
                if (MoveAgent != null)
                {
                    MoveAgent.SetEnable(false);
                    MoveAgent = null;
                }
                InterruptAction();
                ForceStop();

                // Refund gold
                if (GameSystemManager.Instance.Economy != null) GameSystemManager.Instance.Economy.AddGold(refundGold);

                Debug.Log($"Sold {heroData.displayName} ({StarLevel}★) for {refundGold} gold");
                this.Raise(new HeroSoldEvent { Hero = heroData, GoldRefunded = refundGold });
                Destroy(gameObject);
            }
            else
            {
                // Not a sellable hero, return to original position
                ResetPosition();
            }
        }

        /// <summary>
        /// Calculate sell value: 1★ = rarity, 2★ = rarity×3, 3★ = rarity×9
        /// </summary>
        private int GetSellValue(HeroData heroData)
        {
            int multiplier = (int)Mathf.Pow(3, StarLevel - 1);
            return heroData.rarity * multiplier;
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

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dajunctic
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class ChampionActor : CombatActor, IDraggable, IStatSource, IChampionUnit
    {
        [Header("Champion")]
        public Vector2Int CurrentBenchCoord { get; set; } = new Vector2Int(-1, -1);
        public Vector2Int CurrentFieldCoord { get; set; } = new Vector2Int(-1, -1);
        [field: SerializeField] public int StarLevel { get; private set; } = 1;

        private Vector3 originalPosition;
        private Vector3 _targetPosition;
        private Vector3 _moveVelocity;
        private bool _isDragging = false;
        public bool IsOnBench => CurrentBenchCoord.x != -1;
        public bool IsOnField => CurrentFieldCoord.x != -1;
        public override bool CanBeTarget => IsOnField;

        private Vector2Int _originalBenchCoord;
        private Vector2Int _originalFieldCoord;
        private CapsuleCollider _capsuleCollider;

        #region IChampionUnit
        public string UnitId => gameObject.name;
        public string ChampionId => combatActorData.Id;
        public List<ITrait> Traits
        {
            get
            {
                var data = CombatActorData as ChampionData;
                if (data == null) return new List<ITrait>();
                return data.traits.Cast<ITrait>().ToList();
            }
        }

        #endregion

        public void SetStarLevel(int level)
        {
            StarLevel = level;
            float scale = 1f + (level - 1) * 0.2f;
            CachedTransform.localScale = Vector3.one * scale;

            Stats.Health.RemoveAllModifiersFromSource(this);
            Stats.AttackDamage.RemoveAllModifiersFromSource(this);

            if (level > 1)
            {
                float multiplier = Mathf.Pow(1.8f, level - 1) - 1f;
                Stats.Health.AddModifier(new StatModifier(multiplier, StatModType.PercentMult, this, null));
                Stats.AttackDamage.AddModifier(new StatModifier(multiplier, StatModType.PercentMult, this, null));
            }

            this.Raise(new UpdateStarLevelEvent { owner = this, starLevel = level });
        }

        public void OnDragStart()
        {
            _isDragging = true;
            originalPosition = CachedTransform.position;
            _targetPosition = originalPosition;
            _originalBenchCoord = CurrentBenchCoord;
            _originalFieldCoord = CurrentFieldCoord;

            InterruptAction();
            ForceStop();
            if (MoveAgent != null) MoveAgent.SetEnable(false);

            this.Raise(new HeroDragStartedEvent { Hero = this });
        }

        public void OnDragUpdate(Vector3 worldPos)
        {
            _targetPosition = worldPos;
        }

        public void OnDrop(Vector3 finalPos)
        {
            _isDragging = false;

            if (GameSystemManager.Instance.Bench.TrySnapToBench(finalPos, out Vector2Int newBenchCoord))
            {
                HandleBenchDrop(newBenchCoord);
            }
            else if (GameSystemManager.Instance.Field != null && GameSystemManager.Instance.Field.TrySnapToField(finalPos, out Vector2Int newFieldCoord))
            {
                HandleFieldDrop(newFieldCoord);
            }
            else if (SellZoneUI.IsPointerOverSellZone)
            {
                SellSelf();
            }
            else
            {
                ResetPosition();
            }

            this.Raise(new HeroDragEndedEvent { Hero = this });
        }

        private void HandleBenchDrop(Vector2Int newBenchCoord)
        {
            if (IsOnBench && CurrentBenchCoord == newBenchCoord)
            {
                FinalizePlacement(GameSystemManager.Instance.Bench.GetWorldPosition(newBenchCoord));
                return;
            }

            var occupant = GameSystemManager.Instance.Bench.GetHeroAtTile(newBenchCoord);

            if (occupant != null && occupant != this)
            {
                SwapOccupant(occupant);
            }
            else
            {
                ClearPreviousPlacement();
            }

            GameSystemManager.Instance.Bench.RegisterHeroToTile(this, newBenchCoord);
            FinalizePlacement(GameSystemManager.Instance.Bench.GetWorldPosition(newBenchCoord));
        }

        private void HandleFieldDrop(Vector2Int newFieldCoord)
        {
            if (IsOnField && CurrentFieldCoord == newFieldCoord)
            {
                FinalizePlacement(GameSystemManager.Instance.Field.GetWorldPosition(newFieldCoord));
                return;
            }

            var occupant = GameSystemManager.Instance.Field.GetHeroAtTile(newFieldCoord);

            if (IsOnBench && !GameSystemManager.Instance.Field.CanAddUnit() && occupant == null)
            {
                Debug.LogWarning("Unit limit reached! Swap with an existing unit or pull one back.");
                ResetPosition();
                return;
            }

            if (occupant != null && occupant != this)
            {
                SwapOccupant(occupant);
            }
            else
            {
                ClearPreviousPlacement();
            }

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
        private void SwapOccupant(ChampionActor occupant)
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

        private void SellSelf()
        {
            if (CombatActorData is ChampionData heroData)
            {
                int refundGold = GetSellValue(heroData);

                if (GameSystemManager.Instance.Bench != null) GameSystemManager.Instance.Bench.UnregisterHero(this);
                if (GameSystemManager.Instance.Field != null) GameSystemManager.Instance.Field.UnregisterHero(this);

                if (MoveAgent != null)
                {
                    MoveAgent.SetEnable(false);
                    MoveAgent = null;
                }
                InterruptAction();
                ForceStop();

                var container = GetComponent<ItemContainer>();
                if (container != null)
                {
                    var items = container.RemoveAllItems();
                    var itemSystem = GameSystemManager.Instance.Items;
                    if (itemSystem != null)
                    {
                        foreach (var item in items)
                        {
                            itemSystem.AddItemToBench(item);
                        }
                    }
                }

                if (GameSystemManager.Instance.Economy != null) GameSystemManager.Instance.Economy.AddGold(refundGold);

                Debug.Log($"Sold {heroData.displayName} ({StarLevel}★) for {refundGold} gold");
                this.Raise(new HeroSoldEvent { Hero = heroData, GoldRefunded = refundGold });
                Destroy(gameObject);
            }
            else
            {
                ResetPosition();
            }
        }

        /// <summary>
        /// Calculate sell value: 1★ = rarity, 2★ = rarity×3, 3★ = rarity×9 
        /// </summary>
        public int GetSellValue()
        {
            if (CombatActorData is ChampionData heroData)
            {
                return GetSellValue(heroData);
            }
            return 0;
        }

        /// <summary>
        /// Calculate sell value: 1★ = rarity, 2★ = rarity×3, 3★ = rarity×9
        /// </summary>
        public int GetSellValue(ChampionData heroData)
        {
            if (StarLevel == 1) return heroData.rarity;

            int totalCost = (int)Mathf.Pow(3, StarLevel - 1) * heroData.rarity;

            if (StarLevel == 2)
            {
                return (heroData.rarity == 1) ? 3 : (totalCost - 1);
            }
            else if (StarLevel >= 3)
            {
                if (heroData.rarity == 1) return 5;
                return totalCost - (heroData.rarity * 2);
            }

            return totalCost;
        }

        protected override void SyncEntity()
        {
            if (_isDragging) return;
            base.SyncEntity();
        }

        public override void Tick()
        {
            base.Tick();
            if (_isDragging)
            {
                Vector3 targetWithHeight = _targetPosition + Vector3.up * 0.5f;
                CachedTransform.position = targetWithHeight;
                _moveVelocity = Vector3.zero;
            }
        }

        public Transform GetTransform() => CachedTransform;

        public override string DataId => name;

        public override void Initialize()
        {
            if (Initialized) return;
            base.Initialize();

            this.Raise(new SpawnHpViewEvent { owner = this, starLevel = StarLevel });

            _capsuleCollider = GetComponent<CapsuleCollider>();

            _capsuleCollider.radius = combatActorData.movement.radius;
            _capsuleCollider.height = combatActorData.movement.height;
            _capsuleCollider.center = Vector3.zero + Vector3.up * combatActorData.movement.height / 2f;
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
            List<Node> rootNodes = new List<Node>
            {
                new Sequence(new List<Node>()
                {
                    new IsInPhaseNode(GameplayPhase.Combat),
                    new ConditionNode(() => IsOnField, CreateCombatBranch())
                })
            };

            root = new Selector(rootNodes);
        }

        protected override Node CreateCombatBranch()
        {
            List<Node> skillNodes = new List<Node>();
            AddSkillNodeIfAvailable(skillNodes, SkillSlot.Ultimate);
            AddSkillNodeIfAvailable(skillNodes, SkillSlot.Skill);
            AddSkillNodeIfAvailable(skillNodes, SkillSlot.BasicAttack);

            List<Node> targetingNodes = new List<Node>
            {
                new FindTargetNode(this, Stats.AttackRange.Value)
            };

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

        public void OnTestFirstSkill()
        {
            if (CombatActorData != null && CombatActorData.skills.Count > 0)
            {
                var firstSkill = CombatActorData.skills[0];
                if (firstSkill != null && firstSkill.skillGraph != null)
                {
                    var runner = GetSkillGraphRunner();
                    runner.graph = firstSkill.skillGraph;
                    runner.Run();
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }


}

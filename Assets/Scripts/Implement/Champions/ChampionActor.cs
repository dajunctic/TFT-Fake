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

        protected override void Awake()
        {

            if (Application.isPlaying && TickerView.Instance != null)
            {
                ticker = TickerView.Instance.ticker;
            }
        }

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

            if (GameSystemManager.Instance.Bench.TrySnapToBench(finalPos, out Vector2Int newBenchCoord, out int benchArenaId))
            {
                
                if (benchArenaId == OwnerID)
                    HandleBenchDrop(newBenchCoord, benchArenaId);
                else
                    ResetPosition();
            }
            else if (GameSystemManager.Instance.Field != null && GameSystemManager.Instance.Field.TrySnapToField(finalPos, out Vector2Int newFieldCoord, out int fieldArenaId))
            {
                if (fieldArenaId == OwnerID)
                    HandleFieldDrop(newFieldCoord, fieldArenaId);
                else
                    ResetPosition();
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

        private void HandleBenchDrop(Vector2Int newBenchCoord, int arenaId)
        {
            if (IsOnBench && CurrentBenchCoord == newBenchCoord)
            {
                FinalizePlacement(GameSystemManager.Instance.Bench.GetWorldPosition(arenaId, newBenchCoord), arenaId);
                return;
            }

            var occupant = GameSystemManager.Instance.Bench.GetHeroAtTile(arenaId, newBenchCoord);

            if (occupant != null && occupant != this)
            {
                SwapOccupant(occupant, arenaId);
            }
            else
            {
                ClearPreviousPlacement();
            }

            GameSystemManager.Instance.Bench.RegisterHeroToTile(this, newBenchCoord, arenaId);
            FinalizePlacement(GameSystemManager.Instance.Bench.GetWorldPosition(arenaId, newBenchCoord), arenaId);
        }

        private void HandleFieldDrop(Vector2Int newFieldCoord, int arenaId)
        {
            if (IsOnField && CurrentFieldCoord == newFieldCoord)
            {
                FinalizePlacement(GameSystemManager.Instance.Field.GetWorldPosition(arenaId, newFieldCoord), arenaId);
                return;
            }

            var occupant = GameSystemManager.Instance.Field.GetHeroAtTile(arenaId, newFieldCoord);

            if (IsOnBench && !GameSystemManager.Instance.Field.CanAddUnit(OwnerID) && occupant == null)
            {
                Debug.LogWarning("Unit limit reached! Swap with an existing unit or pull one back.");
                ResetPosition();
                return;
            }

            if (occupant != null && occupant != this)
            {
                SwapOccupant(occupant, arenaId);
            }
            else
            {
                ClearPreviousPlacement();
            }

            GameSystemManager.Instance.Field.RegisterHeroToTile(this, newFieldCoord, arenaId);
            FinalizePlacement(GameSystemManager.Instance.Field.GetWorldPosition(arenaId, newFieldCoord), arenaId);

            if (Gameplay.Instance != null && Gameplay.Instance.CurrentPhase == GameplayPhase.Combat
                && PveWaveSpawner.Instance != null)
            {
                var enemyTeam = PveWaveSpawner.Instance.GetEnemyTeamForArena(arenaId);
                if (enemyTeam != null)
                {
                    SetEnemyTeam(enemyTeam);
                    Debug.Log($"<color=lime>[ChampionActor] {name} deployed mid-combat → EnemyTeam set ({enemyTeam.Members.Count} enemies)</color>");
                }
            }
        }

        private void ClearPreviousPlacement()
        {
            if (GameSystemManager.Instance.Bench != null) GameSystemManager.Instance.Bench.UnregisterHero(this);
            if (GameSystemManager.Instance.Field != null) GameSystemManager.Instance.Field.UnregisterHero(this);
        }

        private void SwapOccupant(ChampionActor occupant, int arenaId)
        {
            if (IsOnBench)
            {
                GameSystemManager.Instance.Bench.RegisterHeroToTile(occupant, CurrentBenchCoord, arenaId);
                occupant.Teleport(GameSystemManager.Instance.Bench.GetWorldPosition(arenaId, CurrentBenchCoord), true);
            }
            else if (IsOnField)
            {
                GameSystemManager.Instance.Field.RegisterHeroToTile(occupant, CurrentFieldCoord, arenaId);
                occupant.Teleport(GameSystemManager.Instance.Field.GetWorldPosition(arenaId, CurrentFieldCoord), true);
            }
        }

        private void FinalizePlacement(Vector3 pos, int arenaId)
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

                Debug.Log($"Sold {heroData.displayName} ({StarLevel}★) for {refundGold} gold");
                this.Raise(new HeroSoldEvent { Hero = heroData, GoldRefunded = refundGold });
                Destroy(gameObject);
            }
            else
            {
                ResetPosition();
            }
        }

        public int GetSellValue()
        {
            if (CombatActorData is ChampionData heroData)
            {
                return GetSellValue(heroData);
            }
            return 0;
        }

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

        public override string DataId => name;

        public override void Initialize()
        {
            if (Initialized) return;
            base.Initialize();

            var hpView = GetComponentInChildren<HpView>(true);
            if (hpView != null)
            {
                hpView.Initialize(this, StarLevel);
                hpView.gameObject.SetActive(true);
            }
            else
            {
                this.Raise(new SpawnHpViewEvent { owner = this, starLevel = StarLevel });
            }

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

        protected override void EvaluateGambits()
        {
            if (IsOnBench) return;
            base.EvaluateGambits();
        }

        public override void ListenEvents()
        {
            base.ListenEvents();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }

}

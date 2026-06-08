using System;
using UnityEngine;
using System.Linq;
using FishNet.Object;
using FishNet.Object.Synchronizing;
namespace Dajunctic
{
    public partial class Gameplay : NetworkBehaviour
    {
        public static string HeroLayerName = "CombatActor";
        public static Gameplay Instance { get; private set; }

        [Header("Phase Settings")]
        [SerializeField] private float planningDuration = 10f;
        [SerializeField] private float combatDuration = 30f;

        [Header("Debug")]
        [SerializeField] private bool debugFastMode = false;
        [SerializeField] private float fastModeDuration = 5f;

        private RoundSystem _roundSystem;
        private RoundSystem RoundSys => _roundSystem ?? (_roundSystem = GameSystemManager.Instance.Round);

        private PveWaveSpawner _pveSpawner;
        private PveWaveSpawner PveSpawner => _pveSpawner != null ? _pveSpawner : (_pveSpawner = GetComponent<PveWaveSpawner>() ?? gameObject.AddComponent<PveWaveSpawner>());

        private readonly SyncVar<GameplayPhase> _currentPhase = new SyncVar<GameplayPhase>();
        private readonly SyncVar<float> _timer = new SyncVar<float>();
        private readonly SyncVar<float> _phaseDuration = new SyncVar<float>();

        public GameplayPhase CurrentPhase => _currentPhase.Value;
        public float Timer => _timer.Value;
        public float PhaseDuration => _phaseDuration.Value;

        public static event Action<GameplayPhase> OnPhaseChanged;

        protected void Awake()
        {
            Instance = this;
            _currentPhase.OnChange += OnPhaseChangedCallback;
        }

        private void OnDestroy()
        {
            _currentPhase.OnChange -= OnPhaseChangedCallback;
            if (Instance == this) Instance = null;
        }

        private void OnPhaseChangedCallback(GameplayPhase prev, GameplayPhase next, bool asServer)
        {
            if (!asServer)
            {
                OnPhaseChanged?.Invoke(next);
                this.Raise(new GameplayPhaseChangedEvent { Phase = next });
            }
        }

        public void Initialize()
        {
            
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            if (GameSystemManager.Instance?.Player?.Players.Count > 0)
            {
                StartPhaseServer(GameplayPhase.Planning);
            }
            else
            {
                PlayerSystem.OnPlayerListInitialized += OnPlayersReady;
            }
        }

        private void OnPlayersReady()
        {
            PlayerSystem.OnPlayerListInitialized -= OnPlayersReady;
            StartPhaseServer(GameplayPhase.Planning);
        }

        private float _combatCheckTimer;

        void Update()
        {
            if (!IsServerInitialized) return; 

            // Check if combat should end early (one team eliminated)
            if (_currentPhase.Value == GameplayPhase.Combat)
            {
                _combatCheckTimer -= Time.deltaTime;
                if (_combatCheckTimer <= 0f)
                {
                    _combatCheckTimer = 0.5f;
                    if (CheckCombatFinished())
                    {
                        _timer.Value = 0;
                        OnTimerCompleteServer();
                        return;
                    }
                }
            }

            if (_timer.Value > 0)
            {
                _timer.Value -= Time.deltaTime;
                if (_timer.Value <= 0)
                {
                    OnTimerCompleteServer();
                }
            }
        }

        private bool CheckCombatFinished()
        {
            var roundData = RoundSys?.CurrentRoundData;
            bool isPvE = roundData != null &&
                         (roundData.roundType == RoundType.PvE_Minion ||
                          roundData.roundType == RoundType.PvE_Boss);

            if (isPvE)
            {
                return PveSpawner.GetTotalAliveEnemies() <= 0;
            }

            var travelSystem = GameSystemManager.Instance?.Travel;
            if (travelSystem == null) return false;

            var combatPairs = travelSystem.GetCombatPairs();
            if (combatPairs == null || combatPairs.Count == 0) return false;

            foreach (var pair in combatPairs)
            {
                bool homeAlive = CombatActor.ActiveActors
                    .OfType<ChampionActor>()
                    .Any(u => u.OwnerID == pair.HomeId && u.IsOnField && u.Alive && u.gameObject.activeInHierarchy);
                bool guestAlive = CombatActor.ActiveActors
                    .OfType<ChampionActor>()
                    .Any(u => u.OwnerID == pair.GuestId && u.Alive && u.gameObject.activeInHierarchy);

                if (homeAlive && guestAlive)
                    return false;
            }
            return true;
        }

        private void StartPhaseServer(GameplayPhase phase)
        {
            _currentPhase.Value = phase;

            float duration = (phase == GameplayPhase.Planning) ? planningDuration : combatDuration;

            if (RoundSys != null && RoundSys.CurrentRoundData != null)
            {
                duration = (phase == GameplayPhase.Planning)
                    ? RoundSys.CurrentRoundData.planningDuration
                    : RoundSys.CurrentRoundData.combatDuration;
            }

            if (debugFastMode)
            {
                duration = fastModeDuration;
            }

            _phaseDuration.Value = duration;
            _timer.Value = _phaseDuration.Value;

            Debug.Log($"[Gameplay] Starting Phase: {phase} for {_phaseDuration.Value}s in round {(RoundSys != null ? RoundSys.GetRoundDisplayString() : "N/A")}");

            if (phase == GameplayPhase.Planning)
            {
                // Return all traveling units to their home arenas first
                if (GameSystemManager.Instance.Travel != null)
                {
                    GameSystemManager.Instance.Travel.ReturnAllUnits();
                }

                // Reset all combat actors for new round (heal, reactivate, reset mana)
                foreach (var actor in CombatActor.ActiveActors.ToArray())
                {
                    if (actor != null)
                    {
                        actor.ResetForNewRound();
                    }
                }

                var playerSyncs = FindObjectsByType<PlayerDataSync>(FindObjectsSortMode.None);
                foreach (var sync in playerSyncs)
                    sync.ServerRollShop();

                ShowPlanningPopupRpc();
            }

            if (phase == GameplayPhase.Carousel)
            {
                if (GameSystemManager.Instance.Carousel != null)
                {
                    GameSystemManager.Instance.Carousel.StartCarousel();
                }
            }

            if (phase == GameplayPhase.Combat)
            {
                _combatCheckTimer = 2f; // Wait 2s before checking combat end
                var playerSystem = GameSystemManager.Instance.Player;
                if (playerSystem != null)
                {
                    foreach (var player in playerSystem.Players)
                    {
                        if (player.HP > 0)
                        {
                            AutoDeployBenchUnits(player.Id);
                        }
                    }
                }

                var roundData = RoundSys?.CurrentRoundData;
                bool isPvE = roundData != null &&
                             (roundData.roundType == RoundType.PvE_Minion ||
                              roundData.roundType == RoundType.PvE_Boss);

                if (isPvE)
                {
                    
                    PveSpawner.SpawnWaveForAllArenas(roundData);
                    Debug.Log($"[Gameplay] PvE Combat started — round {RoundSys.GetRoundDisplayString()}");
                }
                else
                {
                    
                    PveSpawner.ClearAllEnemies();
                    // PvP travel is handled by TravelSystem.OnPhaseChanged (via GameplayPhaseChangedEvent)
                }
            }

            OnPhaseChanged?.Invoke(phase);
            this.Raise(new GameplayPhaseChangedEvent { Phase = phase });
        }

        [ObserversRpc(RunLocally = true, BufferLast = true)]
        private void ShowPlanningPopupRpc()
        {
            this.Raise(new ShowPopupEvent { PopupType = typeof(GameplayPopup), ShowMode = PopupShowMode.DoNothing });
        }

        private void OnTimerCompleteServer()
        {
            if (_currentPhase.Value == GameplayPhase.Carousel)
            {
                
                AdvanceToNextRound();
                return;
            }

            if (_currentPhase.Value == GameplayPhase.Planning)
            {
                StartPhaseServer(GameplayPhase.Combat);
            }
            else
            {
                HandleCombatResultServer();
                AdvanceToNextRound();
            }
        }

        private void AdvanceToNextRound()
        {
            // Apply end-of-round income and passive XP to all players (TFT mechanic)
            var playerSyncs = FindObjectsByType<PlayerDataSync>(FindObjectsSortMode.None);
            foreach (var sync in playerSyncs)
            {
                sync.ApplyEndRoundIncome();
                sync.ChangeExp(2); // Passive +2 XP per round
            }

            if (RoundSys != null)
            {
                RoundSys.AdvanceRound();

                // Sync round state to all clients
                SyncRoundStateRpc(RoundSys.StageNumber, RoundSys.RoundNumber);
                
                if (RoundSys.CurrentRoundData != null && RoundSys.CurrentRoundData.roundType == RoundType.Carousel)
                {
                    StartPhaseServer(GameplayPhase.Carousel);
                }
                else
                {
                    StartPhaseServer(GameplayPhase.Planning);
                }
            }
            else
            {
                StartPhaseServer(GameplayPhase.Planning);
            }
        }

        [ObserversRpc(RunLocally = false)]
        private void SyncRoundStateRpc(int stageNumber, int roundNumber)
        {
            var roundSystem = GameSystemManager.Instance?.Round;
            if (roundSystem != null)
            {
                roundSystem.SetRoundState(stageNumber, roundNumber);
            }
        }

        private void HandleCombatResultServer()
        {
            var roundData = RoundSys?.CurrentRoundData;
            bool isPvE = roundData != null &&
                         (roundData.roundType == RoundType.PvE_Minion ||
                          roundData.roundType == RoundType.PvE_Boss);

            if (isPvE)
            {
                HandlePvECombatResult(roundData);
            }
            else
            {
                HandlePvPCombatResult();
            }

            PveSpawner.ClearAllEnemies();
        }

        private void HandlePvECombatResult(RoundData roundData)
        {
            int surviving = PveSpawner.GetTotalAliveEnemies();
            if (surviving <= 0)
            {
                Debug.Log("[Gameplay] PvE: All enemies killed — players take no damage.");
                return;
            }

            var playerSystem = GameSystemManager.Instance.Player;
            if (playerSystem == null) return;

            int stageDamage = (RoundSys != null) ? RoundSys.StageNumber + 1 : 2;
            int totalDamage = stageDamage + surviving;

            foreach (var player in playerSystem.Players)
            {
                playerSystem.ApplyDamage(player.Id, totalDamage);
                Debug.Log($"[Gameplay] PvE: Player {player.Id} takes {totalDamage} damage ({surviving} enemies survived).");
            }
        }

        private void HandlePvPCombatResult()
        {
            var playerSystem = GameSystemManager.Instance.Player;
            var travelSystem = GameSystemManager.Instance.Travel;
            if (playerSystem == null || travelSystem == null) return;

            var combatPairs = travelSystem.GetCombatPairs();
            int stageDamage = (RoundSys != null) ? RoundSys.StageNumber + 1 : 2;

            foreach (var pair in combatPairs)
            {
                // Query active champion actors for this combat pair
                var homeUnits = CombatActor.ActiveActors
                    .OfType<ChampionActor>()
                    .Where(u => u.OwnerID == pair.HomeId && u.Alive && u.gameObject.activeInHierarchy)
                    .ToList();
                var guestUnits = CombatActor.ActiveActors
                    .OfType<ChampionActor>()
                    .Where(u => u.OwnerID == pair.GuestId && u.Alive && u.gameObject.activeInHierarchy)
                    .ToList();

                var homeSync = playerSystem.GetPlayerSync(pair.HomeId);
                var guestSync = playerSystem.GetPlayerSync(pair.GuestId);

                if (homeUnits.Count == 0 && guestUnits.Count > 0)
                {
                    // Home player lost — damage based on surviving unit cost (TFT mechanic)
                    int unitDamage = guestUnits.Sum(u => (u.CombatActorData as ChampionData)?.rarity ?? 1);
                    int damage = stageDamage + unitDamage;
                    playerSystem.ApplyDamage(pair.HomeId, damage);
                    if (homeSync != null) homeSync.RegisterResult(false);
                    if (guestSync != null) guestSync.RegisterResult(true);
                    Debug.Log($"[Gameplay] Player {pair.HomeId} lost PvP. Taking {damage} damage (stage:{stageDamage} + units:{unitDamage}).");
                }
                else if (guestUnits.Count == 0 && homeUnits.Count > 0)
                {
                    // Guest player lost
                    int unitDamage = homeUnits.Sum(u => (u.CombatActorData as ChampionData)?.rarity ?? 1);
                    int damage = stageDamage + unitDamage;
                    playerSystem.ApplyDamage(pair.GuestId, damage);
                    if (guestSync != null) guestSync.RegisterResult(false);
                    if (homeSync != null) homeSync.RegisterResult(true);
                    Debug.Log($"[Gameplay] Player {pair.GuestId} lost PvP as guest. Taking {damage} damage (stage:{stageDamage} + units:{unitDamage}).");
                }
                else
                {
                    // Draw or both wiped
                    if (homeSync != null) homeSync.RegisterResult(false);
                    if (guestSync != null) guestSync.RegisterResult(false);
                }
            }
        }

        private void AutoDeployBenchUnits(int ownerId)
        {
            var benchSystem = GameSystemManager.Instance.Bench;
            var fieldSystem = GameSystemManager.Instance.Field;
            var playerSystem = GameSystemManager.Instance.Player;
            if (benchSystem == null || fieldSystem == null || playerSystem == null) return;

            var playerSync = playerSystem.GetPlayerSync(ownerId);
            if (playerSync == null) return;

            int level = playerSync.Level.Value;
            int currentOnField = fieldSystem.GetUnitCount(ownerId);
            if (currentOnField >= level) return;

            int needed = level - currentOnField;
            var benchHeroes = benchSystem.GetHeroesOnBench(ownerId);
            if (benchHeroes.Count == 0) return;

            var arena = fieldSystem.GetArena(ownerId);
            if (arena == null || arena.FieldArea == null || arena.FieldArea.Data == null) return;

            var allFieldTiles = arena.FieldArea.Data.ActiveTiles.Select(t => t.coordinates).ToList();
            var emptyFieldTiles = allFieldTiles.Where(c => fieldSystem.GetHeroAtTile(ownerId, c) == null).ToList();

            int deployedCount = 0;
            for (int i = 0; i < benchHeroes.Count && deployedCount < needed && i < emptyFieldTiles.Count; i++)
            {
                var actor = benchHeroes[i];
                var targetCoord = emptyFieldTiles[i];

                fieldSystem.RegisterHeroToTile(actor, targetCoord, ownerId);

                Vector3 targetPos = arena.GetFieldWorldPosition(targetCoord);
                actor.Teleport(targetPos, false);
                if (actor.MoveAgent != null)
                {
                    actor.MoveAgent.SetEnable(true);
                    actor.MoveAgent.Warp(targetPos);
                }

                var netSync = actor.GetComponent<ChampionNetworkSync>();
                if (netSync != null)
                {
                    netSync.RpcUpdateCoordinates(new Vector2Int(-1, -1), targetCoord, targetPos);
                }

                deployedCount++;
            }
        }
    }

    public enum GameplayPhase
    {
        Planning,
        Combat,
        Carousel
    }

    public struct GameplayPhaseChangedEvent : IEvent
    {
        public GameplayPhase Phase;
    }

    public enum Team
    {
        Player,
        Opponent,
        PlayerMysthicalAnimal,
        OpponetMysthicalAnimal
    }
}

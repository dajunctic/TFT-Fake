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

        void Update()
        {
            if (!IsServerInitialized) return; 

            if (_timer.Value > 0)
            {
                _timer.Value -= Time.deltaTime;
                if (_timer.Value <= 0)
                {
                    OnTimerCompleteServer();
                }
            }
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
                foreach (var actor in CombatActor.ActiveActors.ToArray())
                {
                    if (actor != null)
                    {
                        actor.ForceSetHp(actor.MaxHp);
                    }
                }

                if (GameSystemManager.Instance.Travel != null)
                {
                    GameSystemManager.Instance.Travel.ReturnAllUnits();
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
            var fieldSystem = GameSystemManager.Instance.Field;
            if (playerSystem == null || travelSystem == null || fieldSystem == null) return;

            var combatPairs = travelSystem.GetCombatPairs();
            int stageDamage = (RoundSys != null) ? RoundSys.StageNumber + 1 : 2;

            foreach (var pair in combatPairs)
            {
                var allUnitsOnArena = fieldSystem.GetAllHeroes().Where(u =>
                    fieldSystem.GetHeroAtTile(pair.HomeId, u.CurrentFieldCoord) == u).ToList();

                var homeUnits = allUnitsOnArena.Where(u => u.OwnerID == pair.HomeId).ToList();
                var guestUnits = allUnitsOnArena.Where(u => u.OwnerID == pair.GuestId).ToList();

                if (homeUnits.Count == 0 && guestUnits.Count > 0)
                {
                    int damage = stageDamage + guestUnits.Count;
                    playerSystem.ApplyDamage(pair.HomeId, damage);
                    Debug.Log($"[Gameplay] Player {pair.HomeId} lost PvP. Taking {damage} damage.");
                }
                else if (guestUnits.Count == 0 && homeUnits.Count > 0)
                {
                    int damage = stageDamage + homeUnits.Count;
                    playerSystem.ApplyDamage(pair.GuestId, damage);
                    Debug.Log($"[Gameplay] Player {pair.GuestId} lost PvP as guest. Taking {damage} damage.");
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


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
        }

        public void Initialize()
        {
            // StartPhase(GameplayPhase.Planning);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            // Chờ PlayerSystem spawn xong PlayerDataSync trước khi bắt đầu phase đầu tiên.
            // Nếu list đã có sẵn (edge case) thì gọi luôn.
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
            if (!IsServerInitialized) return; // Only the server should control the phase timing

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
                // Return everyone home when planning starts
                if (GameSystemManager.Instance.Travel != null)
                {
                    GameSystemManager.Instance.Travel.ReturnAllUnits();
                }

                // Roll shops FIRST so CurrentShop is populated before the popup opens.
                // Previously used a coroutine (yield 1 frame) which caused a race condition:
                // popup would open with empty slots before shop data arrived.
                var playerSyncs = FindObjectsByType<PlayerDataSync>(FindObjectsSortMode.None);
                foreach (var sync in playerSyncs)
                    sync.ServerRollShop();

                // Show planning popup AFTER shops are rolled
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
                // Start matchmaking and travel when combat starts
                if (GameSystemManager.Instance.Travel != null)
                {
                    GameSystemManager.Instance.Travel.GenerateMatchmaking();
                    GameSystemManager.Instance.Travel.ExecuteTravelCards();
                }
            }

            OnPhaseChanged?.Invoke(phase);
            this.Raise(new GameplayPhaseChangedEvent { Phase = phase });
        }

        // NOTE: RollShopsNextFrame coroutine removed.
        // Shops are now rolled synchronously before ShowPlanningPopupRpc() is called
        // to avoid the race condition where popup opened before shop data was ready.

        [ObserversRpc(RunLocally = true, BufferLast = true)]
        private void ShowPlanningPopupRpc()
        {
            this.Raise(new ShowPopupEvent { PopupType = typeof(GameplayPopup), ShowMode = PopupShowMode.DoNothing });
        }

        private void OnTimerCompleteServer()
        {
            if (_currentPhase.Value == GameplayPhase.Carousel)
            {
                // Carousel ends, go to Planning or next round
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

        private void HandleCombatResultServer()
        {
            var playerSystem = GameSystemManager.Instance.Player;
            var travelSystem = GameSystemManager.Instance.Travel;
            var fieldSystem = GameSystemManager.Instance.Field;
            if (playerSystem == null || travelSystem == null || fieldSystem == null) return;

            var combatPairs = travelSystem.GetCombatPairs();
            int stageDamage = (RoundSys != null) ? RoundSys.StageNumber + 1 : 2;

            foreach (var pair in combatPairs)
            {
                // Each pair represents a combat on pair.HomeId arena
                var allUnitsOnArena = fieldSystem.GetAllHeroes().Where(u => 
                    fieldSystem.GetHeroAtTile(pair.HomeId, u.CurrentFieldCoord) == u).ToList();

                var homeUnits = allUnitsOnArena.Where(u => u.OwnerID == pair.HomeId).ToList();
                var guestUnits = allUnitsOnArena.Where(u => u.OwnerID == pair.GuestId).ToList();

                if (homeUnits.Count == 0 && guestUnits.Count > 0)
                {
                    // Home lost
                    int damage = stageDamage + guestUnits.Count;
                    playerSystem.ApplyDamage(pair.HomeId, damage); 
                    // if (pair.HomeId == 0) GameSystemManager.Instance.Economy?.RegisterResult(false);
                    Debug.Log($"[Gameplay] Player {pair.HomeId} lost combat on home arena. Taking {damage} damage.");
                }
                else if (guestUnits.Count == 0 && homeUnits.Count > 0)
                {
                    // Guest lost (Home won)
                    int damage = stageDamage + homeUnits.Count;
                    playerSystem.ApplyDamage(pair.GuestId, damage);
                    // if (pair.HomeId == 0) GameSystemManager.Instance.Economy?.RegisterResult(true);
                    Debug.Log($"[Gameplay] Player {pair.GuestId} lost combat as guest. Taking {damage} damage.");
                }
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

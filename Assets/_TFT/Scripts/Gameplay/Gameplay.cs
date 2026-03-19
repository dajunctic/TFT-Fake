
using System;
using UnityEngine;
using System.Linq;

namespace Dajunctic
{
    public class Gameplay : BaseView
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

        private GameplayPhase _currentPhase;
        private float _timer;
        private float _phaseDuration;

        public GameplayPhase CurrentPhase => _currentPhase;
        public float Timer => _timer;
        public float PhaseDuration => _phaseDuration;

        public static event Action<GameplayPhase> OnPhaseChanged;

        protected override void Awake()
        {
            base.Awake();
            Instance = this;
        }

        public override void Initialize()
        {
            base.Initialize();
            // StartPhase(GameplayPhase.Planning);
        }

        void Start()
        {
            StartPhase(GameplayPhase.Planning);
            var economySystem = this.GetSystem<EconomySystem>();
            if (economySystem != null)
            {
                economySystem.AddGold(1000); // Initial gold for testing
            }
        }

        public override void Tick()
        {
            base.Tick();

            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                {
                    OnTimerComplete();
                }
            }
        }

        private void StartPhase(GameplayPhase phase)
        {
            _currentPhase = phase;

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

            _phaseDuration = duration;
            _timer = _phaseDuration;

            Debug.Log($"[Gameplay] Starting Phase: {phase} for {_phaseDuration}s in round {(RoundSys != null ? RoundSys.GetRoundDisplayString() : "N/A")}");

            if (phase == GameplayPhase.Planning)
            {
                this.Raise(new ShowPopupEvent { PopupType = typeof(GameplayPopup), ShowMode = PopupShowMode.DoNothing });
                
                // Return everyone home when planning starts
                if (GameSystemManager.Instance.Travel != null)
                {
                    GameSystemManager.Instance.Travel.ReturnAllUnits();
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

        private void OnTimerComplete()
        {
            if (_currentPhase == GameplayPhase.Planning)
            {
                StartPhase(GameplayPhase.Combat);
            }
            else
            {
                // Logic for after combat: Advance Round then back to planning
                if (RoundSys != null)
                {
                    HandleCombatResult();
                    RoundSys.AdvanceRound();
                }

                StartPhase(GameplayPhase.Planning);
            }
        }

        private void HandleCombatResult()
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
                    playerSystem.ApplyDamage(pair.HomeId == 0 ? Team.Player : Team.Opponent, damage); // Assuming Team enum is simple
                    if (pair.HomeId == 0) GameSystemManager.Instance.Economy?.RegisterResult(false);
                    Debug.Log($"[Gameplay] Player {pair.HomeId} lost combat on home arena. Taking {damage} damage.");
                }
                else if (guestUnits.Count == 0 && homeUnits.Count > 0)
                {
                    // Guest lost (Home won)
                    int damage = stageDamage + homeUnits.Count;
                    playerSystem.ApplyDamage(pair.GuestId == 0 ? Team.Player : Team.Opponent, damage);
                    if (pair.HomeId == 0) GameSystemManager.Instance.Economy?.RegisterResult(true);
                    Debug.Log($"[Gameplay] Player {pair.GuestId} lost combat as guest. Taking {damage} damage.");
                }
            }
        }
    }

    public enum GameplayPhase
    {
        Planning,
        Combat
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

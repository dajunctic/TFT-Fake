
using System;
using UnityEngine;

namespace Dajunctic
{
    public class Gameplay : BaseView
    {
        public static string HeroLayerName = "CombatActor";
        public static Gameplay Instance { get; private set; }

        [Header("Phase Settings")]
        [SerializeField] private float planningDuration = 10f;
        [SerializeField] private float combatDuration = 30f;

        private RoundSystem _roundSystem;
        private RoundSystem RoundSystem => _roundSystem ?? (_roundSystem = GameSystemManager.Instance.Round);
        
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
            
            if (RoundSystem != null && RoundSystem.CurrentRoundData != null)
            {
                duration = (phase == GameplayPhase.Planning) 
                    ? RoundSystem.CurrentRoundData.planningDuration 
                    : RoundSystem.CurrentRoundData.combatDuration;
            }

            _phaseDuration = duration;
            _timer = _phaseDuration;

            Debug.Log($"[Gameplay] Starting Phase: {phase} for {_phaseDuration}s in round {(RoundSystem != null ? RoundSystem.GetRoundDisplayString() : "N/A")}");
            
            if (phase == GameplayPhase.Planning)
            {
                this.Raise(new ShowPopupEvent { PopupType = typeof(GameplayPopup), ShowMode = PopupShowMode.DoNothing });
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
                if (RoundSystem != null)
                {
                    RoundSystem.AdvanceRound();
                }
                
                StartPhase(GameplayPhase.Planning);
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


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

            _phaseDuration = duration;
            _timer = _phaseDuration;

            Debug.Log($"[Gameplay] Starting Phase: {phase} for {_phaseDuration}s in round {(RoundSys != null ? RoundSys.GetRoundDisplayString() : "N/A")}");

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
            if (playerSystem == null) return;

            // Simplified combat result: for now, assume player always loses if we want to test damage, 
            // or count surviving units if we have a way to identify teams.
            // In a real game, you'd check which team has units left on the FieldSystem.

            var fieldSystem = GameSystemManager.Instance.Field;
            if (fieldSystem == null) return;

            var allUnits = fieldSystem.GetAllHeroes();
            int playerUnits = allUnits.Count(u => u.CombatTeam == Team.Player);
            int opponentUnits = allUnits.Count(u => u.CombatTeam == Team.Opponent);

            int stageDamage = (RoundSys != null) ? RoundSys.StageNumber + 1 : 2;

            if (playerUnits == 0 && opponentUnits > 0)
            {
                // Player lost
                int damage = stageDamage + opponentUnits;
                playerSystem.ApplyDamage(Team.Player, damage);
                GameSystemManager.Instance.Economy?.RegisterResult(false);
                Debug.Log($"[Gameplay] Player lost combat. Taking {damage} damage.");
            }
            else if (opponentUnits == 0 && playerUnits > 0)
            {
                // Opponent lost
                int damage = stageDamage + playerUnits;
                playerSystem.ApplyDamage(Team.Opponent, damage);
                GameSystemManager.Instance.Economy?.RegisterResult(true);
                Debug.Log($"[Gameplay] Opponent lost combat. Taking {damage} damage.");
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

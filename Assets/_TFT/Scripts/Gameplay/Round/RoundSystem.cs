using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Dajunctic
{
    public class RoundSystem : MonoBehaviour, IGameSystem
    {
        private RoundSystemData _data;
        private int _currentStageIndex = 0;
        private int _currentRoundIndex = 0;
        private GameSystemManager _manager;

        // Runtime overrides for Encounters/Kì ngộ
        private Dictionary<(int stage, int round), RoundData> _overrides = new Dictionary<(int stage, int round), RoundData>();

        public RoundData CurrentRoundData { get; private set; }
        public StageData CurrentStageData { get; private set; }

        public int StageNumber => CurrentStageData != null ? CurrentStageData.stageNumber : 1;
        public int RoundNumber => _currentRoundIndex + 1;

        public async Task LoadDataAsync()
        {
            var handle = Addressables.LoadAssetAsync<RoundSystemData>(_manager.Config.roundSystemData);
            _data = await handle.Task;
            Debug.Log("<color=cyan>RoundSystem data loaded</color>");
        }

        public void Initialize(GameSystemManager manager)
        {
            _manager = manager;
            ResetToStart();
            Debug.Log("<color=cyan>RoundSystem initialized</color>");
        }

        public void ResetToStart()
        {
            _currentStageIndex = 0;
            _currentRoundIndex = 0;
            _overrides.Clear();
            UpdateCurrentData();
        }

        /// <summary>Override a specific round's data at runtime (e.g. for Encounters/Kì ngộ).</summary>
        public void OverrideRoundData(int stageNumber, int roundNumber, RoundData newData)
        {
            _overrides[(stageNumber, roundNumber)] = newData;
            
            // If we just overrode the CURRENT round, update it immediately
            if (StageNumber == stageNumber && RoundNumber == roundNumber)
            {
                UpdateCurrentData();
            }
            
            this.Raise(new RoundScheduleChangedEvent());
            Debug.Log($"<color=cyan>RoundSystem: Round {stageNumber}-{roundNumber} overriden to {newData.displayName}</color>");
        }

        public void AdvanceRound()
        {
            if (_data == null || _data.stages.Count == 0) return;

            _currentRoundIndex++;
            if (_currentRoundIndex >= CurrentStageData.rounds.Count)
            {
                // Next Stage
                _currentRoundIndex = 0;
                _currentStageIndex++;

                if (_currentStageIndex >= _data.stages.Count)
                {
                    if (_data.loopLastStage)
                    {
                        _currentStageIndex = _data.stages.Count - 1;
                    }
                    else
                    {
                        Debug.LogWarning("RoundSystem: Reached end of all stages!");
                        return;
                    }
                }
            }

            UpdateCurrentData();
            
            this.Raise(new RoundAdvancedEvent 
            { 
                StageNumber = StageNumber, 
                RoundNumber = RoundNumber, 
                RoundData = CurrentRoundData 
            });
            
            Debug.Log($"<color=cyan>Round advanced to: {GetRoundDisplayString()}</color>");
        }

        private void UpdateCurrentData()
        {
            if (_data == null || _data.stages.Count == 0) return;

            CurrentStageData = _data.stages[_currentStageIndex];
            
            if (CurrentStageData.rounds.Count > 0)
            {
                // Check if there is a runtime override
                if (_overrides.TryGetValue((StageNumber, RoundNumber), out var overridenData))
                {
                    CurrentRoundData = overridenData;
                }
                else
                {
                    CurrentRoundData = CurrentStageData.rounds[_currentRoundIndex];
                }
            }
        }

        /// <summary>Returns the current round data for a specific index, considering overrides.</summary>
        public RoundData GetRoundData(int stageIndex, int roundIndex)
        {
            if (_data == null || stageIndex >= _data.stages.Count) return null;
            var stage = _data.stages[stageIndex];
            if (roundIndex >= stage.rounds.Count) return null;

            int stageNum = stage.stageNumber;
            int roundNum = roundIndex + 1;

            if (_overrides.TryGetValue((stageNum, roundNum), out var overridenData))
            {
                return overridenData;
            }

            return stage.rounds[roundIndex];
        }

        public string GetRoundDisplayString()
        {
            return $"{StageNumber}-{RoundNumber}";
        }

        public void Shutdown()
        {
            Debug.Log("<color=yellow>RoundSystem shutdown</color>");
        }
    }

    public struct RoundAdvancedEvent : IEvent
    {
        public int StageNumber;
        public int RoundNumber;
        public RoundData RoundData;
    }

    public struct RoundScheduleChangedEvent : IEvent { }
}

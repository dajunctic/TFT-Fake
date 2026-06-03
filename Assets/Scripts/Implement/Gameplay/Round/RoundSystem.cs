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

        private Dictionary<(int stage, int round), RoundData> _overrides = new Dictionary<(int stage, int round), RoundData>();

        public RoundData CurrentRoundData { get; private set; }
        public StageData CurrentStageData { get; private set; }

        public int StageNumber => CurrentStageData != null ? CurrentStageData.stageNumber : 1;
        public int RoundNumber => _currentRoundIndex + 1;

        public async Task LoadDataAsync()
        {
            if (GameSystemManager.Instance.Config != null && GameSystemManager.Instance.Config.roundSystemData != null)
            {
                var handle = Addressables.LoadAssetAsync<RoundSystemData>(GameSystemManager.Instance.Config.roundSystemData);
                _data = await handle.Task;
            }
        }

        public void Initialize(GameSystemManager manager)
        {
            _manager = manager;
            ResetToStart();
        }

        public void ResetToStart()
        {
            _currentStageIndex = 0;
            _currentRoundIndex = 0;
            _overrides.Clear();
            UpdateCurrentData();
        }

        public void OverrideRoundData(int stageNumber, int roundNumber, RoundData newData)
        {
            _overrides[(stageNumber, roundNumber)] = newData;

            if (StageNumber == stageNumber && RoundNumber == roundNumber)
            {
                UpdateCurrentData();
            }
            
            this.Raise(new RoundScheduleChangedEvent());
        }

        public void AdvanceRound()
        {
            if (_data == null || _data.stages.Count == 0) return;

            _currentRoundIndex++;
            if (_currentRoundIndex >= CurrentStageData.rounds.Count)
            {
                
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
        }

        private void UpdateCurrentData()
        {
            if (_data == null || _data.stages.Count == 0) return;

            CurrentStageData = _data.stages[_currentStageIndex];
            
            if (CurrentStageData.rounds.Count > 0)
            {
                
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

        /// <summary>
        /// Called on client to sync round state from server.
        /// </summary>
        public void SetRoundState(int stageNumber, int roundNumber)
        {
            if (_data == null || _data.stages.Count == 0) return;

            int stageIdx = _data.stages.FindIndex(s => s.stageNumber == stageNumber);
            if (stageIdx < 0) return;

            _currentStageIndex = stageIdx;
            _currentRoundIndex = roundNumber - 1;
            UpdateCurrentData();

            this.Raise(new RoundAdvancedEvent
            {
                StageNumber = StageNumber,
                RoundNumber = RoundNumber,
                RoundData = CurrentRoundData
            });
        }

        public void Shutdown()
        {
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

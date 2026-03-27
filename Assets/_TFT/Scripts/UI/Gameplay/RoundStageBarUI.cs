using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic
{
    public class RoundStageBarUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RoundStageIconUI iconPrefab;
        [SerializeField] private Transform container;
        [SerializeField] private RectTransform rectTransform;

        public RectTransform RectTransform => rectTransform;

        private List<RoundStageIconUI> _icons = new List<RoundStageIconUI>();

        public void UpdateStage(StageData stageData, int currentRoundNumber)
        {
            if (stageData == null) return;

            for(var i = 0; i < container.childCount; i++)
            {
                Destroy(container.GetChild(i).gameObject);
            }

            // Adjust icon count
            while (_icons.Count < stageData.rounds.Count)
            {
                var icon = Instantiate(iconPrefab, container);
                _icons.Add(icon);
            }

            var roundSystem = GameSystemManager.Instance.Round;

            for (int i = 0; i < _icons.Count; i++)
            {
                if (i < stageData.rounds.Count)
                {
                    _icons[i].gameObject.SetActive(true);
                    bool isCurrent = (i + 1) == currentRoundNumber;
                    bool isPassed = (i + 1) < currentRoundNumber;
                    
                    RoundData data = (roundSystem != null) 
                        ? roundSystem.GetRoundData(stageData.stageNumber - 1, i) 
                        : stageData.rounds[i];
                        
                    _icons[i].Setup(data, isCurrent, isPassed);
                }
                else
                {
                    _icons[i].gameObject.SetActive(false);
                }
            }
        }
    }
}

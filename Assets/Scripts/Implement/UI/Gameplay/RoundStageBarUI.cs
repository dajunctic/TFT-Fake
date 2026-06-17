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

        private void Awake()
        {
            // Destroy editor placeholders under container on load
            if (container != null)
            {
                for (int i = container.childCount - 1; i >= 0; i--)
                {
                    Destroy(container.GetChild(i).gameObject);
                }
            }
        }

        public void UpdateStage(StageData stageData, int currentRoundNumber)
        {
            if (stageData == null) return;

            // Despawn extra icons if any
            while (_icons.Count > stageData.rounds.Count)
            {
                int lastIdx = _icons.Count - 1;
                var icon = _icons[lastIdx];
                _icons.RemoveAt(lastIdx);
                if (icon != null)
                {
                    icon.Despawn();
                }
            }

            // Ensure we have enough icons in the pool
            while (_icons.Count < stageData.rounds.Count)
            {
                var icon = PoolableObject.Pool.Spawn(iconPrefab, container.position, Quaternion.identity);
                icon.transform.SetParent(container, false);
                _icons.Add(icon);
            }

            var roundSystem = GameSystemManager.Instance.Round;

            for (int i = 0; i < _icons.Count; i++)
            {
                bool isCurrent = (i + 1) == currentRoundNumber;
                bool isPassed = (i + 1) < currentRoundNumber;
                
                RoundData data = (roundSystem != null) 
                    ? roundSystem.GetRoundData(stageData.stageNumber - 1, i) 
                    : stageData.rounds[i];
                    
                _icons[i].Setup(data, isCurrent, isPassed);
            }
        }
    }
}

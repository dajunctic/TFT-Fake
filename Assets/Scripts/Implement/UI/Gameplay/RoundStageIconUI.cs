using UnityEngine;
using UnityEngine.UI;

namespace Dajunctic
{
    public class RoundStageIconUI : PoolableObject
    {
        [Header("References")]
        [SerializeField] private Image iconImage;
        [SerializeField] private Image frameImage;
        [SerializeField] private GameObject currentIndicator;
        [SerializeField] private GameObject augmentIndicator;
        
        [Header("Settings")]
        [SerializeField] private Color passedColor = Color.gray;
        [SerializeField] private Color activeColor = Color.white;
        [SerializeField] private Color currentColor = Color.yellow;

        public void Setup(RoundData data, bool isCurrent, bool isPassed)
        {
            if (data == null) return;

            if (iconImage != null)
            {
                iconImage.sprite = data.icon;
                iconImage.color = isPassed ? passedColor : activeColor;
            }

            if (frameImage != null)
            {
                frameImage.color = isCurrent ? currentColor : (isPassed ? passedColor : activeColor);
            }

            if (currentIndicator != null)
            {
                currentIndicator.SetActive(isCurrent);
            }

            if (augmentIndicator != null)
            {
                augmentIndicator.SetActive(data.hasAugment);
            }
        }
    }
}

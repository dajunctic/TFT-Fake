using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dajunctic
{
    public class TraitItemUI : BaseView, IPointerEnterHandler, IPointerClickHandler
    {
        [Header("UI References")]
        [SerializeField] private Image icon;
        [SerializeField] private Image background;
        [SerializeField] private TMP_Text traitName;
        [SerializeField] private TMP_Text traitCount;

        [Header("Tier Sprites")]
        [SerializeField] private Sprite noneSprite;
        [SerializeField] private Sprite bronzeSprite;
        [SerializeField] private Sprite silverSprite;
        [SerializeField] private Sprite goldSprite;
        [SerializeField] private Sprite chromaticSprite;

        private TraitData _traitData;
        private int _count;

        public void Setup(ITrait trait, int count)
        {
            if (trait is TraitData data)
            {
                _traitData = data;
                _count = count;

                icon.sprite = data.Icon;
                traitName.text = data.DisplayName;
                traitCount.text = count.ToString();

                var activeTier = data.Tiers
                    .Where(t => count >= t.RequiredCount)
                    .OrderByDescending(t => t.RequiredCount)
                    .FirstOrDefault();

                UpdateVisualTier(activeTier?.VisualTier ?? TraitTierType.None);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // Do nothing on hover as per request "khi mình bấm vào thì mới hiện"
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_traitData == null) return;

            this.Raise(new TraitHoverEvent
            {
                Trait = _traitData,
                Count = _count,
                Trigger = GetComponent<RectTransform>()
            });
        }

        private void UpdateVisualTier(TraitTierType type)
        {
            switch (type)
            {
                case TraitTierType.Bronze:
                    background.sprite = bronzeSprite;
                    break;
                case TraitTierType.Silver:
                    background.sprite = silverSprite;
                    break;
                case TraitTierType.Gold:
                    background.sprite = goldSprite;
                    break;
                case TraitTierType.Chromatic:
                    background.sprite = chromaticSprite;
                    break;
                default:
                    background.sprite = noneSprite;
                    break;
            }
        }
    }
}

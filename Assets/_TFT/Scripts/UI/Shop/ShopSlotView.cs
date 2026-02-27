using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Dajunctic
{
    public class ShopSlotView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image heroIcon;
        [SerializeField] private TMP_Text heroName;
        [SerializeField] private TMP_Text costText;
        // [SerializeField] private GameObject rarityBorder; // Optional color based on rarity
        [SerializeField] private Image rarityBackground;

        private int _slotIndex;
        private ChampionData _heroData;

        public void Setup(int index, ChampionData data, Sprite bgSprite = null)
        {
            _slotIndex = index;
            _heroData = data;

            if (data == null)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);
            heroIcon.sprite = data.shopIcon;
            heroName.text = data.displayName;
            costText.text = data.rarity.ToString();

            if (rarityBackground != null && bgSprite != null)
            {
                rarityBackground.sprite = bgSprite;
            }
            
            // Set border color based on rarity (1-White, 2-Green, 3-Blue, 4-Purple, 5-Gold)
            // if (rarityBorder != null)
            // {
            //     Image borderImage = rarityBorder.GetComponent<Image>();
            //     if (borderImage != null)
            //     {
            //         borderImage.color = GetRarityColor(data.rarity);
            //     }
            // }
        }

        private Color GetRarityColor(int rarity)
        {
            switch (rarity)
            {
                case 1: return Color.gray;
                case 2: return Color.green;
                case 3: return Color.blue;
                case 4: return new Color(0.5f, 0, 0.5f); // Purple
                case 5: return new Color(1, 0.8f, 0); // Gold
                default: return Color.white;
            }
        }

        public void OnClickBuy()
        {
            this.Raise(new RequestBuyHeroEvent { SlotIndex = _slotIndex });
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace Dajunctic
{
    public class ShopPopup : BasePopup
    {
        [Header("Shop References")]
        [SerializeField] private List<ShopSlotView> slots;
        [SerializeField] private Button rerollButton;
        [SerializeField] private Button buyXpButton;

        [Header("Economy References")]
        [SerializeField] private TMP_Text goldText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text xpText;
        [SerializeField] private Image xpProgress;

        public override void ListenEvents()
        {
            base.ListenEvents();
            this.RegisterListener<ShopRefreshedEvent>(OnShopRefreshed);
            this.RegisterListener<GoldChangedEvent>(OnGoldChanged);
            this.RegisterListener<LevelChangedEvent>(OnLevelChanged);
        }

        public override void StopListenEvents()
        {
            base.StopListenEvents();
            this.RemoveListener<ShopRefreshedEvent>(OnShopRefreshed);
            this.RemoveListener<GoldChangedEvent>(OnGoldChanged);
            this.RemoveListener<LevelChangedEvent>(OnLevelChanged);
        }

        public override void BeforeShow(object data = null)
        {
            base.BeforeShow(data);
            UpdateShop();
            UpdateEconomy();
        }

        private void OnShopRefreshed(ShopRefreshedEvent evt) => UpdateShop();
        private void OnGoldChanged(GoldChangedEvent evt) => UpdateEconomy();
        private void OnLevelChanged(LevelChangedEvent evt) => UpdateEconomy();

        private void UpdateShop()
        {
            var shop = ShopController.Instance.CurrentShop;
            for (int i = 0; i < slots.Count; i++)
            {
                if (i < shop.Length)
                {
                    slots[i].Setup(i, shop[i]);
                }
            }
        }

        private void UpdateEconomy()
        {
            var eco = EconomyManager.Instance;
            goldText.text = eco.Gold.ToString();
            levelText.text = "Lvl. " + eco.Level;
            
            int currentXp = eco.XP;
            int requiredXp = eco.GetXPRequired();
            xpText.text = $"{currentXp}/{requiredXp}";
            
            if (xpProgress != null)
            {
                xpProgress.fillAmount = requiredXp > 0 ? (float)currentXp / requiredXp : 1;
            }
        }

        public void OnClickReroll()
        {
            ShopController.Instance.Reroll();
        }

        public void OnClickBuyXP()
        {
            if (EconomyManager.Instance.SpendGold(4)) // Hardcoded 4 gold for 4 XP
            {
                EconomyManager.Instance.AddXP(4);
            }
        }
    }
}

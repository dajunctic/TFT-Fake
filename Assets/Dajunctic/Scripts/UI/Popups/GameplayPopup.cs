using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Dajunctic
{
    public class GameplayPopup : BasePopup
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private Image progressImage;
        [SerializeField] private TMP_Text phaseText;

        [Header("Shop References")]
        [SerializeField] private SpriteLists cardRarities;
        [SerializeField] private List<ShopSlotView> slots;

        [Header("Economy References")]
        [SerializeField] private TMP_Text goldText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text xpText;
        [SerializeField] private Image xpProgress;
        [SerializeField] private GameObject buyXPButton;
        [SerializeField] private GameObject buyXPDisabledButton;

        public override void ListenEvents()
        {
            base.ListenEvents();
            this.RegisterListener<ShopRefreshedEvent>(OnShopRefreshed);
            this.RegisterListener<GoldChangedEvent>(OnGoldChanged);
            this.RegisterListener<LevelChangedEvent>(OnLevelChanged);
            this.RegisterListener<XPChangedEvent>(OnXPChanged);
        }

        public override void StopListenEvents()
        {
            base.StopListenEvents();
            this.RemoveListener<ShopRefreshedEvent>(OnShopRefreshed);
            this.RemoveListener<GoldChangedEvent>(OnGoldChanged);
            this.RemoveListener<LevelChangedEvent>(OnLevelChanged);
            this.RemoveListener<XPChangedEvent>(OnXPChanged);
        }

        public override void BeforeShow(object data = null)
        {
            base.BeforeShow(data);
            UpdateUI();
            UpdateShop();
            UpdateEconomy();
        }

        private void Update()
        {
            if (Gameplay.Instance == null) return;

            UpdateUI();
        }

        private void UpdateUI()
        {
            var gameplay = Gameplay.Instance;
            float timer = Mathf.Max(0, gameplay.Timer);
            
            // Format time
            timerText.text = Mathf.CeilToInt(timer).ToString();
            
            // Fill amount
            float fill = timer / gameplay.PhaseDuration;
            progressImage.fillAmount = fill;

            // Phase name Vietnamese
            phaseText.text = gameplay.CurrentPhase == GameplayPhase.Planning ? "CHUẨN BỊ" : "CHIẾN ĐẤU";
            
            // Color change for combat phase
            phaseText.color = gameplay.CurrentPhase == GameplayPhase.Planning ? Color.white : Color.red;
            progressImage.color = gameplay.CurrentPhase == GameplayPhase.Planning ? Color.cyan : Color.red;
        }

        private void OnShopRefreshed(ShopRefreshedEvent evt) => UpdateShop();
        private void OnGoldChanged(GoldChangedEvent evt) => UpdateEconomy();
        private void OnLevelChanged(LevelChangedEvent evt) => UpdateEconomy();
        private void OnXPChanged(XPChangedEvent evt) => UpdateEconomy();

        private void UpdateShop()
        {
            if (ShopController.Instance == null) return;
            var shop = ShopController.Instance.CurrentShop;
            for (int i = 0; i < slots.Count; i++)
            {
                if (i < shop.Length && shop[i] != null)
                {
                    Sprite rarityBg = null;
                    if (cardRarities != null)
                    {
                        // rarity is 1-5, index is 0-4
                        rarityBg = cardRarities.GetIndex(shop[i].rarity - 1);
                    }
                    slots[i].Setup(i, shop[i], rarityBg);
                }
                else
                {
                    slots[i].Setup(i, null);
                }
            }
        }

        private void UpdateEconomy()
        {
            if (EconomyManager.Instance == null) return;
            var eco = EconomyManager.Instance;
            goldText.text = eco.Gold.ToString();
            levelText.text = "Lvl. " + eco.Level;
            
            if (eco.IsMaxLevel)
            {
                xpText.text = "MAX";
                if (xpProgress != null) xpProgress.fillAmount = 1;
                
                if (buyXPButton != null) buyXPButton.SetActive(false);
                if (buyXPDisabledButton != null) buyXPDisabledButton.SetActive(true);
            }
            else
            {
                int currentXp = eco.XP;
                int requiredXp = eco.GetXPRequired();
                xpText.text = $"{currentXp}/{requiredXp}";

                if (xpProgress != null)
                {
                    xpProgress.fillAmount = requiredXp > 0 ? (float)currentXp / requiredXp : 1;
                }

                if (buyXPButton != null) buyXPButton.SetActive(true);
                if (buyXPDisabledButton != null) buyXPDisabledButton.SetActive(false);
            }
        }

        public void OnClickReroll()
        {
            ShopController.Instance.Reroll();
        }

        public void OnClickBuyXP()
        {
            if (EconomyManager.Instance.IsMaxLevel) return;

            var shopData = ShopController.Instance.ShopData;
            if (EconomyManager.Instance.SpendGold(shopData.buyXpCost))
            {
                EconomyManager.Instance.AddXP(shopData.xpPerBuy);
            }
        }

    }
}

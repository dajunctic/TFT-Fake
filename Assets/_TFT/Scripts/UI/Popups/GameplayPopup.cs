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
        [SerializeField] private GameObject cardListContent; // Container for the 5 slots
        [SerializeField] private List<TMP_Text> rollRatesTexts; // Optional: To display roll rates for each rarity

        [Header("Economy References")]
        [SerializeField] private TMP_Text goldText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text xpText;
        [SerializeField] private Image xpProgress;
        [SerializeField] private GameObject buyXPButton;
        [SerializeField] private GameObject buyXPDisabledButton;
        [SerializeField] private GameObject unlockShop;
        [SerializeField] private GameObject lockShop;

        [Header("Item Bench")]
        [SerializeField] private Transform[] itemBenchPositions;
        public Transform[] ItemBenchPositions => itemBenchPositions;

        [Header("Trait List")]
        [SerializeField] private TraitListUI traitListUI;

        [Header("FPS & Ping")]
        [SerializeField] private TMP_Text fpsTxt;
        [SerializeField] private TMP_Text pingTxt;

        [Header("Player Lists")]
        [SerializeField] private List<PlayerUI> playerUIs;

        public static GameplayPopup Instance { get; private set; }

        // Cached Systems
        private ShopSystem _shopSystem;
        private EconomySystem _economySystem;

        private ShopSystem ShopSystem => _shopSystem ?? (_shopSystem = this.GetSystem<ShopSystem>());
        private EconomySystem EconomySystem => _economySystem ?? (_economySystem = this.GetSystem<EconomySystem>());

        public override void ListenEvents()
        {
            base.ListenEvents();
            this.RegisterListener<ShopRefreshedEvent>(OnShopRefreshed);
            this.RegisterListener<GoldChangedEvent>(OnGoldChanged);
            this.RegisterListener<LevelChangedEvent>(OnLevelChanged);
            this.RegisterListener<XPChangedEvent>(OnXPChanged);
            this.RegisterListener<HeroDragStartedEvent>(OnHeroDragStarted);
            this.RegisterListener<HeroDragEndedEvent>(OnHeroDragEnded);
            this.RegisterListener<ShopLockChangedEvent>(OnShopLockChanged);
        }

        public override void AfterShow()
        {
            base.AfterShow();

            var itemSystem = this.GetSystem<ItemSystem>();
            if (itemSystem != null) itemSystem.RefreshAllVisuals();
        }

        public override void BeforeDismiss()
        {
            base.BeforeDismiss();

            var itemSystem = this.GetSystem<ItemSystem>();
            if (itemSystem != null) itemSystem.ClearAllVisuals();

            if (Instance == this) Instance = null;
        }

        public override void StopListenEvents()
        {
            base.StopListenEvents();
            this.RemoveListener<ShopRefreshedEvent>(OnShopRefreshed);
            this.RemoveListener<GoldChangedEvent>(OnGoldChanged);
            this.RemoveListener<LevelChangedEvent>(OnLevelChanged);
            this.RemoveListener<XPChangedEvent>(OnXPChanged);
            this.RemoveListener<HeroDragStartedEvent>(OnHeroDragStarted);
            this.RemoveListener<HeroDragEndedEvent>(OnHeroDragEnded);
            this.RemoveListener<ShopLockChangedEvent>(OnShopLockChanged);
        }

        public override void BeforeShow(object data = null)
        {
            Instance = this;
            base.BeforeShow(data);
            UpdateUI();
            UpdateShop();
            UpdateEconomy();
            UpdatePlayerList();
            UpdateShopLockUI(ShopSystem != null && ShopSystem.IsShopLocked);
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
            if (ShopSystem == null) return;
            var shop = ShopSystem.CurrentShop;
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

            UpdateRollRates(ShopSystem.ShopData.GetChancesForLevel(EconomySystem.Level));
        }

        private void UpdateRollRates(float[] chances)
        {
            for (int i = 0; i < rollRatesTexts.Count && i < chances.Length; i++)
            {
                rollRatesTexts[i].text = $"{chances[i] * 100f}%";
            }
        }

        private void UpdateEconomy()
        {
            if (EconomySystem == null) return;
            var eco = EconomySystem;
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
            if (ShopSystem != null)
                UpdateRollRates(ShopSystem.ShopData.GetChancesForLevel(eco.Level));

        }

        private void OnHeroDragStarted(HeroDragStartedEvent evt)
        {
            if (cardListContent != null) cardListContent.SetActive(false);
        }

        private void OnHeroDragEnded(HeroDragEndedEvent evt)
        {
            if (cardListContent != null) cardListContent.SetActive(true);
        }

        public void OnClickReroll()
        {
            this.Raise(new RequestRerollEvent());
        }

        public void OnClickBuyXP()
        {
            this.Raise(new RequestBuyXPEvent());
        }

        public void OnClickOpenSetting()
        {
            this.Raise(new ShowPopupEvent
            {
                PopupType = typeof(SettingsPopup),
                ShowMode = PopupShowMode.DoNothing,
                Data = null
            });
        }

        /// <summary>Drag this to the LockShop button's OnClick in Inspector.</summary>
        public void OnClickLockShop()
        {
            this.Raise(new RequestToggleShopLockEvent());
        }

        /// <summary>Drag this to the UnlockShop button's OnClick in Inspector.</summary>
        public void OnClickUnlockShop()
        {
            this.Raise(new RequestToggleShopLockEvent());
        }

        private void OnShopLockChanged(ShopLockChangedEvent evt)
        {
            UpdateShopLockUI(evt.IsLocked);
        }

        private void UpdateShopLockUI(bool isLocked)
        {
            // Locked: show unlock button, hide lock button
            // Unlocked: show lock button, hide unlock button
            if (lockShop != null) lockShop.SetActive(!isLocked);
            if (unlockShop != null) unlockShop.SetActive(isLocked);
        }


        public override void Tick()
        {
            base.Tick();

            fpsTxt.text = $"FPS: {Mathf.RoundToInt(1f / Time.unscaledDeltaTime)}";
        }

        public void UpdatePing(float ping)
        {
            pingTxt.text = $"Ping: {Mathf.RoundToInt(ping)} ms";
        }

        #region Player List
        private void UpdatePlayerList()
        {
            foreach (var ui in playerUIs)
            {
                ui.TogglePlayer(false);
            }
            playerUIs[0].TogglePlayer(true);
        }


        public void OnclickPlayer(int index)
        {
            if (index < 0 || index >= playerUIs.Count) return;

            var playerUI = playerUIs[index];

            foreach (var ui in playerUIs)
            {
                ui.TogglePlayer(false);
            }
            
            playerUI.TogglePlayer(true);
        }

        #endregion
    }
}

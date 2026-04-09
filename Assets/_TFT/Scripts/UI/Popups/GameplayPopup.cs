using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

namespace Dajunctic
{
    public class GameplayPopup : BasePopup
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private Image progressImage;

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
        [SerializeField] private PlayerListUI playerListUI;

        [SerializeField] private GameObject fightInfoBtn;
        [SerializeField] private GameObject playerInfoBtn;
        [SerializeField] private GameObject playerInfoDisabledBtn;
        [SerializeField] private GameObject fightInfoDisabledBtn;

        public static GameplayPopup Instance { get; private set; }

        // Cached Systems
        private ShopSystem _shopSystem;

        private ShopSystem ShopSystem => _shopSystem ?? (_shopSystem = this.GetSystem<ShopSystem>());

        public override void ListenEvents()
        {
            base.ListenEvents();
            this.RegisterListener<ShopRefreshedEvent>(OnShopRefreshed);
            this.RegisterListener<HeroDragStartedEvent>(OnHeroDragStarted);
            this.RegisterListener<HeroDragEndedEvent>(OnHeroDragEnded);
            this.RegisterListener<ShopLockChangedEvent>(OnShopLockChanged);
            this.RegisterListener<GameplayPhaseChangedEvent>(OnPhaseChanged);
            if (playerListUI != null) playerListUI.OnPlayerClicked += OnPlayerClicked;
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
            this.RemoveListener<HeroDragStartedEvent>(OnHeroDragStarted);
            this.RemoveListener<HeroDragEndedEvent>(OnHeroDragEnded);
            this.RemoveListener<ShopLockChangedEvent>(OnShopLockChanged);
            this.RemoveListener<GameplayPhaseChangedEvent>(OnPhaseChanged);
            if (playerListUI != null) playerListUI.OnPlayerClicked -= OnPlayerClicked;
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
            progressImage.color = gameplay.CurrentPhase == GameplayPhase.Planning ? Color.cyan : Color.red;
        }

        private void OnShopRefreshed(ShopRefreshedEvent evt) => UpdateShop();
        private void OnShopLockChanged(ShopLockChangedEvent evt) => UpdateShopLockUI(evt.IsLocked);

        private void OnPhaseChanged(GameplayPhaseChangedEvent evt)
        {
            if (evt.Phase == GameplayPhase.Planning)
            {
                if (playerListUI != null) playerListUI.SortAndAnimate();
            }
        }



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

            // UpdateRollRates(ShopSystem.ShopData.GetChancesForLevel(EconomySystem.Level));
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
            // if (EconomySystem == null) return;
            // var eco = EconomySystem;
            // goldText.text = eco.Gold.ToString();
            // levelText.text = "Lvl. " + eco.Level;

            // if (eco.IsMaxLevel)
            // {
            //     xpText.text = "MAX";
            //     if (xpProgress != null) xpProgress.fillAmount = 1;

            //     if (buyXPButton != null) buyXPButton.SetActive(false);
            //     if (buyXPDisabledButton != null) buyXPDisabledButton.SetActive(true);
            // }
            // else
            // {
            //     int currentXp = eco.XP;
            //     int requiredXp = eco.GetXPRequired();
            //     xpText.text = $"{currentXp}/{requiredXp}";

            //     if (xpProgress != null)
            //     {
            //         xpProgress.fillAmount = requiredXp > 0 ? (float)currentXp / requiredXp : 1;
            //     }

            //     if (buyXPButton != null) buyXPButton.SetActive(true);
            //     if (buyXPDisabledButton != null) buyXPDisabledButton.SetActive(false);
            // }
            // if (ShopSystem != null)
            //     UpdateRollRates(ShopSystem.ShopData.GetChancesForLevel(eco.Level));

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

        private void UpdateShopLockUI(bool isLocked)
        {
            // lockShop = icon khóa đóng (shop đang locked) → hiện khi locked
            // unlockShop = icon khóa mở (shop đang unlocked) → hiện khi unlocked (mặc định)
            if (lockShop != null) lockShop.SetActive(isLocked);
            if (unlockShop != null) unlockShop.SetActive(!isLocked);
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

        public void ToggleFightInfo(bool show)
        {
            if (fightInfoBtn != null) fightInfoBtn.SetActive(show);
            if (fightInfoDisabledBtn != null) fightInfoDisabledBtn.SetActive(!show);

            if (playerInfoBtn != null) playerInfoBtn.SetActive(!show);
            if (playerInfoDisabledBtn != null) playerInfoDisabledBtn.SetActive(show);
        }

        public void TogglePlayerInfo(bool show)
        {
            if (playerInfoBtn != null) playerInfoBtn.SetActive(show);
            if (playerInfoDisabledBtn != null) playerInfoDisabledBtn.SetActive(!show);

            if (fightInfoBtn != null) fightInfoBtn.SetActive(!show);
            if (fightInfoDisabledBtn != null) fightInfoDisabledBtn.SetActive(show);
        }

        private void UpdatePlayerList()
        {
            var playerSystem = this.GetSystem<PlayerSystem>();
            if (playerSystem == null || playerListUI == null) return;

            playerListUI.Initialize(playerSystem.Players);
        }

        private void OnPlayerClicked(PlayerData data)
        {
            var cam = Camera.main.GetComponent<FollowCamera>();
            if (cam != null && data.Tactician != null)
            {
                cam.target = data.Tactician.transform;
                cam.SnapToTarget();

                var playerSystem = this.GetSystem<PlayerSystem>();
                if (playerSystem != null && playerSystem.LocalPlayer != null)
                {
                    var localTactician = playerSystem.LocalPlayer.Tactician;
                    if (localTactician != null)
                    {
                        var fieldSystem = this.GetSystem<FieldSystem>();
                        if (fieldSystem != null)
                        {
                            var targetArena = fieldSystem.GetArena(data.Id);
                            if (targetArena != null)
                            {
                                Transform spawnTransform = (data.Id == playerSystem.LocalPlayer.Id) 
                                    ? targetArena.TacticianSpawnPoint 
                                    : targetArena.GuestSpawnPoint;

                                Vector3 spawnPos = spawnTransform != null ? spawnTransform.position : targetArena.transform.position;
                                localTactician.Teleport(spawnPos, true, true);
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}

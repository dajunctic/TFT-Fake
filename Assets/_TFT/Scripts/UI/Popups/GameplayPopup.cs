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
        [SerializeField] private TMP_Text roundText;   

        [Header("Shop References")]
        [SerializeField] private SpriteLists cardRarities;
        [SerializeField] private List<ShopSlotView> slots;
        [SerializeField] private GameObject cardListContent; 
        [SerializeField] private List<TMP_Text> rollRatesTexts; 

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

        [Header("Streak UI")]
        [SerializeField] private ShopStreakUI streakUI;

        [SerializeField] private GameObject fightInfoBtn;
        [SerializeField] private GameObject playerInfoBtn;
        [SerializeField] private GameObject playerInfoDisabledBtn;
        [SerializeField] private GameObject fightInfoDisabledBtn;

        public static GameplayPopup Instance { get; private set; }

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
            this.RegisterListener<RoundAdvancedEvent>(OnRoundAdvanced);
            if (playerListUI != null) playerListUI.OnPlayerClicked += OnPlayerClicked;
            PlayerSystem.OnPlayerListInitialized += UpdatePlayerList;
        }

        public override void AfterShow()
        {
            base.AfterShow();

            var itemSystem = this.GetSystem<ItemSystem>();
            if (itemSystem != null) itemSystem.RefreshAllVisuals();

            UpdateShop();
            UpdateStreak();
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
            this.RemoveListener<RoundAdvancedEvent>(OnRoundAdvanced);
            if (playerListUI != null) playerListUI.OnPlayerClicked -= OnPlayerClicked;
            PlayerSystem.OnPlayerListInitialized -= UpdatePlayerList;

            if (_localPlayerSync != null)
            {
                _localPlayerSync.OnGoldChanged -= OnGoldChanged;
                _localPlayerSync.OnLevelChanged -= OnLevelChanged;
                _localPlayerSync.OnExpChanged -= OnExpChanged;
                _localPlayerSync.OnWinStreakChanged -= OnStreakChanged;
                _localPlayerSync.OnLoseStreakChanged -= OnStreakChanged;
            }
        }

        public override void BeforeShow(object data = null)
        {
            Instance = this;
            base.BeforeShow(data);
            UpdateUI();
            UpdateRoundText(); 

            ResetShopSlots();
            UpdateEconomy();
            UpdatePlayerList();
            UpdateShopLockUI(ShopSystem != null && ShopSystem.IsShopLocked);
        }

        private PlayerDataSync _localPlayerSync;

        private PlayerDataSync FindLocalPlayerSync()
        {
            if (!FishNet.InstanceFinder.IsClientStarted) return null;
            var allSyncs = FindObjectsByType<PlayerDataSync>(FindObjectsSortMode.None);
            foreach (var sync in allSyncs)
            {
                var nob = sync.GetComponent<FishNet.Object.NetworkObject>();
                if (nob != null && nob.IsOwner) return sync;
            }
            return null;
        }

        private void Update()
        {
            if (Gameplay.Instance == null) return;

            UpdateUI();

            if (_localPlayerSync == null && FishNet.InstanceFinder.IsClientStarted)
            {
                _localPlayerSync = FindLocalPlayerSync();
                if (_localPlayerSync != null)
                {
                    _localPlayerSync.OnGoldChanged += OnGoldChanged;
                    _localPlayerSync.OnLevelChanged += OnLevelChanged;
                    _localPlayerSync.OnExpChanged += OnExpChanged;
                    _localPlayerSync.OnWinStreakChanged += OnStreakChanged;
                    _localPlayerSync.OnLoseStreakChanged += OnStreakChanged;
                    UpdateEconomy(); 
                    UpdateStreak();
                }
            }
        }

        private void UpdateUI()
        {
            var gameplay = Gameplay.Instance;
            float timer = Mathf.Max(0, gameplay.Timer);

            timerText.text = Mathf.CeilToInt(timer).ToString();

            float fill = timer / gameplay.PhaseDuration;
            progressImage.fillAmount = fill;

            progressImage.color = gameplay.CurrentPhase == GameplayPhase.Planning ? Color.cyan : Color.red;
        }

        private void OnShopRefreshed(ShopRefreshedEvent evt) => UpdateShop();
        private void OnShopLockChanged(ShopLockChangedEvent evt) => UpdateShopLockUI(evt.IsLocked);

        private void OnRoundAdvanced(RoundAdvancedEvent evt)
        {
            UpdateRoundText();
        }

        private void OnPhaseChanged(GameplayPhaseChangedEvent evt)
        {
            if (evt.Phase == GameplayPhase.Planning)
            {
                if (playerListUI != null) playerListUI.SortAndAnimate();
            }
        }

        private void UpdateRoundText()
        {
            if (roundText == null) return;
            var roundSystem = GameSystemManager.Instance?.Round;
            if (roundSystem != null)
            {
                roundText.text = roundSystem.GetRoundDisplayString();
            }
        }

        private void UpdateShop()
        {
            if (ShopSystem == null)
            {
                Debug.LogWarning("[GameplayPopup] UpdateShop: ShopSystem not found.");
                return;
            }

            if (slots == null || slots.Count == 0)
            {
                Debug.LogError("[GameplayPopup] UpdateShop: 'slots' list is empty! " +
                               "Assign 5 ShopSlotView references in the GameplayPopup prefab Inspector.");
                return;
            }

            var shop = ShopSystem.CurrentShop;
            
            int nonNull = 0;
            for (int k = 0; k < shop.Length; k++) if (shop[k] != null) nonNull++;
            Debug.Log($"[GameplayPopup] UpdateShop called. NonNull slots={nonNull}/5");

            int shown = 0;
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i] == null)
                {
                    Debug.LogWarning($"[GameplayPopup] slots[{i}] is null — check Inspector assignment.");
                    continue;
                }

                if (i < shop.Length && shop[i] != null)
                {
                    Sprite rarityBg = null;
                    if (cardRarities != null)
                    {
                        
                        rarityBg = cardRarities.GetIndex(shop[i].rarity - 1);
                    }
                    slots[i].Setup(i, shop[i], rarityBg);
                    slots[i].gameObject.SetActive(true);
                    shown++;
                }
                else
                {
                    slots[i].Setup(i, null);
                }
            }

            Debug.Log($"<color=cyan>[GameplayPopup] UpdateShop: {shown}/{slots.Count} slots filled.</color>");
            
        }

        private void ResetShopSlots()
        {
            foreach (var slot in slots)
            {
                if (slot != null) slot.gameObject.SetActive(false);
            }
        }

        private void UpdateRollRates(float[] chances)
        {
            for (int i = 0; i < rollRatesTexts.Count && i < chances.Length; i++)
            {
                rollRatesTexts[i].text = $"{chances[i] * 100f}%";
            }
        }

        private void OnGoldChanged(int value) => goldText.text = value.ToString();
        private void OnLevelChanged(int value) 
        {
            levelText.text = "Lvl. " + value;
            if (ShopSystem != null) UpdateRollRates(ShopSystem.ShopData.GetChancesForLevel(value));
        }
        private void OnExpChanged(int value) => UpdateEconomy(); 
        private void OnStreakChanged(int value) => UpdateStreak();

        private void UpdateStreak()
        {
            if (streakUI == null || _localPlayerSync == null) return;
            
            int netStreak = _localPlayerSync.WinStreak.Value - _localPlayerSync.LoseStreak.Value;
            streakUI.UpdateStreak(netStreak);
        }

        private void UpdateEconomy()
        {
            if (_localPlayerSync == null) return;
            
            goldText.text = _localPlayerSync.Gold.Value.ToString();
            levelText.text = "Lvl. " + _localPlayerSync.Level.Value.ToString();

            int currentXp = _localPlayerSync.Exp.Value;
            int requiredXp = _localPlayerSync.GetXPRequired();
            
            if (requiredXp == 0) 
            {
                xpText.text = "MAX";
                if (xpProgress != null) xpProgress.fillAmount = 1;

                if (buyXPButton != null) buyXPButton.SetActive(false);
                if (buyXPDisabledButton != null) buyXPDisabledButton.SetActive(true);
            }
            else
            {
                xpText.text = $"{currentXp}/{requiredXp}";

                if (xpProgress != null)
                {
                    xpProgress.fillAmount = (float)currentXp / requiredXp;
                }

                if (buyXPButton != null) buyXPButton.SetActive(true);
                if (buyXPDisabledButton != null) buyXPDisabledButton.SetActive(false);
            }

            if (ShopSystem != null)
                UpdateRollRates(ShopSystem.ShopData.GetChancesForLevel(_localPlayerSync.Level.Value));
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

        public void OnClickLockShop()
        {
            this.Raise(new RequestToggleShopLockEvent());
        }

        public void OnClickUnlockShop()
        {
            this.Raise(new RequestToggleShopLockEvent());
        }

        private void UpdateShopLockUI(bool isLocked)
        {

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
                                localTactician.Teleport(spawnPos, false, true); 
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}

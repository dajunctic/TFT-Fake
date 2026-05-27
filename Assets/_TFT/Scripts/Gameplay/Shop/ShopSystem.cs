using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Linq;

namespace Dajunctic
{
    public class ShopSystem : MonoBehaviour, IGameSystem
    {
        private ShopSystemData _data;

        private ChampionData[] _currentShop = new ChampionData[5];
        public ChampionData[] CurrentShop => _currentShop;

        private bool _isShopLocked;
        public bool IsShopLocked => _isShopLocked;
        public ShopSystemData ShopSystemData => _data;
        public ShopData ShopData => _data?.shopData;
        public List<ChampionData> AllHeroes => _data?.allHeroes;
        private GameSystemManager _manager;

        public static event System.Action OnShopRefreshed;

        public async Task LoadDataAsync()
        {
            var handle = Addressables.LoadAssetAsync<ShopSystemData>(GameSystemManager.Instance.Config.shopSystemData);
            _data = await handle.Task;
            Debug.Log("<color=cyan>ShopSystem data loaded</color>");
        }

        public void Initialize(GameSystemManager manager)
        {
            _manager = manager;

            this.RegisterListener<RequestRerollEvent>(OnRequestReroll);
            this.RegisterListener<RequestBuyXPEvent>(OnRequestBuyXP);
            this.RegisterListener<RequestBuyHeroEvent>(OnRequestBuyHero);
            this.RegisterListener<GameplayPhaseChangedEvent>(OnPhaseChanged);
            this.RegisterListener<RequestToggleShopLockEvent>(OnRequestToggleShopLock);

            Debug.Log("<color=cyan>ShopSystem initialized</color>");
        }

        private PlayerDataSync GetLocalPlayerSync()
        {
            if (!FishNet.InstanceFinder.IsClientStarted) return null;

            int localClientId = FishNet.InstanceFinder.ClientManager?.Connection?.ClientId ?? -1;
            if (localClientId < 0) return null;

            var allSyncs = UnityEngine.Object.FindObjectsByType<PlayerDataSync>(UnityEngine.FindObjectsSortMode.None);

            foreach (var sync in allSyncs)
            {
                var nob = sync.GetComponent<FishNet.Object.NetworkObject>();
                if (nob != null && nob.IsOwner)
                    return sync;
            }

            foreach (var sync in allSyncs)
            {
                if ((int)sync.ClientId.Value == localClientId)
                    return sync;
            }

            Debug.LogWarning("[ShopSystem] Could not find local PlayerDataSync (IsOwner or ClientId match). ServerRpc will not be sent.");
            return null;
        }

        private void OnRequestReroll(RequestRerollEvent evt)
        {
            var localPlayerSync = GetLocalPlayerSync();
            if (localPlayerSync != null)
            {
                localPlayerSync.CmdRequestReroll();
            }
        }

        private void OnRequestBuyXP(RequestBuyXPEvent evt)
        {
            var localPlayerSync = GetLocalPlayerSync();
            if (localPlayerSync != null)
            {
                localPlayerSync.CmdBuyXP();
            }
        }

        private void OnRequestBuyHero(RequestBuyHeroEvent evt)
        {
            var localPlayerSync = GetLocalPlayerSync();
            if (localPlayerSync != null)
            {
                localPlayerSync.CmdBuyChampion(evt.SlotIndex);
            }
        }

        public void Shutdown()
        {
            this.RemoveListener<RequestRerollEvent>(OnRequestReroll);
            this.RemoveListener<RequestBuyXPEvent>(OnRequestBuyXP);
            this.RemoveListener<RequestBuyHeroEvent>(OnRequestBuyHero);
            this.RemoveListener<GameplayPhaseChangedEvent>(OnPhaseChanged);
            this.RemoveListener<RequestToggleShopLockEvent>(OnRequestToggleShopLock);
            Debug.Log("<color=yellow>ShopSystem shutdown</color>");
        }

        private void OnDestroy()
        {
            Shutdown();
        }

        private void OnPhaseChanged(GameplayPhaseChangedEvent evt)
        {
            if (evt.Phase == GameplayPhase.Planning)
            {

                if (_isShopLocked)
                    SetShopLock(false);
            }
        }

        public void Reroll()
        {
            if (_data == null) return;

        }

        private void OnRequestToggleShopLock(RequestToggleShopLockEvent evt)
        {
            SetShopLock(!_isShopLocked);
        }

        public void SetShopLock(bool locked)
        {
            if (_isShopLocked == locked) return;
            _isShopLocked = locked;
            this.Raise(new ShopLockChangedEvent { IsLocked = _isShopLocked });
            Debug.Log($"<color=cyan>ShopSystem: Shop {(_isShopLocked ? "LOCKED" : "UNLOCKED")}</color>");
        }

        public void SyncShopData(string[] championIds)
        {
            if (_data == null)
            {
                Debug.LogError("[ShopSystem] SyncShopData called but _data (ShopSystemData) is null! " +
                               "Ensure ShopSystemData is assigned and loaded via Addressables.");
                return;
            }

            if (_data.allHeroes == null || _data.allHeroes.Count == 0)
            {
                Debug.LogError("[ShopSystem] SyncShopData: allHeroes is empty! " +
                               "Add ChampionData entries to ShopSystemData.allHeroes in the Inspector.");
                return;
            }

            int resolved = 0;
            for (int i = 0; i < 5; i++)
            {
                if (string.IsNullOrEmpty(championIds[i]))
                {
                    _currentShop[i] = null;
                }
                else
                {
                    _currentShop[i] = _data.allHeroes.FirstOrDefault(h => h != null && h.Id == championIds[i]);
                    if (_currentShop[i] != null) resolved++;
                    else Debug.LogWarning($"[ShopSystem] SyncShopData: id='{championIds[i]}' not found in allHeroes. " +
                                          "Check ChampionData.Id matches exactly.");
                }
            }

            Debug.Log($"<color=cyan>[ShopSystem] SyncShopData: resolved {resolved}/5 champions " +
                      $"(allHeroes={_data.allHeroes.Count}).</color>");

            OnShopRefreshed?.Invoke();
            this.Raise(new ShopRefreshedEvent());
        }

        public void RefreshShop()
        {

        }

        private int RollRarity(float[] chances)
        {
            float roll = Random.value; 
            float cumulative = 0;
            for (int i = 0; i < chances.Length; i++)
            {
                cumulative += chances[i];
                if (roll <= cumulative) return i + 1; 
            }
            return 1;
        }

        private ChampionData GetRandomHeroOfRarity(int rarity)
        {
            if (_data == null || _data.allHeroes == null) return null;
            var eligible = _data.allHeroes.Where(h => h.rarity == rarity).ToList();
            if (eligible.Count == 0) return null;
            return eligible[Random.Range(0, eligible.Count)];
        }

        public void BuyHero(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= _currentShop.Length) return;

            ChampionData hero = _currentShop[slotIndex];
            if (hero == null) return;

            int localPlayerId = 0; 
            if (!_manager.Bench.CanAcceptHero(localPlayerId, hero))
            {
                Debug.LogWarning($"[ShopSystem] Cannot buy {hero.displayName}: Bench is full and no upgrade possible!");
                return;
            }

        }
    }

    public struct ShopRefreshedEvent : IEvent { }
    public struct ShopLockChangedEvent : IEvent { public bool IsLocked; }
    public struct HeroBoughtEvent : IEvent { public ChampionData Hero; }
    public struct HeroSoldEvent : IEvent { public ChampionData Hero; public int GoldRefunded; }

    public struct HeroDragStartedEvent : IEvent { public ChampionActor Hero; }
    public struct HeroDragEndedEvent : IEvent { public ChampionActor Hero; }
}

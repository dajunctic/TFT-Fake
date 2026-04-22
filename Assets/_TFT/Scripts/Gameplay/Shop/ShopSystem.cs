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
            this.RegisterListener<RequestBuyHeroEvent>(OnRequestBuyHero);
            this.RegisterListener<GameplayPhaseChangedEvent>(OnPhaseChanged);
            this.RegisterListener<RequestToggleShopLockEvent>(OnRequestToggleShopLock);

            Debug.Log("<color=cyan>ShopSystem initialized</color>");
        }

        private void OnRequestReroll(RequestRerollEvent evt)
        {
            // Triggers network request via PlayerDataSync
            var localPlayerSync = FishNet.InstanceFinder.ClientManager.Connection.FirstObject?.GetComponent<PlayerDataSync>();
            if (localPlayerSync != null)
            {
                localPlayerSync.CmdRequestReroll();
            }
        }

        private void OnRequestBuyHero(RequestBuyHeroEvent evt)
        {
            var localPlayerSync = FishNet.InstanceFinder.ClientManager.Connection.FirstObject?.GetComponent<PlayerDataSync>();
            if (localPlayerSync != null)
            {
                localPlayerSync.CmdBuyChampion(evt.SlotIndex);
            }
        }

        public void Shutdown()
        {
            this.RemoveListener<RequestRerollEvent>(OnRequestReroll);
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
                {
                    // Shop locked: keep current shop, auto-unlock (TFT behavior: lock lasts 1 round)
                    SetShopLock(false);
                }
                else
                {
                    RefreshShop();
                }
            }
        }

        public void Reroll()
        {
            if (_data == null) return;
            // if (_manager.Economy.SpendGold(_data.shopData.rerollCost))
            // {
            //     RefreshShop();
            //     // Auto-unlock on manual reroll (TFT behavior)
            //     if (_isShopLocked) SetShopLock(false);
            // }
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

        // Called by PlayerDataSync when Server sends TargetUpdateShop
        public void SyncShopData(string[] championIds)
        {
            if (_data == null || _data.allHeroes == null) return;
            
            for (int i = 0; i < 5; i++)
            {
                if (string.IsNullOrEmpty(championIds[i]))
                {
                    _currentShop[i] = null;
                }
                else
                {
                    _currentShop[i] = _data.allHeroes.FirstOrDefault(h => h.Id == championIds[i]);
                }
            }

            OnShopRefreshed?.Invoke();
            this.Raise(new ShopRefreshedEvent());
        }

        public void RefreshShop()
        {
            // if (_data == null) return;
            // // int level = _manager.Economy.Level;
            // float[] chances = _data.shopData.GetChancesForLevel(level);

            // for (int i = 0; i < 5; i++)
            // {
            //     int rarity = RollRarity(chances);
            //     _currentShop[i] = GetRandomHeroOfRarity(rarity);
            // }

            // OnShopRefreshed?.Invoke();
            // this.Raise(new ShopRefreshedEvent());
        }

        private int RollRarity(float[] chances)
        {
            float roll = Random.value; // 0.0 to 1.0
            float cumulative = 0;
            for (int i = 0; i < chances.Length; i++)
            {
                cumulative += chances[i];
                if (roll <= cumulative) return i + 1; // Rarity is 1-indexed
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

            // Check if bench can accept this hero (has space or would trigger upgrade)
            int localPlayerId = 0; // Local player defaults to 0
            if (!_manager.Bench.CanAcceptHero(localPlayerId, hero))
            {
                Debug.LogWarning($"[ShopSystem] Cannot buy {hero.displayName}: Bench is full and no upgrade possible!");
                return;
            }

            // if (_manager.Economy.SpendGold(hero.rarity))
            // {
            //     // Add hero to bench
            //     _manager.Bench.AddHeroToBench(localPlayerId, hero);

            //     Debug.Log($"[ShopSystem] Purchased {hero.displayName} for {hero.rarity} gold.");
            //     _currentShop[slotIndex] = null;

            //     this.Raise(new HeroBoughtEvent { Hero = hero });
            //     this.Raise(new ShopRefreshedEvent());
            //     OnShopRefreshed?.Invoke();
            // }
            // else
            // {
            //     Debug.LogWarning($"[ShopSystem] Cannot buy {hero.displayName}: Not enough gold ({_manager.Economy.Gold}/{hero.rarity})");
            // }
        }
    }

    public struct ShopRefreshedEvent : IEvent { }
    public struct ShopLockChangedEvent : IEvent { public bool IsLocked; }
    public struct HeroBoughtEvent : IEvent { public ChampionData Hero; }
    public struct HeroSoldEvent : IEvent { public ChampionData Hero; public int GoldRefunded; }

    public struct HeroDragStartedEvent : IEvent { public ChampionActor Hero; }
    public struct HeroDragEndedEvent : IEvent { public ChampionActor Hero; }
}

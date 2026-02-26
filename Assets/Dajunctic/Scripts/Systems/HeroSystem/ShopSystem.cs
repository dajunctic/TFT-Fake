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

        private HeroData[] _currentShop = new HeroData[5];
        public HeroData[] CurrentShop => _currentShop;
        public ShopData ShopData => _data?.shopData;
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

            Debug.Log("<color=cyan>ShopSystem initialized</color>");
        }

        private void OnRequestReroll(RequestRerollEvent evt)
        {
            Reroll();
        }

        private void OnRequestBuyHero(RequestBuyHeroEvent evt)
        {
            BuyHero(evt.SlotIndex);
        }

        public void Shutdown()
        {
            this.RemoveListener<RequestRerollEvent>(OnRequestReroll);
            this.RemoveListener<RequestBuyHeroEvent>(OnRequestBuyHero);
            this.RemoveListener<GameplayPhaseChangedEvent>(OnPhaseChanged);
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
                RefreshShop();
            }
        }

        public void Reroll()
        {
            if (_data == null) return;
            if (_manager.Economy.SpendGold(_data.shopData.rerollCost))
            {
                RefreshShop();
            }
        }

        public void RefreshShop()
        {
            if (_data == null) return;
            int level = _manager.Economy.Level;
            float[] chances = _data.shopData.GetChancesForLevel(level);

            for (int i = 0; i < 5; i++)
            {
                int rarity = RollRarity(chances);
                _currentShop[i] = GetRandomHeroOfRarity(rarity);
            }

            OnShopRefreshed?.Invoke();
            this.Raise(new ShopRefreshedEvent());
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

        private HeroData GetRandomHeroOfRarity(int rarity)
        {
            if (_data == null || _data.allHeroes == null) return null;
            var eligible = _data.allHeroes.Where(h => h.rarity == rarity).ToList();
            if (eligible.Count == 0) return null;
            return eligible[Random.Range(0, eligible.Count)];
        }

        public void BuyHero(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= _currentShop.Length) return;

            HeroData hero = _currentShop[slotIndex];
            if (hero == null) return;

            // Check if bench can accept this hero (has space or would trigger upgrade)
            if (!_manager.Bench.CanAcceptHero(hero))
            {
                Debug.LogWarning($"[ShopSystem] Cannot buy {hero.displayName}: Bench is full and no upgrade possible!");
                return;
            }

            if (_manager.Economy.SpendGold(hero.rarity))
            {
                // Add hero to bench
                _manager.Bench.AddHeroToBench(hero);

                Debug.Log($"[ShopSystem] Purchased {hero.displayName} for {hero.rarity} gold.");
                _currentShop[slotIndex] = null;

                this.Raise(new HeroBoughtEvent { Hero = hero });
                this.Raise(new ShopRefreshedEvent());
                OnShopRefreshed?.Invoke();
            }
            else
            {
                Debug.LogWarning($"[ShopSystem] Cannot buy {hero.displayName}: Not enough gold ({_manager.Economy.Gold}/{hero.rarity})");
            }
        }
    }

    public struct ShopRefreshedEvent : IEvent { }
    public struct HeroBoughtEvent : IEvent { public HeroData Hero; }
    public struct HeroSoldEvent : IEvent { public HeroData Hero; public int GoldRefunded; }

    public struct HeroDragStartedEvent : IEvent { public HeroCombatActor Hero; }
    public struct HeroDragEndedEvent : IEvent { public HeroCombatActor Hero; }
}

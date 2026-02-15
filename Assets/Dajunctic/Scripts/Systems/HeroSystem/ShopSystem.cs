using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Dajunctic
{
    public class ShopSystem : MonoBehaviour, IGameSystem
    {
        [SerializeField] private ShopData shopData;
        [SerializeField] private List<HeroData> allHeroes; // Ideally populated from a database or folder
        
        private HeroData[] _currentShop = new HeroData[5];
        public HeroData[] CurrentShop => _currentShop;
        public ShopData ShopData => shopData;
        private GameSystemManager _manager;

        public static event System.Action OnShopRefreshed;

        public void Initialize(GameSystemManager manager)
        {
            _manager = manager;
            if (allHeroes == null || allHeroes.Count == 0)
            {
                allHeroes = Resources.LoadAll<HeroData>("").ToList(); 
                // Commented out to prevent slow loading if not needed immediately
            }

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
            if (_manager.Economy.SpendGold(shopData.rerollCost))
            {
                RefreshShop();
            }
        }

        public void RefreshShop()
        {
            int level = _manager.Economy.Level;
            float[] chances = shopData.GetChancesForLevel(level);

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
            var eligible = allHeroes.Where(h => h.rarity == rarity).ToList();
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
                Debug.LogWarning("Bench is full and no upgrade possible!");
                return;
            }

            if (_manager.Economy.SpendGold(hero.rarity))
            {
                // Add hero to bench
                _manager.Bench.AddHeroToBench(hero);
                
                Debug.Log($"Bought {hero.displayName}");
                _currentShop[slotIndex] = null;
                
                this.Raise(new HeroBoughtEvent { Hero = hero });
                this.Raise(new ShopRefreshedEvent());
                OnShopRefreshed?.Invoke();
            }
        }
    }

    public struct ShopRefreshedEvent : IEvent { }
    public struct HeroBoughtEvent : IEvent { public HeroData Hero; }
    public struct HeroSoldEvent : IEvent { public HeroData Hero; public int GoldRefunded; }
}

using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Dajunctic
{
    public class ShopController : Singleton<ShopController>
    {
        [SerializeField] private ShopData shopData;
        [SerializeField] private List<HeroData> allHeroes; // Ideally populated from a database or folder
        
        private HeroData[] _currentShop = new HeroData[5];
        public HeroData[] CurrentShop => _currentShop;
        public ShopData ShopData => shopData;

        public static event System.Action OnShopRefreshed;

        protected override void Awake()
        {
            base.Awake();
            if (allHeroes == null || allHeroes.Count == 0)
            {
                allHeroes = Resources.LoadAll<HeroData>("").ToList();
            }

            Gameplay.OnPhaseChanged += OnPhaseChanged;
        }

        private void OnDestroy()
        {
            Gameplay.OnPhaseChanged -= OnPhaseChanged;
        }

        private void OnPhaseChanged(GameplayPhase phase)
        {
            if (phase == GameplayPhase.Planning)
            {
                RefreshShop();
            }
        }

        public void Reroll()
        {
            if (EconomyManager.Instance.SpendGold(shopData.rerollCost))
            {
                RefreshShop();
            }
        }

        public void RefreshShop()
        {
            int level = EconomyManager.Instance.Level;
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

            // Check if bench is full
            if (!BenchManager.Instance.HasEmptySlot())
            {
                Debug.LogWarning("Bench is full! Cannot buy hero.");
                return;
            }

            if (EconomyManager.Instance.SpendGold(hero.rarity))
            {
                // Add hero to bench
                BenchManager.Instance.AddHeroToBench(hero);
                
                Debug.Log($"Bought {hero.displayName}");
                this.Raise(new HeroBoughtEvent { Hero = hero });
                _currentShop[slotIndex] = null;
                OnShopRefreshed?.Invoke();
            }
        }
    }

    public struct ShopRefreshedEvent : IEvent { }
    public struct HeroBoughtEvent : IEvent { public HeroData Hero; }
}

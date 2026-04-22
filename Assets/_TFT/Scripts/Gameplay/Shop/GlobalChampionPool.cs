using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Dajunctic
{
    public class GlobalChampionPool : MonoBehaviour, IGameSystem
    {
        private GameSystemManager _manager;
        private ShopSystemData _data;
        
        // Dictionary mapping rarity -> max copies per champion of that rarity
        private readonly Dictionary<int, int> RarityPoolSizes = new Dictionary<int, int>
        {
            { 1, 29 }, // Tier 1: 29 copies
            { 2, 22 }, // Tier 2: 22 copies
            { 3, 18 }, // Tier 3: 18 copies
            { 4, 12 }, // Tier 4: 12 copies
            { 5, 10 }  // Tier 5: 10 copies
        };

        // Champion ID -> remaining copies
        private Dictionary<string, int> _pool = new Dictionary<string, int>();

        public System.Threading.Tasks.Task LoadDataAsync()
        {
            return System.Threading.Tasks.Task.CompletedTask;
        }

        public void Initialize(GameSystemManager manager)
        {
            _manager = manager;

            if (FishNet.InstanceFinder.IsServerStarted)
            {
                InitializePool();
            }
        }

        private void InitializePool()
        {
            _pool.Clear();
            _data = _manager.Shop.ShopSystemData; // Assuming ShopSystem exposes it

            if (_data != null && _data.allHeroes != null)
            {
                foreach (var hero in _data.allHeroes)
                {
                    if (hero != null && RarityPoolSizes.TryGetValue(hero.rarity, out int size))
                    {
                        _pool[hero.Id] = size;
                    }
                }
            }
        }

        /// <summary>
        /// Server-side: Draws a champion of a specific rarity from the global pool.
        /// Returns null if no champions of that rarity are left.
        /// </summary>
        public ChampionData DrawChampion(int rarity)
        {
            if (_data == null || _data.allHeroes == null) return null;

            // Get all heroes of this rarity that still have copies left in the pool
            var eligible = _data.allHeroes
                .Where(h => h.rarity == rarity && _pool.GetValueOrDefault(h.Id, 0) > 0)
                .ToList();

            if (eligible.Count == 0) return null;

            // Pick a random eligible hero
            var pickedHero = eligible[Random.Range(0, eligible.Count)];
            
            // Remove 1 copy from the pool
            _pool[pickedHero.Id]--;

            return pickedHero;
        }

        /// <summary>
        /// Server-side: Returns a champion back to the global pool (when sold or player dies).
        /// </summary>
        public void ReturnChampion(string championId)
        {
            if (_pool.ContainsKey(championId))
            {
                _pool[championId]++;
            }
        }

        public void Shutdown()
        {
            _pool.Clear();
        }
    }
}

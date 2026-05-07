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
        private bool _poolInitialized;

        public System.Threading.Tasks.Task LoadDataAsync()
        {
            return System.Threading.Tasks.Task.CompletedTask;
        }

        public void Initialize(GameSystemManager manager)
        {
            _manager = manager;
            // NOTE: Do NOT call InitializePool() here.
            // ShopSystem.LoadDataAsync() hasn't run yet at this point so
            // Shop.ShopSystemData is still null → pool would be empty.
            // InitializeAfterDataLoad() is called by GameSystemManager after all data is loaded.
        }

        /// <summary>
        /// Called by GameSystemManager immediately after all LoadDataAsync() calls complete.
        /// Server may not have started yet at this point, so we subscribe to the server
        /// started event and initialize the pool when the server is ready.
        /// </summary>
        public void InitializeAfterDataLoad()
        {
            // Cache data now — it's guaranteed loaded at this point
            _data = _manager.Shop?.ShopSystemData;

            if (FishNet.InstanceFinder.IsServerStarted)
            {
                // Edge case: server already running (e.g. hot-reload in editor)
                InitializePool();
            }
            else
            {
                // Normal flow: wait for the server to start (user clicks Host)
                FishNet.InstanceFinder.ServerManager.OnServerConnectionState += OnServerConnectionState;
            }
        }

        private void OnServerConnectionState(FishNet.Transporting.ServerConnectionStateArgs args)
        {
            if (args.ConnectionState == FishNet.Transporting.LocalConnectionState.Started)
            {
                FishNet.InstanceFinder.ServerManager.OnServerConnectionState -= OnServerConnectionState;
                InitializePool();
            }
        }

        private void InitializePool()
        {
            _pool.Clear();
            _poolInitialized = false;

            _data = _manager.Shop?.ShopSystemData;

            if (_data == null)
            {
                Debug.LogError("[GlobalChampionPool] ShopSystemData is null! Cannot initialize pool. " +
                               "Ensure ShopSystemData is assigned and loaded before InitializePool() is called.");
                return;
            }

            if (_data.allHeroes == null || _data.allHeroes.Count == 0)
            {
                Debug.LogError("[GlobalChampionPool] ShopSystemData.allHeroes is empty! " +
                               "Add ChampionData assets to the allHeroes list in ShopSystemData.");
                return;
            }

            int added = 0;
            foreach (var hero in _data.allHeroes)
            {
                if (hero == null) continue;
                if (string.IsNullOrEmpty(hero.Id))
                {
                    Debug.LogWarning($"[GlobalChampionPool] ChampionData '{hero.displayName}' has empty Id — skipped.");
                    continue;
                }
                if (RarityPoolSizes.TryGetValue(hero.rarity, out int size))
                {
                    _pool[hero.Id] = size;
                    added++;
                }
                else
                {
                    Debug.LogWarning($"[GlobalChampionPool] ChampionData '{hero.displayName}' has unsupported rarity {hero.rarity} (expected 1-5).");
                }
            }

            _poolInitialized = added > 0;
            Debug.Log($"<color=green>[GlobalChampionPool] Pool initialized with {added} champions.</color>");
        }

        /// <summary>
        /// Server-side: Draws a champion of a specific rarity from the global pool.
        /// Returns null if no champions of that rarity are left.
        /// </summary>
        public ChampionData DrawChampion(int rarity)
        {
            // Lazy-init fallback: if server started but pool wasn't initialized yet
            if (!_poolInitialized && FishNet.InstanceFinder.IsServerStarted)
            {
                Debug.LogWarning("[GlobalChampionPool] Pool not initialized at DrawChampion — running lazy init.");
                InitializePool();
            }

            if (!_poolInitialized)
            {
                Debug.LogError("[GlobalChampionPool] Pool still not initialized after lazy init attempt!");
                return null;
            }

            if (_data == null || _data.allHeroes == null) return null;

            // Get all heroes of this rarity that still have copies left in the pool
            var eligible = _data.allHeroes
                .Where(h => h != null && h.rarity == rarity && _pool.GetValueOrDefault(h.Id, 0) > 0)
                .ToList();

            if (eligible.Count == 0)
            {
                Debug.LogWarning($"[GlobalChampionPool] No champions left in pool for rarity {rarity}.");
                return null;
            }

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
            if (FishNet.InstanceFinder.ServerManager != null)
                FishNet.InstanceFinder.ServerManager.OnServerConnectionState -= OnServerConnectionState;

            _pool.Clear();
            _poolInitialized = false;
        }
    }
}

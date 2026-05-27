using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Dajunctic
{
    public class GlobalChampionPool : MonoBehaviour, IGameSystem
    {
        private GameSystemManager _manager;
        private ShopSystemData _data;

        private readonly Dictionary<int, int> RarityPoolSizes = new Dictionary<int, int>
        {
            { 1, 29 }, 
            { 2, 22 }, 
            { 3, 18 }, 
            { 4, 10 }, 
            { 5, 9 }   
        };

        private Dictionary<string, int> _pool = new Dictionary<string, int>();
        private bool _poolInitialized;

        public System.Threading.Tasks.Task LoadDataAsync()
        {
            return System.Threading.Tasks.Task.CompletedTask;
        }

        public void Initialize(GameSystemManager manager)
        {
            _manager = manager;

        }

        public void InitializeAfterDataLoad()
        {
            
            _data = _manager.Shop?.ShopSystemData;

            var nm = FishNet.InstanceFinder.NetworkManager;
            if (nm != null && nm.IsServerStarted)
            {
                
                InitializePool();
            }
            else if (nm != null && nm.ServerManager != null)
            {
                
                nm.ServerManager.OnServerConnectionState += OnServerConnectionState;
            }
            else
            {
                Debug.LogWarning("[GlobalChampionPool] FishNet NetworkManager is missing. Cannot subscribe to ServerManager.");
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

        public ChampionData DrawChampion(int rarity)
        {
            
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

            var eligible = _data.allHeroes
                .Where(h => h != null && h.rarity == rarity && _pool.GetValueOrDefault(h.Id, 0) > 0)
                .ToList();

            if (eligible.Count == 0)
            {
                Debug.LogWarning($"[GlobalChampionPool] No champions left in pool for rarity {rarity}.");
                return null;
            }

            var pickedHero = eligible[Random.Range(0, eligible.Count)];

            _pool[pickedHero.Id]--;

            return pickedHero;
        }

        public void ReturnChampion(string championId)
        {
            if (_pool.ContainsKey(championId))
            {
                _pool[championId]++;
            }
        }

        public void Shutdown()
        {
            var nm = FishNet.InstanceFinder.NetworkManager;
            if (nm != null && nm.ServerManager != null)
                nm.ServerManager.OnServerConnectionState -= OnServerConnectionState;

            _pool.Clear();
            _poolInitialized = false;
        }
    }
}

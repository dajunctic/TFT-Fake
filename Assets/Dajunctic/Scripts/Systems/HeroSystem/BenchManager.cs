using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Dajunctic
{
    public class BenchManager : Singleton<BenchManager>
    {
        [SerializeField] private SquareAreaView benchArea;
        private Dictionary<Vector2Int, HeroCombatActor> _heroOnTiles = new Dictionary<Vector2Int, HeroCombatActor>();

        public bool HasEmptySlot()
        {
            if (benchArea == null || benchArea.Data == null) return false;
            return _heroOnTiles.Count < benchArea.Data.ActiveTiles.Count;
        }

        public Vector2Int GetFirstEmptyTileCoord()
        {
            if (benchArea == null || benchArea.Data == null) return new Vector2Int(-1, -1);

            foreach (var tile in benchArea.Data.ActiveTiles)
            {
                if (!_heroOnTiles.ContainsKey(tile.coordinates))
                {
                    return tile.coordinates;
                }
            }
            return new Vector2Int(-1, -1);
        }

        public void AddHeroToBench(HeroData heroData)
        {
            Vector2Int coord = GetFirstEmptyTileCoord();
            if (coord.x == -1)
            {
                Debug.LogWarning("Bench is full!");
                return;
            }

            if (heroData.prefab == null)
            {
                Debug.LogError($"Hero {heroData.displayName} has no prefab assigned!");
                return;
            }

            // Get world position from bench area
            Vector3 localPos = benchArea.Data.SquareToWorld(Vector3.zero, coord);
            Vector3 worldPos = benchArea.CachedTransform.TransformPoint(localPos);

            GameObject heroObj = Instantiate(heroData.prefab, worldPos, benchArea.CachedTransform.rotation);
            HeroCombatActor actor = heroObj.GetComponent<HeroCombatActor>();
            
            if (actor != null)
            {
                _heroOnTiles[coord] = actor;
                actor.CurrentBenchCoord = coord;
                actor.Initialize();
                
                // Check for upgrades (merging 3 same heroes)
                CheckForUpgrades(heroData);
            }
        }

        private void CheckForUpgrades(HeroData heroData)
        {
            var sameHeroes = _heroOnTiles.Values
                .Where(h => h != null && h.combatActorData.name == heroData.name)
                .ToList();
            
            if (sameHeroes.Count >= 3)
            {
                MergeHeroes(sameHeroes.Take(3).ToList(), heroData);
            }
        }

        private void MergeHeroes(List<HeroCombatActor> instances, HeroData heroData)
        {
            Debug.Log($"Upgrading {heroData.displayName} to 2 stars!");
            
            // Remove old instances
            var keysToRemove = new List<Vector2Int>();
            foreach (var kvp in _heroOnTiles)
            {
                if (instances.Contains(kvp.Value))
                {
                    Destroy(kvp.Value.gameObject);
                    keysToRemove.Add(kvp.Key);
                }
            }

            foreach (var key in keysToRemove)
            {
                _heroOnTiles.Remove(key);
            }
            
            AddHeroToBench(heroData);
        }

        public void RemoveHeroFromTile(Vector2Int coord)
        {
            if (_heroOnTiles.ContainsKey(coord))
            {
                _heroOnTiles.Remove(coord);
            }
        }

        // Helper to find coord by actor
        public Vector2Int GetCoordOfActor(HeroCombatActor actor)
        {
            foreach (var kvp in _heroOnTiles)
            {
                if (kvp.Value == actor) return kvp.Key;
            }
            return new Vector2Int(-1, -1);
        }
    }
}

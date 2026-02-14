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
            return GetFirstEmptyTileCoord().x != -1;
        }

        public Vector2Int GetFirstEmptyTileCoord()
        {
            if (benchArea == null || benchArea.Data == null) return new Vector2Int(-1, -1);

            // Sort tiles: Primary by X (Left to Right), Secondary by Y (Bottom to Top)
            // This ensures filling the first row from left to right.
            var sortedTiles = benchArea.Data.ActiveTiles
                .OrderBy(t => t.coordinates.x)
                .ThenBy(t => t.coordinates.y)
                .ToList();

            foreach (var tile in sortedTiles)
            {
                // Robust check: Tile is empty if key doesn't exist OR the actor registered there is null/destroyed
                if (!_heroOnTiles.TryGetValue(tile.coordinates, out var occupant) || occupant == null)
                {
                    return tile.coordinates;
                }
            }
            return new Vector2Int(-1, -1);
        }

        public void AddHeroToBench(HeroData heroData, int starLevel = 1)
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
                actor.SetStarLevel(starLevel);
                actor.Initialize();
                
                // Check for upgrades (merging 3 same heroes of same star level)
                CheckForUpgrades(heroData, starLevel);
            }
        }

        private void CheckForUpgrades(HeroData heroData, int starLevel)
        {
            if (starLevel >= 3) return;

            // Find all matching heroes on BENCH by comparing the HeroData reference
            var heroesOnBench = _heroOnTiles.Values
                .Where(h => h != null && h.CombatActorData is HeroData data && 
                            data == heroData && 
                            h.StarLevel == starLevel)
                .ToList();

            // Find all matching heroes on FIELD
            var heroesOnField = new List<HeroCombatActor>();
            if (FieldManager.Instance != null)
            {
                heroesOnField = FieldManager.Instance.GetAllHeroes()
                    .Where(h => h != null && h.CombatActorData is HeroData data && 
                            data == heroData && 
                                h.StarLevel == starLevel)
                    .ToList();
            }

            var allMatching = heroesOnBench.Concat(heroesOnField).ToList();
            
            if (allMatching.Count >= 3)
            {
                MergeHeroes(allMatching.Take(3).ToList(), heroData, starLevel + 1);
            }
        }

        private void MergeHeroes(List<HeroCombatActor> instances, HeroData heroData, int newStarLevel)
        {
            Debug.Log($"Upgrading {heroData.displayName} to {newStarLevel} stars!");

            // 1. Identify where to put the new unit (Preferably on field if one was there)
            HeroCombatActor primary = instances.FirstOrDefault(h => h.IsOnField);
            if (primary == null) primary = instances[0];

            bool wasOnField = primary.IsOnField;
            Vector2Int targetCoord = wasOnField ? primary.CurrentFieldCoord : primary.CurrentBenchCoord;

            // 2. Remove all 3 from managers and destroy
            foreach (var hero in instances)
            {
                if (BenchManager.Instance != null) BenchManager.Instance.UnregisterHero(hero);
                if (FieldManager.Instance != null) FieldManager.Instance.UnregisterHero(hero);
                
                Destroy(hero.gameObject);
            }
            
            // 3. Spawn new unit at the primary location
            if (wasOnField)
            {
                FieldManager.Instance.AddHeroToField(heroData, targetCoord, newStarLevel);
            }
            else
            {
                AddHeroToBenchAtCoord(heroData, targetCoord, newStarLevel);
            }

            // 4. IMPORTANT: Check for upgrades again for the new level (e.g., 2rd star to 3rd star)
            CheckForUpgrades(heroData, newStarLevel);
        }

        public void RemoveHeroFromTile(Vector2Int coord)
        {
            if (_heroOnTiles.ContainsKey(coord))
            {
                _heroOnTiles.Remove(coord);
            }
        }

        public bool TrySnapToBench(Vector3 worldPos, out Vector2Int coord)
        {
            coord = new Vector2Int(-1, -1);
            if (benchArea == null || benchArea.Data == null) return false;

            Vector3 localPos = benchArea.CachedTransform.InverseTransformPoint(worldPos);
            Vector2Int squareCoords = benchArea.Data.WorldToSquare(localPos, Vector3.zero);

            if (benchArea.Data.TryGetTile(squareCoords, out _))
            {
                coord = squareCoords;
                return true;
            }
            return false;
        }

        public void AddHeroToBenchAtCoord(HeroData heroData, Vector2Int coord, int starLevel)
        {
            Vector3 worldPos = GetWorldPosition(coord);
            GameObject heroObj = Instantiate(heroData.prefab, worldPos, benchArea.CachedTransform.rotation);
            HeroCombatActor actor = heroObj.GetComponent<HeroCombatActor>();
            
            if (actor != null)
            {
                actor.CurrentBenchCoord = coord;
                actor.SetStarLevel(starLevel);
                actor.Initialize();
                RegisterHeroToTile(actor, coord);
            }
        }

        public void RegisterHeroToTile(HeroCombatActor actor, Vector2Int coord)
        {
            UnregisterHero(actor);
            _heroOnTiles[coord] = actor;
            actor.CurrentBenchCoord = coord;
        }

        public void UnregisterHero(HeroCombatActor actor)
        {
            Vector2Int keyToRemove = new Vector2Int(-1, -1);
            foreach (var kvp in _heroOnTiles)
            {
                if (kvp.Value == actor)
                {
                    keyToRemove = kvp.Key;
                    break;
                }
            }
            if (keyToRemove.x != -1) _heroOnTiles.Remove(keyToRemove);
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
        public HeroCombatActor GetHeroAtTile(Vector2Int coord)
        {
            _heroOnTiles.TryGetValue(coord, out var actor);
            return actor;
        }

        public Vector3 GetWorldPosition(Vector2Int coord)
        {
            Vector3 localPos = benchArea.Data.SquareToWorld(Vector3.zero, coord);
            return benchArea.CachedTransform.TransformPoint(localPos);
        }
    }
}

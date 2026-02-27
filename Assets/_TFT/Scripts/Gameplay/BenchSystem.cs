using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

namespace Dajunctic
{
    public class BenchSystem : MonoBehaviour, IGameSystem
    {
        // Scene refs — bound at runtime by BenchAreaBinder in the gameplay scene
        private SquareAreaView _benchArea;
        private string _fxGuid;
        private Dictionary<Vector2Int, HeroCombatActor> _heroOnTiles = new Dictionary<Vector2Int, HeroCombatActor>();
        private GameSystemManager _manager;

        public async Task LoadDataAsync()
        {
            // BenchSystem has no Addressable data — scene refs are bound via BenchAreaBinder
            await Task.CompletedTask;
            Debug.Log("<color=cyan>BenchSystem data loaded (no-op)</color>");
        }

        public void Initialize(GameSystemManager manager)
        {
            _manager = manager;
            Debug.Log("<color=cyan>BenchSystem initialized</color>");
        }

        /// <summary>Called by BenchAreaBinder when the gameplay scene loads.</summary>
        public void BindArea(SquareAreaView area, string fxGuid)
        {
            _benchArea = area;
            _fxGuid = fxGuid;
        }

        public void Shutdown()
        {
            _heroOnTiles.Clear();
            Debug.Log("<color=yellow>BenchSystem shutdown</color>");
        }

        public bool HasEmptySlot()
        {
            return GetFirstEmptyTileCoord().y >= 0;
        }

        /// <summary>
        /// Check if we can accept a hero: either bench has space, or buying would trigger an upgrade.
        /// </summary>
        public bool CanAcceptHero(HeroData heroData, int starLevel = 1)
        {
            if (HasEmptySlot()) return true;
            return WouldTriggerUpgrade(heroData, starLevel);
        }

        /// <summary>
        /// Check if adding one more hero of this type/star would trigger a 3-to-1 merge.
        /// </summary>
        private bool WouldTriggerUpgrade(HeroData heroData, int starLevel)
        {
            if (starLevel >= 5) return false;
            // Need 2 existing + 1 new (purchased) = 3 total
            return GetMatchingHeroes(heroData, starLevel).Count >= 2;
        }

        public Vector2Int GetFirstEmptyTileCoord()
        {
            if (_benchArea == null || _benchArea.Data == null) return new Vector2Int(-10, -10);

            var sortedTiles = _benchArea.Data.ActiveTiles
                .OrderBy(t => t.coordinates.x)
                .ThenBy(t => t.coordinates.y)
                .ToList();



            // Debug.LogError("<color=green>Checking bench tiles for empty slot.." + $" Total tiles: {sortedTiles.Count}, Occupied: {_heroOnTiles.Count}</color>");

            // // Dump all dictionary entries
            // Debug.LogError("<color=magenta>=== DICTIONARY DUMP ===</color>");
            // foreach (var kvp in _heroOnTiles)
            // {
            //     Debug.LogError($"<color=magenta>Dict Entry: {kvp.Key} -> {(kvp.Value != null ? kvp.Value.name : "NULL")}</color>");
            // }
            // Debug.LogError("<color=magenta>======================</color>");


            foreach (var tile in sortedTiles)
            {

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

            if (coord.y < 0)
            {
                // Bench is full — try direct upgrade without placing the hero
                if (TryDirectUpgrade(heroData, starLevel))
                    return;

                Debug.LogWarning("Bench is full!");
                return;
            }

            if (heroData.prefab == null)
            {
                Debug.LogError($"Hero {heroData.displayName} has no prefab assigned!");
                return;
            }

            Vector3 localPos = _benchArea.Data.SquareToWorld(Vector3.zero, coord);
            Vector3 worldPos = _benchArea.CachedTransform.TransformPoint(localPos);

            GameObject heroObj = Instantiate(heroData.prefab, worldPos, _benchArea.CachedTransform.rotation);
            HeroCombatActor actor = heroObj.GetComponent<HeroCombatActor>();

            if (actor != null)
            {
                _heroOnTiles[coord] = actor;
                actor.CurrentBenchCoord = coord;
                actor.SetStarLevel(starLevel);
                actor.Initialize();

                CheckForUpgrades(heroData, starLevel);
            }
        }

        /// <summary>
        /// Perform an upgrade when bench is full. The purchased hero is consumed
        /// without being placed — we only need 2 existing copies on board.
        /// </summary>
        private bool TryDirectUpgrade(HeroData heroData, int starLevel)
        {
            if (starLevel >= 5) return false;

            var allMatching = GetMatchingHeroes(heroData, starLevel);

            if (allMatching.Count >= 2)
            {
                // Take 2 existing — the purchased hero is the virtual 3rd
                MergeHeroes(allMatching.Take(2).ToList(), heroData, starLevel + 1);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Find all heroes matching the given HeroData and star level across bench and field.
        /// </summary>
        private List<HeroCombatActor> GetMatchingHeroes(HeroData heroData, int starLevel)
        {
            if (heroData == null) return new List<HeroCombatActor>();

            var heroesOnBench = _heroOnTiles.Values
                .Where(h => h != null && h.CombatActorData != null &&
                            h.CombatActorData.Id == heroData.Id &&
                            h.StarLevel == starLevel)
                .ToList();

            var heroesOnField = new List<HeroCombatActor>();
            if (_manager.Field != null)
            {
                heroesOnField = _manager.Field.GetAllHeroes()
                    .Where(h => h != null && h.CombatActorData != null &&
                                h.CombatActorData.Id == heroData.Id &&
                                h.StarLevel == starLevel)
                    .ToList();
            }

            return heroesOnBench.Concat(heroesOnField).ToList();
        }

        private void CheckForUpgrades(HeroData heroData, int starLevel)
        {
            if (starLevel >= 5) return;

            var allMatching = GetMatchingHeroes(heroData, starLevel);

            if (allMatching.Count >= 3)
            {
                MergeHeroes(allMatching.Take(3).ToList(), heroData, starLevel + 1);
            }
        }

        private void MergeHeroes(List<HeroCombatActor> instances, HeroData heroData, int newStarLevel)
        {
            Debug.Log($"Upgrading {heroData.displayName} to {newStarLevel} stars!");

            // 1. Determine placement: field merges stay on field, bench merges use leftmost merged hero position
            HeroCombatActor fieldHero = instances.FirstOrDefault(h => h.IsOnField);
            bool wasOnField = fieldHero != null;

            Vector2Int targetCoord;
            if (wasOnField)
            {
                // Keep on field at the position of the field hero
                targetCoord = fieldHero.CurrentFieldCoord;
            }
            else
            {
                // All on bench - find the leftmost position among merged heroes
                // This ensures merging heroes at 1,2,3 results in merged hero at position 1
                targetCoord = instances
                    .Select(h => h.CurrentBenchCoord)
                    .OrderBy(coord => coord.x)
                    .ThenBy(coord => coord.y)
                    .First();
            }


            // 2. Remove all instances from managers and destroy

            foreach (var hero in instances)
            {
                UnregisterHero(hero); // Call local method directly
                if (_manager.Field != null) _manager.Field.UnregisterHero(hero);

                // Cleanup MoveAgent properly - return to pool if pooled, destroy if not
                if (hero.MoveAgent != null)
                {
                    hero.MoveAgent.SetEnable(false);

                    // Call Cleanup if it's a pooled NavMeshMoveAgent to return it to pool
                    if (hero.MoveAgent is NavMeshMoveAgent navMeshAgent)
                    {
                        navMeshAgent.Cleanup();
                    }

                    hero.MoveAgent = null;
                }
                hero.InterruptAction();
                hero.ForceStop();

                Destroy(hero.gameObject);
            }

            // 3. Spawn upgraded unit at the primary location
            Vector3 spawnPos = wasOnField ? _manager.Field.GetWorldPosition(targetCoord) : GetWorldPosition(targetCoord);

            if (wasOnField)
            {
                _manager.Field.AddHeroToField(heroData, targetCoord, newStarLevel);
            }
            else
            {
                AddHeroToBenchAtCoord(heroData, targetCoord, newStarLevel);
            }

            // 4. Spawn merge effect FX at the upgraded hero position
            if (!string.IsNullOrEmpty(_fxGuid))
            {
                this.Raise(new SpawnFxEvent
                {
                    id = _fxGuid,
                    position = spawnPos,
                    duration = 1f
                });
            }

            // 4. Chain upgrade check (e.g., three 2★ → one 3★)
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
            if (_benchArea == null || _benchArea.Data == null) return false;

            Vector3 localPos = _benchArea.CachedTransform.InverseTransformPoint(worldPos);
            Vector2Int squareCoords = _benchArea.Data.WorldToSquare(localPos, Vector3.zero);

            if (_benchArea.Data.TryGetTile(squareCoords, out _))
            {
                coord = squareCoords;
                return true;
            }
            return false;
        }

        public void AddHeroToBenchAtCoord(HeroData heroData, Vector2Int coord, int starLevel)
        {
            Vector3 worldPos = GetWorldPosition(coord);
            GameObject heroObj = Instantiate(heroData.prefab, worldPos, _benchArea.CachedTransform.rotation);
            HeroCombatActor actor = heroObj.GetComponent<HeroCombatActor>();

            if (actor != null)
            {
                actor.SetStarLevel(starLevel);
                actor.Initialize();
                RegisterHeroToTile(actor, coord);
            }
        }

        public void RegisterHeroToTile(HeroCombatActor actor, Vector2Int coord)
        {
            UnregisterHero(actor);
            // Cross-zone cleanup: moving to bench means leaving field
            if (_manager.Field != null) _manager.Field.UnregisterHero(actor);

            // Remove any existing entry at this coord (in case of stale data)
            if (_heroOnTiles.ContainsKey(coord))
            {
                Debug.LogWarning($"Tile {coord} already has an entry! Removing stale data.");
                _heroOnTiles.Remove(coord);
            }

            _heroOnTiles[coord] = actor;
            actor.CurrentBenchCoord = coord;
            actor.CurrentFieldCoord = new Vector2Int(-1, -1);
        }

        public void UnregisterHero(HeroCombatActor actor)
        {
            // Remove ALL entries for this actor (not just first one)
            var keysToRemove = new List<Vector2Int>();
            foreach (var kvp in _heroOnTiles)
            {
                if (kvp.Value == actor || kvp.Value == null)
                {
                    keysToRemove.Add(kvp.Key);
                }
            }

            foreach (var key in keysToRemove)
            {
                _heroOnTiles.Remove(key);
            }
        }

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
            Vector3 localPos = _benchArea.Data.SquareToWorld(Vector3.zero, coord);
            return _benchArea.CachedTransform.TransformPoint(localPos);
        }
    }
}

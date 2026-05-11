using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

namespace Dajunctic
{
    public class BenchSystem : MonoBehaviour, IGameSystem
    {
        // Scene refs — bound at runtime by BenchAreaBinder in the gameplay scene
        private List<Arena> _arenas = new List<Arena>();
        private Dictionary<int, Dictionary<Vector2Int, ChampionActor>> _heroesOnArenas = new Dictionary<int, Dictionary<Vector2Int, ChampionActor>>();
        private GameSystemManager _manager;
        private string _fxGuid;

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

        /// <summary>Called by Arena when it's initialized or by a binder.</summary>
        public void RegisterArena(Arena arena, string fxGuid)
        {
            if (!_arenas.Contains(arena)) _arenas.Add(arena);
            _fxGuid = fxGuid;
            if (!_heroesOnArenas.ContainsKey(arena.OwnerID))
                _heroesOnArenas[arena.OwnerID] = new Dictionary<Vector2Int, ChampionActor>();
        }

        public Arena GetArena(int ownerId) => _arenas.Find(a => a.OwnerID == ownerId);

        public void Shutdown()
        {
            foreach (var dict in _heroesOnArenas.Values) dict.Clear();
            _heroesOnArenas.Clear();
            _arenas.Clear();
            Debug.Log("<color=yellow>BenchSystem shutdown</color>");
        }

        public bool HasEmptySlot(int ownerId)
        {
            return GetFirstEmptyTileCoord(ownerId).y >= 0;
        }

        /// <summary>
        /// Check if we can accept a hero: either bench has space, or buying would trigger an upgrade.
        /// </summary>
        public bool CanAcceptHero(int ownerId, ChampionData heroData, int starLevel = 1)
        {
            if (HasEmptySlot(ownerId)) return true;
            return WouldTriggerUpgrade(ownerId, heroData, starLevel);
        }

        /// <summary>
        /// Check if adding one more hero of this type/star would trigger a 3-to-1 merge.
        /// </summary>
        private bool WouldTriggerUpgrade(int ownerId, ChampionData heroData, int starLevel)
        {
            if (starLevel >= 5) return false;
            return GetMatchingHeroes(ownerId, heroData, starLevel).Count >= 2;
        }

        public Vector2Int GetFirstEmptyTileCoord(int ownerId)
        {
            Arena arena = GetArena(ownerId);
            if (arena == null || arena.BenchArea == null || arena.BenchArea.Data == null) return new Vector2Int(-1, -1);

            // Auto-create entry to avoid KeyNotFoundException if arena was registered late
            if (!_heroesOnArenas.ContainsKey(ownerId))
                _heroesOnArenas[ownerId] = new Dictionary<Vector2Int, ChampionActor>();

            var sortedTiles = arena.BenchArea.Data.ActiveTiles
                .OrderBy(t => t.coordinates.x)
                .ThenBy(t => t.coordinates.y)
                .ToList();

            var heroes = _heroesOnArenas[ownerId];
            foreach (var tile in sortedTiles)
            {
                if (!heroes.TryGetValue(tile.coordinates, out var occupant) || occupant == null)
                {
                    return tile.coordinates;
                }
            }
            return new Vector2Int(-1, -1);
        }

        public void AddHeroToBench(int ownerId, ChampionData heroData, int starLevel = 1)
        {
            Vector2Int coord = GetFirstEmptyTileCoord(ownerId);

            if (coord.y < 0)
            {
                if (TryDirectUpgrade(ownerId, heroData, starLevel))
                    return;

                Debug.LogWarning("Bench is full!");
                return;
            }

            Arena arena = GetArena(ownerId);
            if (arena == null) return;

            Vector3 worldPos = arena.GetBenchWorldPosition(coord);
            GameObject heroObj = Instantiate(heroData.prefab, worldPos, arena.BenchArea.CachedTransform.rotation);
            ChampionActor actor = heroObj.GetComponent<ChampionActor>();

            if (actor != null)
            {
                _heroesOnArenas[ownerId][coord] = actor;
                actor.CurrentBenchCoord = coord;
                actor.SetStarLevel(starLevel);
                actor.Initialize();

                CheckForUpgrades(ownerId, heroData, starLevel);
            }
        }

        /// <summary>
        /// Perform an upgrade when bench is full. The purchased hero is consumed
        /// without being placed — we only need 2 existing copies on board.
        /// </summary>
        private bool TryDirectUpgrade(int ownerId, ChampionData heroData, int starLevel)
        {
            if (starLevel >= 5) return false;

            var allMatching = GetMatchingHeroes(ownerId, heroData, starLevel);

            if (allMatching.Count >= 2)
            {
                MergeHeroes(ownerId, allMatching.Take(2).ToList(), heroData, starLevel + 1);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Find all heroes matching the given HeroData and star level across bench and field.
        /// </summary>
        private List<ChampionActor> GetMatchingHeroes(int ownerId, ChampionData heroData, int starLevel)
        {
            if (heroData == null) return new List<ChampionActor>();

            var heroesOnBench = _heroesOnArenas[ownerId].Values
                .Where(h => h != null && h.CombatActorData != null &&
                            h.CombatActorData.Id == heroData.Id &&
                            h.StarLevel == starLevel)
                .ToList();

            var heroesOnField = new List<ChampionActor>();
            if (_manager.Field != null)
            {
                heroesOnField = _manager.Field.GetAllHeroes()
                    .Where(h => h != null && h.CombatActorData != null &&
                                h.CombatActorData.Id == heroData.Id &&
                                h.StarLevel == starLevel)
                    .ToList(); // Note: This gets ALL heroes from ALL fields, might need to filter by owner
            }

            return heroesOnBench.Concat(heroesOnField).ToList();
        }

        public void ServerCheckForUpgrades(int ownerId, ChampionData heroData, int starLevel)
        {
            if (starLevel >= 5) return;

            var allMatching = GetMatchingHeroes(ownerId, heroData, starLevel);

            if (allMatching.Count >= 3)
            {
                ServerMergeHeroes(ownerId, allMatching.Take(3).ToList(), heroData, starLevel + 1);
            }
        }

        private void ServerMergeHeroes(int ownerId, List<ChampionActor> instances, ChampionData heroData, int newStarLevel)
        {
            Debug.Log($"[Server] Upgrading {heroData.displayName} to {newStarLevel} stars for player {ownerId}!");

            ChampionActor targetHero = instances.FirstOrDefault(h => h.IsOnField);
            if (targetHero == null)
            {
                targetHero = instances
                    .OrderBy(h => h.CurrentBenchCoord.x)
                    .ThenBy(h => h.CurrentBenchCoord.y)
                    .First();
            }

            Vector3 spawnPos = targetHero.IsOnField 
                ? _manager.Field.GetWorldPosition(ownerId, targetHero.CurrentFieldCoord) 
                : GetWorldPosition(ownerId, targetHero.CurrentBenchCoord);

            foreach (var hero in instances)
            {
                if (hero == targetHero) continue;

                UnregisterHero(hero);
                if (_manager.Field != null) _manager.Field.UnregisterHero(hero);

                if (hero.MoveAgent != null)
                {
                    hero.MoveAgent.SetEnable(false);
                    if (hero.MoveAgent is NavMeshMoveAgent navMeshAgent)
                    {
                        navMeshAgent.Cleanup();
                    }
                    hero.MoveAgent = null;
                }
                hero.InterruptAction();
                hero.ForceStop();

                if (FishNet.InstanceFinder.IsServerStarted)
                {
                    FishNet.InstanceFinder.ServerManager.Despawn(hero.gameObject);
                }
                else
                {
                    Destroy(hero.gameObject);
                }
            }

            // Sync level up via RPC on the target hero
            var netSync = targetHero.GetComponent<ChampionNetworkSync>();
            if (netSync != null)
            {
                netSync.RpcSetStarLevel(newStarLevel);
            }
            else
            {
                targetHero.SetStarLevel(newStarLevel);
            }

            if (!string.IsNullOrEmpty(_fxGuid))
            {
                this.Raise(new SpawnFxEvent { id = _fxGuid, position = spawnPos, duration = 1f });
            }

            ServerCheckForUpgrades(ownerId, heroData, newStarLevel);
        }

        private void CheckForUpgrades(int ownerId, ChampionData heroData, int starLevel)
        {
            if (starLevel >= 5) return;

            var allMatching = GetMatchingHeroes(ownerId, heroData, starLevel);

            if (allMatching.Count >= 3)
            {
                MergeHeroes(ownerId, allMatching.Take(3).ToList(), heroData, starLevel + 1);
            }
        }

        private void MergeHeroes(int ownerId, List<ChampionActor> instances, ChampionData heroData, int newStarLevel)
        {
            Debug.Log($"Upgrading {heroData.displayName} to {newStarLevel} stars for player {ownerId}!");

            ChampionActor fieldHero = instances.FirstOrDefault(h => h.IsOnField);
            bool wasOnField = fieldHero != null;

            Vector2Int targetCoord;
            if (wasOnField)
            {
                targetCoord = fieldHero.CurrentFieldCoord;
            }
            else
            {
                targetCoord = instances
                    .Select(h => h.CurrentBenchCoord)
                    .OrderBy(coord => coord.x)
                    .ThenBy(coord => coord.y)
                    .First();
            }

            foreach (var hero in instances)
            {
                UnregisterHero(hero);
                if (_manager.Field != null) _manager.Field.UnregisterHero(hero);

                if (hero.MoveAgent != null)
                {
                    hero.MoveAgent.SetEnable(false);
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

            Vector3 spawnPos = wasOnField ? _manager.Field.GetWorldPosition(ownerId, targetCoord) : GetWorldPosition(ownerId, targetCoord);

            if (wasOnField)
            {
                _manager.Field.AddHeroToField(heroData, targetCoord, newStarLevel, ownerId);
            }
            else
            {
                AddHeroToBenchAtCoord(ownerId, heroData, targetCoord, newStarLevel);
            }

            if (!string.IsNullOrEmpty(_fxGuid))
            {
                this.Raise(new SpawnFxEvent { id = _fxGuid, position = spawnPos, duration = 1f });
            }

            CheckForUpgrades(ownerId, heroData, newStarLevel);
        }

        public void RemoveHeroFromTile(int arenaId, Vector2Int coord)
        {
            if (_heroesOnArenas.TryGetValue(arenaId, out var heroes))
            {
                heroes.Remove(coord);
            }
        }

        public bool TrySnapToBench(Vector3 worldPos, out Vector2Int coord, out int arenaId)
        {
            coord = new Vector2Int(-1, -1);
            arenaId = -1;

            foreach (var arena in _arenas)
            {
                if (arena.TrySnapToBench(worldPos, out coord))
                {
                    arenaId = arena.OwnerID;
                    return true;
                }
            }
            return false;
        }

        public void AddHeroToBenchAtCoord(int ownerId, ChampionData heroData, Vector2Int coord, int starLevel)
        {
            Vector3 worldPos = GetWorldPosition(ownerId, coord);
            GameObject heroObj = Instantiate(heroData.prefab, worldPos, transform.rotation);
            ChampionActor actor = heroObj.GetComponent<ChampionActor>();

            if (actor != null)
            {
                actor.SetStarLevel(starLevel);
                actor.Initialize();
                RegisterHeroToTile(actor, coord, ownerId);
            }
        }

        public void RegisterHeroToTile(ChampionActor actor, Vector2Int coord, int ownerId)
        {
            UnregisterHero(actor);
            if (_manager.Field != null) _manager.Field.UnregisterHero(actor);

            if (!_heroesOnArenas.ContainsKey(ownerId))
                _heroesOnArenas[ownerId] = new Dictionary<Vector2Int, ChampionActor>();

            var heroes = _heroesOnArenas[ownerId];
            if (heroes.ContainsKey(coord))
            {
                Debug.LogWarning($"Tile {coord} on Arena {ownerId} already occupied! Removing stale data.");
                heroes.Remove(coord);
            }

            heroes[coord] = actor;
            actor.CurrentBenchCoord = coord;
            actor.CurrentFieldCoord = new Vector2Int(-1, -1);
        }

        public void UnregisterHero(ChampionActor actor)
        {
            foreach (var ownerId in _heroesOnArenas.Keys.ToList())
            {
                var heroes = _heroesOnArenas[ownerId];
                var keysToRemove = heroes.Where(kvp => kvp.Value == actor || kvp.Value == null).Select(kvp => kvp.Key).ToList();
                foreach (var key in keysToRemove) heroes.Remove(key);
            }
        }

        public ChampionActor GetHeroAtTile(int ownerId, Vector2Int coord)
        {
            if (_heroesOnArenas.TryGetValue(ownerId, out var heroes))
            {
                heroes.TryGetValue(coord, out var actor);
                return actor;
            }
            return null;
        }

        public Vector3 GetWorldPosition(int ownerId, Vector2Int coord)
        {
            Arena arena = GetArena(ownerId);
            return arena != null ? arena.GetBenchWorldPosition(coord) : Vector3.zero;
        }
    }
}

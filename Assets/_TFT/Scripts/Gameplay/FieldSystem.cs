using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

namespace Dajunctic
{
    public class FieldSystem : MonoBehaviour, IGameSystem
    {
        // Scene ref — bound at runtime by FieldAreaBinder in the gameplay scene
        private List<Arena> _arenas = new List<Arena>();
        private Dictionary<int, Dictionary<Vector2Int, ChampionActor>> _heroesOnArenas = new Dictionary<int, Dictionary<Vector2Int, ChampionActor>>();
        private GameSystemManager _manager;

        public async Task LoadDataAsync()
        {
            // FieldSystem has no Addressable data — scene ref bound via FieldAreaBinder
            await Task.CompletedTask;
            Debug.Log("<color=cyan>FieldSystem data loaded (no-op)</color>");
        }

        public void Initialize(GameSystemManager manager)
        {
            _manager = manager;
            Debug.Log("<color=cyan>FieldSystem initialized</color>");
        }

        /// <summary>Called by Arena when it's initialized or by a binder.</summary>
        public void RegisterArena(Arena arena)
        {
            if (!_arenas.Contains(arena)) _arenas.Add(arena);
            if (!_heroesOnArenas.ContainsKey(arena.OwnerID))
                _heroesOnArenas[arena.OwnerID] = new Dictionary<Vector2Int, ChampionActor>();
        }

        public Arena GetArena(int ownerId) => _arenas.Find(a => a.OwnerID == ownerId);

        public void Shutdown()
        {
            foreach (var dict in _heroesOnArenas.Values) dict.Clear();
            _heroesOnArenas.Clear();
            _arenas.Clear();
            Debug.Log("<color=yellow>FieldSystem shutdown</color>");
        }

        public int GetUnitCount(int ownerId) 
        {
            if (_heroesOnArenas.TryGetValue(ownerId, out var heroes)) return heroes.Count;
            return 0;
        }

        public bool CanAddUnit(int ownerId)
        {
            if (_manager.Economy == null) return true;
            int count = GetUnitCount(ownerId);
            bool canAdd = count < _manager.Economy.Level;
            if (!canAdd) Debug.LogWarning($"Field Manager: Limit reached for player {ownerId}. Units: {count}, Level: {_manager.Economy.Level}");
            return canAdd;
        }

        public bool TrySnapToField(Vector3 worldPos, out Vector2Int coord, out int arenaId)
        {
            coord = new Vector2Int(-1, -1);
            arenaId = -1;

            foreach (var arena in _arenas)
            {
                if (arena.TrySnapToField(worldPos, out coord))
                {
                    arenaId = arena.OwnerID;
                    return true;
                }
            }
            return false;
        }

        public void RegisterHeroToTile(ChampionActor actor, Vector2Int coord, int arenaId)
        {
            UnregisterHero(actor);
            if (_manager.Bench != null) _manager.Bench.UnregisterHero(actor);

            if (!_heroesOnArenas.ContainsKey(arenaId))
                _heroesOnArenas[arenaId] = new Dictionary<Vector2Int, ChampionActor>();

            var heroes = _heroesOnArenas[arenaId];
            if (heroes.ContainsKey(coord))
            {
                Debug.LogWarning($"Field tile {coord} on Arena {arenaId} already occupied! Removing stale data.");
                heroes.Remove(coord);
            }

            heroes[coord] = actor;
            actor.CurrentFieldCoord = coord;
            actor.CurrentBenchCoord = new Vector2Int(-1, -1);
            // actor.OwnerID = arenaId; // In case we need to track owner on actor

            _manager.Traits?.RefreshTraits();
        }

        public void RemoveHeroFromTile(int arenaId, Vector2Int coord)
        {
            if (_heroesOnArenas.TryGetValue(arenaId, out var heroes))
            {
                heroes.Remove(coord);
            }
        }

        public void UnregisterHero(ChampionActor actor)
        {
            foreach (var arenaId in _heroesOnArenas.Keys.ToList())
            {
                var heroes = _heroesOnArenas[arenaId];
                var keysToRemove = heroes.Where(kvp => kvp.Value == actor || kvp.Value == null).Select(kvp => kvp.Key).ToList();
                foreach (var key in keysToRemove) heroes.Remove(key);
            }

            _manager.Traits?.RefreshTraits();
        }

        public ChampionActor GetHeroAtTile(int arenaId, Vector2Int coord)
        {
            if (_heroesOnArenas.TryGetValue(arenaId, out var heroes))
            {
                heroes.TryGetValue(coord, out var actor);
                return actor;
            }
            return null;
        }

        public void AddHeroToField(ChampionData heroData, Vector2Int coord, int starLevel, int arenaId)
        {
            Arena arena = GetArena(arenaId);
            if (arena == null) return;

            Vector3 worldPos = arena.GetFieldWorldPosition(coord);
            GameObject heroObj = Instantiate(heroData.prefab, worldPos, arena.FieldArea.CachedTransform.rotation);
            ChampionActor actor = heroObj.GetComponent<ChampionActor>();

            if (actor != null)
            {
                actor.CurrentFieldCoord = coord;
                actor.SetStarLevel(starLevel);
                actor.Initialize();
                RegisterHeroToTile(actor, coord, arenaId);
            }
        }

        public List<ChampionActor> GetAllHeroes()
        {
            return _heroesOnArenas.Values.SelectMany(d => d.Values).ToList();
        }

        public Vector3 GetWorldPosition(int arenaId, Vector2Int coord)
        {
            Arena arena = GetArena(arenaId);
            return arena != null ? arena.GetFieldWorldPosition(coord) : Vector3.zero;
        }
    }
}

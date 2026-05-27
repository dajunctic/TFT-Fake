using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

namespace Dajunctic
{
    public class FieldSystem : MonoBehaviour, IGameSystem
    {
        
        private List<Arena> _arenas = new List<Arena>();
        private Dictionary<int, Dictionary<Vector2Int, ChampionActor>> _heroesOnArenas = new Dictionary<int, Dictionary<Vector2Int, ChampionActor>>();
        private Dictionary<int, Dictionary<Vector2Int, ChampionActor>> _guestHeroesOnArenas = new Dictionary<int, Dictionary<Vector2Int, ChampionActor>>();
        private GameSystemManager _manager;

        public async Task LoadDataAsync()
        {
            
            await Task.CompletedTask;
        }

        public void Initialize(GameSystemManager manager)
        {
            _manager = manager;
        }

        public void RegisterArena(Arena arena)
        {
            if (!_arenas.Contains(arena)) _arenas.Add(arena);
            if (!_heroesOnArenas.ContainsKey(arena.OwnerID))
                _heroesOnArenas[arena.OwnerID] = new Dictionary<Vector2Int, ChampionActor>();
            if (!_guestHeroesOnArenas.ContainsKey(arena.OwnerID))
                _guestHeroesOnArenas[arena.OwnerID] = new Dictionary<Vector2Int, ChampionActor>();
        }

        public Arena GetArena(int ownerId) => _arenas.Find(a => a.OwnerID == ownerId);

        public IReadOnlyList<Arena> GetAllArenas() => _arenas;

        public IEnumerable<ChampionActor> GetHeroesOnField(int ownerId)
        {
            if (_heroesOnArenas.TryGetValue(ownerId, out var heroes))
                return heroes.Values;
            return System.Linq.Enumerable.Empty<ChampionActor>();
        }

        public void Shutdown()
        {
            foreach (var dict in _heroesOnArenas.Values) dict.Clear();
            _heroesOnArenas.Clear();
            foreach (var dict in _guestHeroesOnArenas.Values) dict.Clear();
            _guestHeroesOnArenas.Clear();
            _arenas.Clear();
        }

        public int GetUnitCount(int ownerId) 
        {
            if (_heroesOnArenas.TryGetValue(ownerId, out var heroes)) return heroes.Count;
            return 0;
        }

        public bool CanAddUnit(int ownerId)
        {
            var sync = _manager.Player?.GetPlayerSync(ownerId);
            if (sync == null) return true;

            int count = GetUnitCount(ownerId);
            bool canAdd = count < sync.Level.Value;
            if (!canAdd) Debug.LogWarning($"Field Manager: Limit reached for player {ownerId}. Units: {count}, Level: {sync.Level.Value}");
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

            _manager.Traits?.RefreshTraits();
        }

        public void RemoveHeroFromTile(int arenaId, Vector2Int coord)
        {
            if (_heroesOnArenas.TryGetValue(arenaId, out var heroes))
            {
                heroes.Remove(coord);
            }
        }

        public void RegisterGuestHeroToTile(ChampionActor actor, Vector2Int coord, int arenaId)
        {
            if (actor == null) return;
            if (!_guestHeroesOnArenas.TryGetValue(arenaId, out var heroes))
            {
                Debug.LogWarning($"FieldSystem: Arena {arenaId} not registered for guest tracking.");
                return;
            }
            heroes[coord] = actor;

        }

        public void UnregisterHero(ChampionActor actor)
        {
            foreach (var arenaId in _heroesOnArenas.Keys.ToList())
            {
                var heroes = _heroesOnArenas[arenaId];
                var keysToRemove = heroes.Where(kvp => kvp.Value == actor || kvp.Value == null).Select(kvp => kvp.Key).ToList();
                foreach (var key in keysToRemove) heroes.Remove(key);
            }

            foreach (var arenaId in _guestHeroesOnArenas.Keys.ToList())
            {
                var heroes = _guestHeroesOnArenas[arenaId];
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
                actor.Initialize();
                actor.SetStarLevel(starLevel);
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

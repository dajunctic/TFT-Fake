using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Dajunctic
{
    public class FieldSystem : MonoBehaviour, IGameSystem
    {
        [SerializeField] private HexAreaView fieldArea;
        private Dictionary<Vector2Int, HeroCombatActor> _heroOnTiles = new Dictionary<Vector2Int, HeroCombatActor>();
        private GameSystemManager _manager;

        public void Initialize(GameSystemManager manager)
        {
            _manager = manager;
            Debug.Log("<color=cyan>FieldSystem initialized</color>");
        }

        public void Shutdown()
        {
            _heroOnTiles.Clear();
            Debug.Log("<color=yellow>FieldSystem shutdown</color>");
        }

        public int UnitCount => _heroOnTiles.Count;

        public bool CanAddUnit()
        {
            if (_manager.Economy == null) return true;
            bool canAdd = UnitCount < _manager.Economy.Level;
            if (!canAdd) Debug.LogWarning($"Field Manager: Limit reached. Units: {UnitCount}, Level: {_manager.Economy.Level}");
            return canAdd;
        }

        public bool TrySnapToField(Vector3 worldPos, out Vector2Int coord)
        {
            coord = new Vector2Int(-1, -1);
            if (fieldArea == null || fieldArea.Data == null) return false;

            Vector3 localPos = fieldArea.CachedTransform.InverseTransformPoint(worldPos);
            Vector2Int hexCoords = fieldArea.Data.WorldToHex(localPos, Vector3.zero);

            if (fieldArea.Data.TryGetTile(hexCoords, out _))
            {
                coord = hexCoords;
                return true;
            }
            return false;
        }

        public void RegisterHeroToTile(HeroCombatActor actor, Vector2Int coord)
        {
            UnregisterHero(actor);
            // Cross-zone cleanup: moving to field means leaving bench
            if (_manager.Bench != null) _manager.Bench.UnregisterHero(actor);
            
            // Remove any existing entry at this coord (in case of stale data)
            if (_heroOnTiles.ContainsKey(coord))
            {
                Debug.LogWarning($"Field tile {coord} already has an entry! Removing stale data.");
                _heroOnTiles.Remove(coord);
            }
            
            _heroOnTiles[coord] = actor;
            actor.CurrentFieldCoord = coord;
            actor.CurrentBenchCoord = new Vector2Int(-1, -1);
        }

        public void RemoveHeroFromTile(Vector2Int coord)
        {
            if (_heroOnTiles.ContainsKey(coord))
            {
                _heroOnTiles.Remove(coord);
            }
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

        public HeroCombatActor GetHeroAtTile(Vector2Int coord)
        {
            _heroOnTiles.TryGetValue(coord, out var actor);
            return actor;
        }

        public void AddHeroToField(HeroData heroData, Vector2Int coord, int starLevel)
        {
            Vector3 worldPos = GetWorldPosition(coord);
            GameObject heroObj = Instantiate(heroData.prefab, worldPos, fieldArea.CachedTransform.rotation);
            HeroCombatActor actor = heroObj.GetComponent<HeroCombatActor>();
            
            if (actor != null)
            {
                actor.CurrentFieldCoord = coord;
                actor.SetStarLevel(starLevel);
                actor.Initialize();
                RegisterHeroToTile(actor, coord);
            }
        }

        public List<HeroCombatActor> GetAllHeroes()
        {
            return _heroOnTiles.Values.ToList();
        }

        public Vector3 GetWorldPosition(Vector2Int coord)
        {
            Vector3 localPos = fieldArea.Data.HexToWorld(Vector3.zero, coord);
            return fieldArea.CachedTransform.TransformPoint(localPos);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Dajunctic
{
    public class FieldManager : Singleton<FieldManager>
    {
        [SerializeField] private HexAreaView fieldArea;
        private Dictionary<Vector2Int, HeroCombatActor> _heroOnTiles = new Dictionary<Vector2Int, HeroCombatActor>();

        protected override void Awake()
        {
            base.Awake();
            FindFieldArea();
        }

        private void FindFieldArea()
        {
            if (fieldArea != null) return;
            
            // Try to find the player's area. Usually it's the 'bossArea' in this project structure
            if (GameManager.Instance != null)
                fieldArea = GameManager.Instance.bossArea;
            
            if (fieldArea == null)
            {
                Debug.LogError("FieldManager: Could not find Field Area (HexAreaView)!");
            }
        }

        public int UnitCount => _heroOnTiles.Count;

        public bool CanAddUnit()
        {
            if (EconomyManager.Instance == null) return true;
            bool canAdd = UnitCount < EconomyManager.Instance.Level;
            if (!canAdd) Debug.LogWarning($"Field Manager: Limit reached. Units: {UnitCount}, Level: {EconomyManager.Instance.Level}");
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
            if (BenchManager.Instance != null) BenchManager.Instance.UnregisterHero(actor);
            
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

using System;
using System.Collections.Generic;
using System.Linq;
using Dajunctic;
using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(fileName = "HexAreaData", menuName = "Panthera/HexAreaData")]
    public class HexAreaData: BaseSO
    {
        [SerializeField]
        private List<HexTile> activeTiles = new List<HexTile>();
        public List<HexTile> ActiveTiles => activeTiles;
        public float HexSize = 1f;
        public float InnerRadius => HexSize * 0.866025404f; // sqrt(3)/2
        public bool TryGetTile(Vector2Int coor, out HexTile tile)
        {
            tile = activeTiles.FirstOrDefault(h => h.coordinates == coor);
            return tile != null;
        }
        public void AddTile(Vector2Int coor)
        {
            if (TryGetTile(coor, out var tile)) return;
            activeTiles.Add(new HexTile { coordinates = coor });
        }

        public void RemoveTile(Vector2Int coor)
        {
            if (!TryGetTile(coor, out var tile)) return;
            activeTiles.Remove(tile);
        }

        public void Clear()
        {
            activeTiles.Clear();
        }

        public List<Vector3> GetAllPositions(Vector3 offset)
        {
            var positions = new List<Vector3>();

            foreach (var tile in activeTiles)
            {
                positions.Add(HexToWorld(offset, tile.coordinates));
            }

            return positions;
        }

        public Vector3 HexToWorld(Vector3 position, Vector2Int hex)
        {
            float x = HexSize * 1.5f * hex.x;
            float z = HexSize * Mathf.Sqrt(3f) * (hex.y + hex.x * 0.5f);
            return position + new Vector3(x, 0, z);
        }

    }

    [Serializable]
    public class HexTile
    {
        public Vector2Int coordinates;
    }
}
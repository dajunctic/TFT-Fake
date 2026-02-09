using System;
using System.Collections.Generic;
using System.Linq;
using Dajunctic;
using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(fileName = "SquareAreaData", menuName = "Dajunctic/SquareAreaData")]
    public class SquareAreaData : BaseSO
    {
        [SerializeField]
        private List<SquareTile> activeTiles = new List<SquareTile>();
        public List<SquareTile> ActiveTiles => activeTiles;
        public float SquareSize = 1f;
        public float Spacing = 0.1f;

        public bool TryGetTile(Vector2Int coor, out SquareTile tile)
        {
            tile = activeTiles.FirstOrDefault(h => h.coordinates == coor);
            return tile != null;
        }

        public void AddTile(Vector2Int coor)
        {
            if (TryGetTile(coor, out var tile)) return;
            activeTiles.Add(new SquareTile { coordinates = coor });
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
                positions.Add(SquareToWorld(offset, tile.coordinates));
            }

            return positions;
        }

        public Vector3 SquareToWorld(Vector3 position, Vector2Int square)
        {
            float step = SquareSize + Spacing;
            float x = square.x * step;
            float z = square.y * step;
            return position + new Vector3(x, 0, z);
        }

        public Vector2Int WorldToSquare(Vector3 worldPos, Vector3 areaPosition)
        {
            Vector3 localPos = worldPos - areaPosition;
            float step = SquareSize + Spacing;

            int x = Mathf.RoundToInt(localPos.x / step);
            int y = Mathf.RoundToInt(localPos.z / step);

            return new Vector2Int(x, y);
        }
    }

    [Serializable]
    public class SquareTile
    {
        public Vector2Int coordinates;
    }
}

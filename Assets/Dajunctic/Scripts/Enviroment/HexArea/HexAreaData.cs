using System;
using System.Collections.Generic;
using System.Linq;
using Dajunctic;
using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(fileName = "HexAreaData", menuName = "Dajunctic/HexAreaData")]
    public class HexAreaData: BaseSO
    {
        [SerializeField]
        private List<HexTile> activeTiles = new List<HexTile>();
        public List<HexTile> ActiveTiles => activeTiles;
        public float HexSize = 1f;
        public float Spacing = 0f;
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
            float effSize = HexSize + Spacing;
            float z = effSize * 1.5f * hex.y;
            float x = effSize * Mathf.Sqrt(3f) * (hex.x + hex.y * 0.5f);
            return position + new Vector3(x, 0, z);
        }

        public Vector2Int WorldToHex(Vector3 worldPos, Vector3 areaPosition)
        {
            Vector3 localPos = worldPos - areaPosition;
            float effSize = HexSize + Spacing;

            float q = (Mathf.Sqrt(3f) / 3f * localPos.x - 1f / 3f * localPos.z) / effSize;
            float r = (2f / 3f * localPos.z) / effSize;

            return HexRound(q, r);
        }

        private Vector2Int HexRound(float q, float r)
        {
            float s = -q - r;
            int rq = Mathf.RoundToInt(q);
            int rr = Mathf.RoundToInt(r);
            int rs = Mathf.RoundToInt(s);

            float q_diff = Mathf.Abs(rq - q);
            float r_diff = Mathf.Abs(rr - r);
            float s_diff = Mathf.Abs(rs - s);

            if (q_diff > r_diff && q_diff > s_diff) rq = -rr - rs;
            else if (r_diff > s_diff) rr = -rq - rs;

            return new Vector2Int(rq, rr);
        }

    }

    [Serializable]
    public class HexTile
    {
        public Vector2Int coordinates;
    }
}
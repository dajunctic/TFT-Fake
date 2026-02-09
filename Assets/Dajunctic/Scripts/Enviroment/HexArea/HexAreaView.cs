using Dajunctic;
using UnityEngine;

namespace Dajunctic
{
    public class HexAreaView: BaseView, IDragTarget
    {
        public HexAreaData Data;
        private Color NormalColor = Color.cyan;

        public bool TryGetSnapPosition(Vector3 worldPos, out Vector3 snappedPos)
        {
            snappedPos = worldPos;
            if (Data == null) return false;

            Vector2Int hexCoords = Data.WorldToHex(worldPos, CachedTransform.position);
            if (Data.TryGetTile(hexCoords, out _))
            {
                snappedPos = Data.HexToWorld(CachedTransform.position, hexCoords);
                return true;
            }
            return false;
        }

        public Vector3 GetRandomPosition()
        {
            var allPositions = Data.GetAllPositions(CachedTransform.position);
            return allPositions.GetRandom();
        }

        private void OnDrawGizmos()
        {
            if (Data == null) return;

            foreach (var hex in Data.ActiveTiles)
            {
                Vector3 worldPos = Data.HexToWorld(CachedTransform.position, hex.coordinates);
                DrawHexagon(worldPos, Data.HexSize, NormalColor);
            }
        }

        private void DrawHexagon(Vector3 center, float size, Color color)
        {
            Gizmos.color = color;
            Vector3[] corners = new Vector3[6];
            for (int i = 0; i < 6; i++)
            {
                float angle_deg = 60 * i -30;
                float angle_rad = Mathf.PI / 180 * angle_deg;
                corners[i] = center + new Vector3(Mathf.Cos(angle_rad) * size, 0, Mathf.Sin(angle_rad) * size);
            }

            for (int i = 0; i < 6; i++)
            {
                Gizmos.DrawLine(corners[i], corners[(i + 1) % 6]);
            }
        }
    }
}
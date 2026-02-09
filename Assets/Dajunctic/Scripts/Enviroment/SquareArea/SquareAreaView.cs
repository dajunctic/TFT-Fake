using Dajunctic;
using UnityEngine;

namespace Dajunctic
{
    public class SquareAreaView : BaseView, IDragTarget
    {
        public SquareAreaData Data;
        private Color NormalColor = Color.cyan;

        public bool TryGetSnapPosition(Vector3 worldPos, out Vector3 snappedPos)
        {
            snappedPos = worldPos;
            if (Data == null) return false;

            Vector2Int squareCoords = Data.WorldToSquare(worldPos, CachedTransform.position);
            if (Data.TryGetTile(squareCoords, out _))
            {
                snappedPos = Data.SquareToWorld(CachedTransform.position, squareCoords);
                return true;
            }
            return false;
        }

        public Vector3 GetRandomPosition()
        {
            if (Data == null || Data.ActiveTiles.Count == 0) return CachedTransform.position;
            var allPositions = Data.GetAllPositions(CachedTransform.position);
            return allPositions.GetRandom();
        }

        private void OnDrawGizmos()
        {
            if (Data == null) return;

            foreach (var square in Data.ActiveTiles)
            {
                Vector3 worldPos = Data.SquareToWorld(CachedTransform.position, square.coordinates);
                DrawSquare(worldPos, Data.SquareSize, NormalColor);
            }
        }

        private void DrawSquare(Vector3 center, float size, Color color)
        {
            Gizmos.color = color;
            float halfSize = size * 0.5f;
            
            Vector3 topLeft = center + new Vector3(-halfSize, 0, halfSize);
            Vector3 topRight = center + new Vector3(halfSize, 0, halfSize);
            Vector3 bottomLeft = center + new Vector3(-halfSize, 0, -halfSize);
            Vector3 bottomRight = center + new Vector3(halfSize, 0, -halfSize);

            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
        }
    }
}

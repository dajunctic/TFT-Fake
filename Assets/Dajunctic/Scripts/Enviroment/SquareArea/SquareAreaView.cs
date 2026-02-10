using System.Collections.Generic;
using Dajunctic;
using UnityEngine;

namespace Dajunctic
{
    public class SquareAreaView : BaseView, IDragTarget
    {
        public SquareAreaData Data;
        [SerializeField] private SquareTileView tilePrefab;
        private List<SquareTileView> _spawnedTiles = new List<SquareTileView>();
        private Dictionary<Vector2Int, SquareTileView> _tilesMap = new Dictionary<Vector2Int, SquareTileView>();
        private SquareTileView _lastHighlighted;
        
        private Color NormalColor = Color.cyan;

        public override void Initialize()
        {
            base.Initialize();
            SpawnTiles();
            SetTilesVisible(false);
        }

        [ContextMenu("Spawn Tiles")]
        public void SpawnTiles()
        {
            foreach (var tile in _spawnedTiles)
            {
                if (tile != null)
                {
                    if (Application.isPlaying) Destroy(tile.gameObject);
                    else DestroyImmediate(tile.gameObject);
                }
            }
            _spawnedTiles.Clear();
            _tilesMap.Clear();

            // Clear potential stray children if editor-only
            if (!Application.isPlaying)
            {
                var children = new List<GameObject>();
                foreach (Transform child in CachedTransform) children.Add(child.gameObject);
                children.ForEach(DestroyImmediate);
            }

            if (Data == null || tilePrefab == null) return;

            foreach (var square in Data.ActiveTiles)
            {
                Vector3 localPos = Data.SquareToWorld(Vector3.zero, square.coordinates);
                Vector3 worldPos = CachedTransform.TransformPoint(localPos);
                var tileInstance = Instantiate(tilePrefab, worldPos, CachedTransform.rotation, CachedTransform);
                tileInstance.name = $"SquareTile_{square.coordinates.x}_{square.coordinates.y}";
                _spawnedTiles.Add(tileInstance);
                _tilesMap.Add(square.coordinates, tileInstance);

                tileInstance.Initialize();
            }
        }

        public void OnDragStart()
        {
            SetTilesVisible(true);
        }

        public void OnDragEnd()
        {
            SetTilesVisible(false);
            if (_lastHighlighted != null) _lastHighlighted.SetHighlight(false);
            _lastHighlighted = null;
        }

        private void SetTilesVisible(bool visible)
        {
            foreach (var tile in _spawnedTiles) tile.SetVisible(visible);
        }

        public bool TryGetSnapPosition(Vector3 worldPos, out Vector3 snappedPos)
        {
            snappedPos = worldPos;
            if (Data == null) return false;

            if (_lastHighlighted != null)
            {
                _lastHighlighted.SetHighlight(false);
                _lastHighlighted = null;
            }

            // Convert world position to local position of this area
            Vector3 localPos = CachedTransform.InverseTransformPoint(worldPos);
            
            // Calculate coordinates using local position (offset is effectively zero now)
            Vector2Int squareCoords = Data.WorldToSquare(localPos, Vector3.zero);
            
            if (Data.TryGetTile(squareCoords, out _))
            {
                // Get local snapped position and convert back to world
                Vector3 localSnapped = Data.SquareToWorld(Vector3.zero, squareCoords);
                snappedPos = CachedTransform.TransformPoint(localSnapped);

                if (_tilesMap.TryGetValue(squareCoords, out var tileView))
                {
                    tileView.SetHighlight(true);
                    _lastHighlighted = tileView;
                }

                return true;
            }
            return false;
        }

        public Vector3 GetRandomPosition()
        {
            if (Data == null || Data.ActiveTiles.Count == 0) return CachedTransform.position;
            var allPositions = Data.GetAllPositions(Vector3.zero);
            var randomLocal = allPositions.GetRandom();
            return CachedTransform.TransformPoint(randomLocal);
        }

        private void OnDrawGizmos()
        {
            if (Data == null) return;

            foreach (var square in Data.ActiveTiles)
            {
                Vector3 localPos = Data.SquareToWorld(Vector3.zero, square.coordinates);
                Vector3 worldPos = CachedTransform.TransformPoint(localPos);
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

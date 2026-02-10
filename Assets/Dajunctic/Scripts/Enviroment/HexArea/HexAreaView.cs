using System.Collections.Generic;
using Dajunctic;
using UnityEngine;

namespace Dajunctic
{
    public class HexAreaView: BaseView, IDragTarget
    {
        public HexAreaData Data;
        [SerializeField] private HexTileView tilePrefab;
        private List<HexTileView> _spawnedTiles = new List<HexTileView>();
        private Dictionary<Vector2Int, HexTileView> _tilesMap = new Dictionary<Vector2Int, HexTileView>();
        private HexTileView _lastHighlighted;

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

            foreach (var hex in Data.ActiveTiles)
            {
                Vector3 localPos = Data.HexToWorld(Vector3.zero, hex.coordinates);
                Vector3 worldPos = CachedTransform.TransformPoint(localPos);
                var tileInstance = Instantiate(tilePrefab, worldPos, CachedTransform.rotation, CachedTransform);
                tileInstance.name = $"HexTile_{hex.coordinates.x}_{hex.coordinates.y}";
                _spawnedTiles.Add(tileInstance);
                _tilesMap.Add(hex.coordinates, tileInstance);
                
                // If it's a BaseView and needs initialization
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

            // Use local coordinates to handle rotation and scale
            Vector3 localPos = CachedTransform.InverseTransformPoint(worldPos);
            Vector2Int hexCoords = Data.WorldToHex(localPos, Vector3.zero);

            if (Data.TryGetTile(hexCoords, out _))
            {
                Vector3 localSnapped = Data.HexToWorld(Vector3.zero, hexCoords);
                snappedPos = CachedTransform.TransformPoint(localSnapped);

                if (_tilesMap.TryGetValue(hexCoords, out var tileView))
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
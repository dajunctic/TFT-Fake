using System.Collections.Generic;
using Dajunctic;
using UnityEngine;

namespace Dajunctic
{
    /// <summary>Distinguishes the purpose of a HexAreaView for colour-coding tiles.</summary>
    public enum HexAreaType
    {
        /// <summary>Bench — square area, normally not coloured differently.</summary>
        None,
        /// <summary>The local player's own field rows (front of arena).</summary>
        OwnField,
        /// <summary>The opponent's field rows (back of arena).</summary>
        GuestField,
    }

    public class HexAreaView: BaseView, IDragTarget
    {
        public HexAreaData Data;
        [SerializeField] private HexTileView tilePrefab;
        private List<HexTileView> _spawnedTiles = new List<HexTileView>();
        private Dictionary<Vector2Int, HexTileView> _tilesMap = new Dictionary<Vector2Int, HexTileView>();
        private HexTileView _lastHighlighted;

        [Header("Area Type")]
        [Tooltip("Set to OwnField or GuestField to colour-code tiles by spawn area.")]
        [SerializeField] private HexAreaType areaType = HexAreaType.None;


        [Header("Area Colours")]
        [ColorUsage(true, true)]
        [SerializeField] private Color ownFieldColor  = new Color(0.0f, 0.6f, 1.0f, 1f);   // blue-cyan
        [ColorUsage(true, true)]
        [SerializeField] private Color guestFieldColor = new Color(1.0f, 0.25f, 0.1f, 1f); // red-orange

        private Color NormalColor = Color.cyan;

        private Color GetAreaColor()
        {
            return areaType switch
            {
                HexAreaType.OwnField   => ownFieldColor,
                HexAreaType.GuestField => guestFieldColor,
                _                      => NormalColor,
            };
        }

        public override void Initialize()
        {
            base.Initialize();
            SpawnTiles();
            SetTilesVisible(false);

            // Đăng ký vào DragManager để tiles hiện ra khi drag (dù arena spawn sau DragManager.Awake)
            DragManager.Register(this);
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
                
                // Initialize first so _originalEmissionColor is captured
                tileInstance.Initialize();

                // Then apply area-type colour so tiles are tinted correctly at runtime
                if (areaType != HexAreaType.None)
                    tileInstance.SetBaseColor(GetAreaColor());
            }
        }

        public void OnDragStart()
        {
            // GuestField không được highlight khi kéo thả tướng — chỉ FieldArea của người chơi mới highlight
            if (areaType == HexAreaType.GuestField) return;
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

            Color gizmoColor = GetAreaColor();
            foreach (var hex in Data.ActiveTiles)
            {
                Vector3 localPos = Data.HexToWorld(Vector3.zero, hex.coordinates);
                Vector3 worldPos = transform.TransformPoint(localPos);
                DrawHexagon(worldPos, Data.HexSize, gizmoColor);
            }
        }

        private void DrawHexagon(Vector3 center, float size, Color color)
        {
            Gizmos.color = color;
            Vector3[] corners = new Vector3[6];
            for (int i = 0; i < 6; i++)
            {
                float angle_deg = 60 * i - 30;
                float angle_rad = Mathf.PI / 180 * angle_deg;
                Vector3 localCorner = new Vector3(Mathf.Cos(angle_rad) * size, 0, Mathf.Sin(angle_rad) * size);
                corners[i] = center + transform.TransformDirection(localCorner);
            }

            for (int i = 0; i < 6; i++)
            {
                Gizmos.DrawLine(corners[i], corners[(i + 1) % 6]);
            }
        }
        private void OnDestroy()
        {
            DragManager.Unregister(this);
        }
    }
}
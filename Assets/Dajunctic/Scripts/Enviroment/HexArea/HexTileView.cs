using UnityEngine;

namespace Dajunctic

{
    public class HexTileView: BaseView
    {
        [SerializeField] private MeshRenderer meshRenderer;
        
        [Header("Highlight Settings")]
        [ColorUsage(true, true)]
        [SerializeField] private Color highlightColor = Color.yellow * 2.5f;
        [SerializeField] private float highlightGlow = 12f;

        private Color _originalEmissionColor;
        private Color _baseEmissionColor;   // overridden by SetBaseColor (area type tint)
        private float _originalGlow;
        
        private static readonly int EmissionColorKey = Shader.PropertyToID("_EmissionColor");
        private static readonly int EmissionGlowKey = Shader.PropertyToID("_EmissionSelfGlow");

        public override void Initialize()
        {
            base.Initialize();
            if (meshRenderer != null)
            {
                _originalEmissionColor = meshRenderer.material.GetColor(EmissionColorKey);
                _baseEmissionColor = _originalEmissionColor;
                _originalGlow = meshRenderer.material.GetFloat(EmissionGlowKey);
            }
        }

        /// <summary>
        /// Sets a persistent base emission color for this tile (e.g. to distinguish
        /// own field vs guest/opponent field). Call after Initialize().
        /// </summary>
        public void SetBaseColor(Color color)
        {
            _baseEmissionColor = color;
            if (meshRenderer != null)
                meshRenderer.material.SetColor(EmissionColorKey, _baseEmissionColor);
        }

        public void SetHighlight(bool highlight)
        {
            if (meshRenderer == null) return;
            
            // Toggle between highlight color and the area-type base color (not the original prefab color)
            meshRenderer.material.SetColor(EmissionColorKey, highlight ? highlightColor : _baseEmissionColor);
            meshRenderer.material.SetFloat(EmissionGlowKey, highlight ? highlightGlow : _originalGlow);
        }

        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}
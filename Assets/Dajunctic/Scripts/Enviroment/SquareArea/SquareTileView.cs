using UnityEngine;

namespace Dajunctic
{
    public class SquareTileView : BaseView
    {
        [SerializeField] private MeshRenderer meshRenderer;
        
        [Header("Highlight Settings")]
        [ColorUsage(true, true)]
        [SerializeField] private Color highlightColor = Color.yellow * 2.5f;
        [SerializeField] private float highlightGlow = 12f;

        private Color _originalEmissionColor;
        private float _originalGlow;
        
        private static readonly int EmissionColorKey = Shader.PropertyToID("_EmissionColor");
        private static readonly int EmissionGlowKey = Shader.PropertyToID("_EmissionSelfGlow");

        public override void Initialize()
        {
            base.Initialize();
            if (meshRenderer != null)
            {
                _originalEmissionColor = meshRenderer.material.GetColor(EmissionColorKey);
                _originalGlow = meshRenderer.material.GetFloat(EmissionGlowKey);
            }
        }

        public void SetHighlight(bool highlight)
        {
            if (meshRenderer == null) return;
            
            // Toggle between highlight settings and original material settings
            meshRenderer.material.SetColor(EmissionColorKey, highlight ? highlightColor : _originalEmissionColor);
            meshRenderer.material.SetFloat(EmissionGlowKey, highlight ? highlightGlow : _originalGlow);
        }

        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}

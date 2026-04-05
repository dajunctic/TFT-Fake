using DTT.PublishingTools;
using UnityEditor;
using UnityEngine;

namespace DTT.AreaOfEffectRegions.Editor
{
    /// <summary>
    /// Draws a custom inspector for the circle region.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SRPCircleRegionProjector))]
    [DTTHeader("dtt.area-of-effect-regions", "SRP Circle Region Projector")]
    internal class SRPCircleRegionProjectorEditor : SRPInspector
    {
        /// <summary>
        /// Draws the inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
#if UNIVERSAL_PACKAGE
            if (GUILayout.Button("Generate Projector"))
            {
                DisplayAddProjector();
            }
#endif
            DrawDefaultInspector();
        }
#if UNIVERSAL_PACKAGE    
        /// <summary>
        /// Generate SRP projectors.
        /// </summary>
        private void DisplayAddProjector()
        {
            SRPCircleRegionProjector _target = (SRPCircleRegionProjector)target;
            _target.GenerateProjector();
        }
#endif
    }
}
using DTT.PublishingTools;
using UnityEditor;
using UnityEngine;

namespace DTT.AreaOfEffectRegions.Editor
{
    /// <summary>
    /// Custom inspector for the line region projector.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SRPLineRegionProjector))]
    [DTTHeader("dtt.area-of-effect-regions", "SRP Line Region Projector")]
    internal class SRPLineRegionProjectorEditor : SRPInspector
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
           SRPLineRegionProjector _target = (SRPLineRegionProjector)target;
           _target.GenerateProjector();

        }
#endif
    }
}
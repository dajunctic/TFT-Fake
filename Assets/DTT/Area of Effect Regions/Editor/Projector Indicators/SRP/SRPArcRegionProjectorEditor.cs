using Codice.CM.Common;
using DTT.AreaOfEffectRegions.Editor;
using DTT.PublishingTools;
using DTT.Utils.EditorUtilities;
using UnityEditor;
using UnityEngine;

namespace DTT.AreaOfEffectRegions.Editor
{
    /// <summary>
    /// Draws the custom editor for the arc region projector component.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SRPArcRegionProjector))]
    [DTTHeader("dtt.area-of-effect-regions", "SRP Arc Region Projector")]
    internal class SRPArcRegionProjectorEditor : SRPInspector
    {
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
        /// Generate SRP Projectors.
        /// </summary>
        private void DisplayAddProjector()
        {
            SRPArcRegionProjector _target = (SRPArcRegionProjector)target;
            _target.GenerateProjector();

        }
#endif
    }
}
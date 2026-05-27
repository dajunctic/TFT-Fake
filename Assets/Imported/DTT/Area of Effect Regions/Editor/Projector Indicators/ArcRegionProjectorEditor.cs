using DTT.AreaOfEffectRegions.Editor;
using DTT.PublishingTools;
using DTT.Utils.EditorUtilities;
using UnityEditor;

namespace DTT.AreaOfEffectRegions.Editor
{
    /// <summary>
    /// Draws the custom editor for the arc region projector component.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ArcRegionProjector))]
    [DTTHeader("dtt.area-of-effect-regions", "Arc Region Projector")]
    public class ArcRegionProjectorEditor : DTTInspector
    {
        /// <summary>
        /// Draws all the properties.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawDefaultInspector();
        }
    }
}
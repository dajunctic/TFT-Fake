using DTT.PublishingTools;
using UnityEditor;

namespace DTT.AreaOfEffectRegions.Editor
{
    /// <summary>
    /// Custom inspector for the line region projector.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LineRegionProjector))]
    [DTTHeader("dtt.area-of-effect-regions", "Line Region Projector")]
    internal class LineRegionProjectorEditor : DTTInspector
    {
        /// <summary>
        /// Draws the inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawDefaultInspector();
        }
    }
}
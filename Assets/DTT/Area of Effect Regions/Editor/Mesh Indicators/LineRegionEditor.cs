using DTT.PublishingTools;
using UnityEditor;

namespace DTT.AreaOfEffectRegions.Editor
{
    /// <summary>
    /// Custom inspector for the line region.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LineRegion))]
    [DTTHeader("dtt.area-of-effect-regions", "Line Region")]
    internal class LineRegionEditor : DTTInspector
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
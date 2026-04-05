using DTT.PublishingTools;
using UnityEditor;

namespace DTT.AreaOfEffectRegions.Editor
{
    /// <summary>
    /// Draws a custom inspector for the circle region.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CircleRegion))]
    [DTTHeader("dtt.area-of-effect-regions", "Circle Region")]
    internal class CircleRegionEditor : DTTInspector
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
using DTT.PublishingTools;
using UnityEditor;

namespace DTT.AreaOfEffectRegions.Editor
{
    /// <summary>
    /// Draws a custom inspector for the circle region.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CircleRegionProjector))]
    [DTTHeader("dtt.area-of-effect-regions", "Circle Region Projector")]
    internal class CircleRegionProjectorEditor : DTTInspector
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
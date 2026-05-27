using DTT.PublishingTools;
using DTT.Utils.EditorUtilities;
using UnityEditor;

namespace DTT.AreaOfEffectRegions.Editor
{
    /// <summary>
    /// Draws the custom editor for the arc region component.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ArcRegion))]
    [DTTHeader("dtt.area-of-effect-regions", "Arc Region")]
    internal class ArcRegionEditor : DTTInspector
    {
        /// <summary>
        /// Caches the properties of the target object.
        /// </summary>
        private ArcRegionPropertyCache _propertyCache;

        /// <summary>
        /// Gets everything ready.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            _propertyCache = new ArcRegionPropertyCache(serializedObject);
        }

        /// <summary>
        /// Draws all the properties.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawDefaultInspector();
        }
    }

    /// <summary>
    /// Caches all the ArcRegion properties.
    /// </summary>
    internal class ArcRegionPropertyCache : SerializedPropertyCache
    {
        public SerializedProperty ArcProperty => base["_arc"];
        public SerializedProperty AngleProperty => base["_angle"];
        public SerializedProperty RadiusProperty => base["_radius"];
        public SerializedProperty CentreDotProperty => base["_centreDot"];
        public SerializedProperty LeftSideProperty => base["_leftSide"];
        public SerializedProperty RightSideProperty => base["_rightSide"];
        public SerializedProperty CentreProperty => base["_centre"];
        public SerializedProperty ProgressProperty => base["_progress"];
        public SerializedProperty MaskedCentresProperty => base["_maskedCentres"];
        
        public ArcRegionPropertyCache(SerializedObject serializedObject) : base(serializedObject)
        {
        }
    }
}
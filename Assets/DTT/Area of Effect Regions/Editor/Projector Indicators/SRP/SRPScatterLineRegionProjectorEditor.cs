using UnityEngine;
using UnityEditor;
using DTT.PublishingTools;
using System.Linq;

namespace DTT.AreaOfEffectRegions.Editor
{
    /// <summary>
    /// Draws a custom inspector for the line region projector editor.
    /// </summary>
    [CustomEditor(typeof(SRPScatterLineRegionProjector))]
    [CanEditMultipleObjects]
    [DTTHeader("dtt.area-of-effect-regions", "SRP Line Region Projector")]
    internal class SRPScatterLineRegionProjectorEditor : SRPInspector
    {
#if UNIVERSAL_PACKAGE
        /// <summary>
        /// The targets.
        /// </summary>
        private SRPScatterLineRegionProjector[] _scatterLineRegions;

        /// <summary>
        /// Gets references.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            _scatterLineRegions = targets.Cast<SRPScatterLineRegionProjector>().ToArray();
        }

        /// <summary>
        /// Draws the custom editor.
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

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Add");
            if (GUILayout.Button("+"))
            {
                for (int i = 0; i < _scatterLineRegions.Length; i++)
                    _scatterLineRegions[i].Add(1);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUI.BeginDisabledGroup(_scatterLineRegions.All(region => region.NumberOfLines <= 1));
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Remove");
            if (GUILayout.Button("-"))
                for (int i = 0; i < _scatterLineRegions.Length; i++)
                    _scatterLineRegions[i].Remove(1);
            EditorGUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();
        }
        
        /// <summary>
        /// Generate SRP projectors.
        /// </summary>
        private void DisplayAddProjector()
        {
            SRPScatterLineRegionProjector _target = (SRPScatterLineRegionProjector)target;
            _target.GenerateProjector();
        }
#endif
    }
}
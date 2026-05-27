using UnityEngine;
using UnityEditor;
using DTT.PublishingTools;
using System.Linq;

namespace DTT.AreaOfEffectRegions.Editor
{
    /// <summary>
    /// Draws a custom inspector for the line region projector editor.
    /// </summary>
    [CustomEditor(typeof(ScatterLineRegionProjector))]
    [CanEditMultipleObjects]
    [DTTHeader("dtt.area-of-effect-regions", "Scatter Line Region Projector")]
    public class ScatterLineRegionProjectorEditor : DTTInspector
    {
        /// <summary>
        /// The targets.
        /// </summary>
        private ScatterLineRegionProjector[] _scatterLineRegions;

        /// <summary>
        /// Gets references.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            _scatterLineRegions = targets.Cast<ScatterLineRegionProjector>().ToArray();
        }

        /// <summary>
        /// Draws the custom editor.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawDefaultInspector();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Add");
            if (GUILayout.Button("+"))
            {
                for (int i = 0; i < _scatterLineRegions.Length; i++)
                {
                    _scatterLineRegions[i].Add(1);
                }
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
    }
}
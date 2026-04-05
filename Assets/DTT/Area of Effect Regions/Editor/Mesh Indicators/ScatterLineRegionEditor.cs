using System.Linq;
using DTT.PublishingTools;
using UnityEditor;
using UnityEngine;

namespace DTT.AreaOfEffectRegions.Editor
{
    /// <summary>
    /// Draws a custom inspector for the line region editor.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ScatterLineRegion))]
    [DTTHeader("dtt.area-of-effect-regions", "Scatter Line Region")]
    internal class ScatterLineRegionEditor : DTTInspector
    {
        /// <summary>
        /// The targets.
        /// </summary>
        private ScatterLineRegion[] _scatterLineRegions;
        
        /// <summary>
        /// Gets references.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            _scatterLineRegions = targets.Cast<ScatterLineRegion>().ToArray();
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
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Remove");
            if (GUILayout.Button("-"))
            {
                for (int i = 0; i < _scatterLineRegions.Length; i++)
                {
                    _scatterLineRegions[i].Remove(1);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
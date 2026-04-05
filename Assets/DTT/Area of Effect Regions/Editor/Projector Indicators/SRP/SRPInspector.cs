using DTT.PublishingTools;
using DTT.Utils.EditorUtilities;
using UnityEditor;
using UnityEngine;

namespace DTT.AreaOfEffectRegions.Editor
{
    /// <summary>
    /// Draws the custom editor for the arc region projector component.
    /// </summary>
    internal class SRPInspector : DTTInspector
    {
        /// <summary>
        /// Draws all the properties.
        /// </summary>
        public override void OnInspectorGUI()
        {
#if !UNIVERSAL_PACKAGE
            DisplayUniversalError();
#endif
            base.OnInspectorGUI();
#if UNIVERSAL_PACKAGE
            if (GUILayout.Button("Generate ALL Projectors"))
            {
                GenerateAllProjector();
            }
#endif
        }

#if !UNIVERSAL_PACKAGE
        /// <summary>
        /// Display missing Universal package error.
        /// </summary>
        private void DisplayUniversalError()
        {
            GUILayout.Space(4);
            GUILayout.BeginVertical(EditorStyles.helpBox);
            
            EditorGUILayout.HelpBox(Constants.UNIVERSAL_PLUGIN_ERROR,MessageType.Error);
            if(GUIDrawTools.LinkLabel(new GUIContent("Get the package")))
                Application.OpenURL(Constants.UNIVERSAL_PACKAGE_URL);
            GUILayout.Space(2);
            
            if(GUILayout.Button("Search again"))
                PackageRefresh();
            GUILayout.EndVertical();
            GUILayout.Space(2);
        }

        /// <summary>
        /// Refresh the package search.
        /// </summary>
        private void PackageRefresh()
        {
            bool isFound = UniversalDependencyCheckup.AreUniversalFound();
            UniversalDependencyCheckup.ModifyDefineSymbols(isFound);
        }
#endif
#if UNIVERSAL_PACKAGE
        /// <summary>
        /// Generate all the SRP projectors and camera.
        /// </summary>
        private void GenerateAllProjector()
        {
            // Add the main camera to the scene.
            if (Camera.main == null)
            {
                GameObject cam = new GameObject();
                cam.AddComponent<Camera>();
                cam.transform.localPosition = new Vector3(50, 8, -13);
                cam.transform.Rotate(new Vector3(45,0,0));
                cam.tag = "MainCamera";
            }
            SRPLineRegionProjector[] srpLineRegionProjectors = FindObjectsOfType<SRPLineRegionProjector>();
            SRPCircleRegionProjector[] srpCircleRegionProjectors = FindObjectsOfType<SRPCircleRegionProjector>();
            SRPArcRegionProjector[] srpArcRegionProjectors = FindObjectsOfType<SRPArcRegionProjector>();

            foreach (var srpArcRegionProjector in srpArcRegionProjectors)
            {
                srpArcRegionProjector.GenerateProjector();
            }

            foreach (var SrpCircleRegionProjector in srpCircleRegionProjectors)
            {
                SrpCircleRegionProjector.GenerateProjector();
            }

            foreach (var srpLineRegionProjector in srpLineRegionProjectors)
            {
                srpLineRegionProjector.GenerateProjector();
            }
        }
#endif
    }
}
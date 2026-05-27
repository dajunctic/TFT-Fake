#if UNITY_EDITOR

using DTT.Utils.Workflow;
using System.IO;
using DTT.PublishingTools.Utils;
using UnityEditor;

namespace DTT.PublishingTools
{
    /// <summary>
    /// A static class initialized on load that opens the ReadMe Window 
    /// the first time someone imports this package.
    /// </summary>
    internal class DTTReadMeFocusPostprocessor : AssetPostprocessor
    {
        #region Methods
        #region Private

        /// <summary>
        /// Removes deleted packages hun readme focus key and 
        /// focuses readme's of packages that are imported if
        /// possible.
        /// </summary>
        /// <param name="importedAssets">Imported assets.</param>
        /// <param name="deletedAssets">Deleted assets.</param>
        /// <param name="movedAssets">Moved assets.</param>
        /// <param name="movedFromAssetPaths">Assets moved form asset path.</param>
        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            for (int i = 0; i < importedAssets.Length; i++)
            {
                string assetPath = importedAssets[i];
                if (DTTPathUtility.IsAssetJson(assetPath))
                    TryFocusReadMe(assetPath);
            }
        }

        /// <summary>
        /// Focuses the <see cref="DTTReadMe"/> asset if it can.
        /// </summary>
        private static void TryFocusReadMe(string assetJsonPath)
        {
            AssetJson assetJson = GetAssetJson(assetJsonPath);
            string displayName = assetJson.displayName;
            if (displayName == null || !DTTEditorConfig.HasReadMeSections(assetJson))
                return;

            string focusKey = DTTEditorConfig.GetReadMeFocusKey(assetJson.displayName);
            if (!EditorPrefs.GetBool(focusKey))
            {
                DTTReadMeEditorWindow.Open(assetJson);

                EditorPrefs.SetBool(focusKey, true);
            }
        }

        /// <summary>
        /// Returns the asset json at given asset json path.
        /// </summary>
        /// <param name="assetJsonPath">The asset json path.</param>
        /// <returns>The asset json instance.</returns>
        private static AssetJson GetAssetJson(string assetJsonPath)
        {
            string json = File.ReadAllText(assetJsonPath);
            AssetJson assetJson = new AssetJson();
            EditorJsonUtility.FromJsonOverwrite(json, assetJson);

            return assetJson;
        }
        #endregion
        #endregion
    }
}

#endif
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DTT.PublishingTools
{
    /// <summary>
    /// Provides operations for loading textures from packages/assets. It uses the full package name or 
    /// asset json info to resolve the base path towards your package its 'Editor/Art' directory meaning
    /// you only have to provide the relative path from this folder.
    /// </summary>
    public static class EditorTextures
    {
        /// <summary>
        /// Loads a texture from a dtt package.
        /// <para>Uses the package its 'Editor/Art' directory as base path.</para>
        /// </summary>
        /// <typeparam name="T">The type of texture.</typeparam>
        /// <param name="fullPackageName">The full package name.</param>
        /// <param name="relativePath">The relative path from the 'Editor/Art' folder.</param>
        /// <returns>The loaded texture.</returns>
        public static T Load<T>(string fullPackageName, string relativePath) where T : Texture
        {
            if (fullPackageName == null)
                throw new ArgumentNullException(nameof(fullPackageName));

            AssetJson assetJson = DTTEditorConfig.GetAssetJson(fullPackageName);
            if (assetJson == null)
                throw new ArgumentNullException(nameof(assetJson), $"Could not find asset json with package name: {fullPackageName}");

            return Load<T>(assetJson, relativePath);
        }

        /// <summary>
        /// Loads a texture from a dtt package.
        /// <para>Uses the package its 'Editor/Art' directory as base path.</para>
        /// </summary>
        /// <typeparam name="T">The type of texture.</typeparam>
        /// <param name="assetJson">The asset json info.</param>
        /// <param name="relativePath">The relative path from the 'Editor/Art' folder.</param>
        /// <returns>The loaded texture.</returns>
        public static T Load<T>(AssetJson assetJson, string relativePath) where T : Texture
        {
            if (assetJson == null)
                throw new ArgumentNullException(nameof(assetJson));

            if (relativePath == null)
                throw new ArgumentNullException(relativePath);

            string path = Path.Combine(DTTEditorConfig.GetContentFolderPath(assetJson), "Editor", "Art", relativePath);
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }
    }
}
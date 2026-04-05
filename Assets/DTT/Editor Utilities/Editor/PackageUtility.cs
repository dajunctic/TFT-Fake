#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Provides utility methods for interacting with packages in Unity's 'Packages' folder.
    /// </summary>
    public static class PackageUtility
    {
        /// <summary>
        /// Returns the package info of an asset.
        /// </summary>
        /// <param name="asset">The asset to get the package info for.</param>
        /// <returns>The package info.</returns>
        public static PackageInfo GetPackageInfo(this Object asset)
        {
            if (asset == null)
                throw new ArgumentNullException(nameof(asset));

            string assetPath = AssetDatabase.GetAssetPath(asset);
            if (string.IsNullOrEmpty(assetPath))
            {
                Debug.LogWarning($"Asset {asset.name} was not found in the asset database. Make sure it references an actual asset.");
                return null;
            }

            PackageInfo info = PackageInfo.FindForAssetPath(assetPath);
            if (info == null)
                Debug.LogWarning($"No package info was found for asset {asset.name}. Make sure it is inside a package folder.");

            return info;
        }
    }
}

#endif

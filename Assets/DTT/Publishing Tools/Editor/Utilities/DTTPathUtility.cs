using System;
using System.IO;
using DTT.Utils.Workflow;

namespace DTT.PublishingTools.Utils
{
    /// <summary>
    /// Provides utility methods for asset paths related to the DTT package.
    /// </summary>
    public static class DTTPathUtility
    {
        /// <summary>
        /// Returns whether the given asset path is inside a DTT directory.
        /// </summary>
        /// <param name="assetPath">The asset path to check.</param>
        /// <returns>Whether the given asset path is inside a DTT directory.</returns>
        public static bool InDTTDirectory(string assetPath)
        {
            if (assetPath == null)
                throw new ArgumentNullException(nameof(assetPath));

            string rootDirectory = PathUtility.GetPathElementAt(assetPath, 0);

            if (rootDirectory == "Assets")
            {
                return PathUtility.ContainsDirectory(assetPath, "DTT");
            } 
            
            if (rootDirectory == "Packages")
            {
                string subDirectory = PathUtility.GetPathElementAt(assetPath, 1);
                return subDirectory.Contains("dtt.") || subDirectory.Contains("nl.dtt.");
            }
            
            return false;
        }

        /// <summary>
        /// Returns whether the given asset path is corresponding with a dtt asset.json file.
        /// </summary>
        /// <param name="assetPath">The asset path to check.</param>
        /// <returns>Whether the given asset path is corresponding with a dtt asset.json file.</returns>
        public static bool IsAssetJson(string assetPath) => Path.HasExtension(assetPath)
                                                            && InDTTDirectory(assetPath) && Path.GetFileName(assetPath) == "asset.json";
    }
}

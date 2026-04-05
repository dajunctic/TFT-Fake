using DTT.Utils.Exceptions;
using DTT.Utils.Extensions;
using System;
using System.IO;
using UnityEngine;

namespace DTT.Utils.Workflow
{
    /// <summary>
    /// Provides path related utility methods.
    /// </summary>
    public static class PathUtility
    {
        /// <summary>
        /// Returns the element of the path depending on the given
        /// index when split using the directory separation character.
        /// </summary>
        /// <param name="path">The path of which to get the element.</param>
        /// <param name="index">The index of the split elements.</param>
        /// <returns>The name of the directory.</returns>
        public static string GetPathElementAt(this string path, int index)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            string[] names = path.Split(Path.AltDirectorySeparatorChar);
            if (names.HasIndex(index))
                return names[index];
            else
                return string.Empty;
        }

        /// <summary>
        /// Returns whether the given directory name is part of the given path.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <param name="directoryName">The directory name to look for.</param>
        /// <returns>Whether the given directory name is part of the given path.</returns>
        public static bool ContainsDirectory(string path, string directoryName)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            if (directoryName == null)
                throw new ArgumentNullException(nameof(directoryName));

            string[] directoriesInPath = path.Split(Path.AltDirectorySeparatorChar);
            for (int i = 0; i < directoriesInPath.Length; i++)
            {
                if (directoriesInPath[i] == directoryName)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Ensures the directory at given directory path exists by creating
        /// it if it isn't there yet.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        public static void EnsureDirectoryExistence(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath))
                throw new NullOrEmptyException(nameof(directoryPath));

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
        }

        /// <summary>
        /// Returns the asset path for a given full path of the asset.
        /// </summary>
        /// <param name="fullPathOfAsset">The full path of the asset.</param>
        /// <returns>The asset path.</returns>
        public static string ToAssetPath(this string fullPathOfAsset)
        {
            if (string.IsNullOrEmpty(fullPathOfAsset))
                throw new NullOrEmptyException(nameof(fullPathOfAsset));

            if (fullPathOfAsset.Contains("\\"))
                fullPathOfAsset = fullPathOfAsset.Replace("\\", "/");

            string path;
            if (fullPathOfAsset.StartsWith(Application.dataPath))
                path = "Assets" + fullPathOfAsset.Substring(Application.dataPath.Length);
            else
                path = fullPathOfAsset;

            // Make sure the path is compatible with an asset path structure, otherwise Unity won't find it.
            return path.Replace("\\", "/");
        }

        /// <summary>
        /// Returns whether a path corresponds with an asset inside the packages folder.
        /// </summary>
        /// <param name="assetPath">The asset path to check.</param>
        /// <returns>Whether the path corresponds with an asset inside the packages folder.</returns>
        public static bool IsPackagePath(this string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
                throw new NullOrEmptyException(nameof(assetPath));

            return assetPath.StartsWith("Packages/");
        }
    }

}

#if UNITY_EDITOR

using System;

namespace DTT.PublishingTools
{
    /// <summary>
    /// A container of an asset.json file containing relevant data 
    /// of a package/asset.
    /// </summary>
    [Serializable]
    public class AssetJson
    {
        /// <summary>
        /// Whether this package is used as an asset for a store release.
        /// </summary>
        public bool assetStoreRelease;

        /// <summary>
        /// The full package name. This should correspond with the "name"
        /// in the package.json file.
        /// </summary>
        public string packageName;

        /// <summary>
        /// The version of the package.
        /// </summary>
        public string version;

        /// <summary>
        /// The display name used for the package.
        /// </summary>
        public string displayName;

        /// <summary>
        /// The documentation url of the asset.
        /// </summary>
        public string documentationUrl = string.Empty;

        /// <summary>
        /// The path towards a custom icon to be drawn in the header.
        /// <para>This path should be relative to the package folder.</para>
        /// </summary>
        public string customIconPath;
    }
}

#endif

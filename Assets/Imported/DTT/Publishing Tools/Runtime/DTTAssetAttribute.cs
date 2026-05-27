using System;

namespace DTT.PublishingTools.Attributes
{
    /// <summary>
    /// Marks a scriptable object as DTT asset, ensuring it always has an asset instance
    /// in the DTT project folder.
    /// </summary>
    public class DTTAssetAttribute : Attribute
    {
        /// <summary>
        /// The full package name of the package the asset belongs to.
        /// </summary>
        public readonly string fullPackageName;

        /// <summary>
        /// Whether the asset uses the resources folder to be loaded.
        /// </summary>
        public readonly bool isResource;

        /// <summary>
        /// The custom relative directory path from the package folder. DTT/packageDisplayName/relativePath.
        /// Should include the .asset extension if the asset name is included in the path.
        /// </summary>
        public readonly string relativePath;

        /// <summary>
        /// A custom name used for the asset to be created. Should include the .asset extension.
        /// </summary>
        public readonly string assetName;

        /// <summary>
        /// Creates a new instance of the attribute.
        /// </summary>
        /// <param name="fullPackageName">The full package name of the package the asset belongs to.</param>
        /// <param name="isResource">Whether the asset uses the resources folder to be loaded.</param>
        public DTTAssetAttribute(string fullPackageName, bool isResource)
        {
            this.fullPackageName = fullPackageName;
            this.isResource = isResource;
        }
        
        /// <summary>
        /// Creates a new instance of the attribute.
        /// </summary>
        /// <param name="fullPackageName">The full package name of the package the asset belongs to.</param>
        /// <param name="assetName">A custom name used for the asset to be created. Should include the .asset extension.</param>
        /// <param name="isResource">Whether the asset uses the resources folder to be loaded.</param>
        public DTTAssetAttribute(string fullPackageName, string assetName, bool isResource)
        {
            this.fullPackageName = fullPackageName;
            this.assetName = assetName;
            this.isResource = isResource;
        } 

        /// <summary>
        /// Creates a new instance of the attribute.
        /// </summary>
        /// <param name="fullPackageName">The full package name of the package the asset belongs to.</param>
        /// <param name="relativePath">The custom relative directory path from the package folder. DTT/packageDisplayName/relativePath.
        /// Should include the .asset extension if the asset name is included in the path.</param>
        public DTTAssetAttribute(string fullPackageName, string relativePath)
        {
            this.fullPackageName = fullPackageName;
            this.relativePath = relativePath;
        }
    }
}


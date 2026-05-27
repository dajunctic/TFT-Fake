#if UNITY_EDITOR

using System;

namespace DTT.PublishingTools
{
    /// <summary>
    /// Used for drawing a header for a specific package.
    /// </summary>
    public class DTTHeaderAttribute : Attribute
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of this object, using the full package name.
        /// </summary>
        /// <param name="fullPackageName">The full name of the package (e.g. dtt.editorutilities).</param>
        public DTTHeaderAttribute(string fullPackageName) : this(fullPackageName, null, null, null)
        {
        }

        /// <summary>
        /// Creates a new instance of this object, using the full package name and a custom display name.
        /// </summary>
        /// <param name="fullPackageName">The full name of the package (e.g. dtt.editorutilities).</param>
        /// <param name="customDisplayName">The display name to show on the header.</param>
        public DTTHeaderAttribute(string fullPackageName, string customDisplayName)
            : this(fullPackageName, customDisplayName, null, null)
        {
        }

        /// <summary>
        /// Creates a new instance of this object, using the full package name, a custom display name
        /// and a custom documentation url.
        /// </summary>
        /// <param name="fullPackageName">The full name of the package (e.g. dtt.editorutilities).</param>
        /// <param name="customDisplayName">The display name to show on the header.</param>
        /// <param name="customDocumentationUrl">
        /// The url towards the documentation. 
        /// This can be an webUrl or local path relative to the package.
        /// </param>
        public DTTHeaderAttribute(string fullPackageName, string customDisplayName, string customDocumentationUrl)
            : this(fullPackageName, customDisplayName, customDocumentationUrl, null)
        {
        }

        /// <summary>
        /// Creates a new instance of this object, using the full package name, a custom display name,
        /// documentation url and icon path.
        /// </summary>
        /// <param name="fullPackageName">The full name of the package (e.g. dtt.editorutilities).</param>
        /// <param name="customDisplayName">The display name to show on the header.</param>
        /// <param name="customDocumentationUrl">
        /// The url towards the documentation. 
        /// This can be an webUrl or local path relative to the package.
        /// </param>
        /// <param name="customIconPath">
        /// The path towards a custom icon. 
        /// This value needs to be a local path relative to the package folder.
        /// </param>
        public DTTHeaderAttribute(
            string fullPackageName,
            string customDisplayName,
            string customDocumentationUrl,
            string customIconPath)
        {
            this.fullPackageName = fullPackageName;
            this.displayName = customDisplayName;
            this.documentationUrl = customDocumentationUrl;
            this.customIconPath = customIconPath;
        }
        #endregion

        #region Variables
        #region Public
        /// <summary>
        /// The full name of the package (e.g. dtt.editorutilities).
        /// </summary>
        public readonly string fullPackageName;

        /// <summary>
        /// The display name to show on the header. 
        /// <para>If this value is null, the asset json value based on the full package name will be used.</para>
        /// </summary>
        public readonly string displayName;

        /// <summary>
        /// The url towards the documentation. This can be an webUrl or local path relative to the package.
        /// <para>If this value is null, the asset json value based on the full package name will be used.</para>
        /// </summary>
        public readonly string documentationUrl;

        /// <summary>
        /// The path towards a custom icon. This value needs to be a local path relative to the package.
        /// </summary>
        public readonly string customIconPath;
        #endregion
        #endregion
    }
}

#endif

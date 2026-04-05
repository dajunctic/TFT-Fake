#if UNITY_EDITOR

using DTT.Utils.EditorUtilities;
using UnityEngine;

namespace DTT.PublishingTools
{
    /// <summary>
    /// Stores the content used for drawing the header.
    /// </summary>
    internal class DTTHeaderContent : GUIContentCache
    {
        #region Variables
        #region Public
        /// <summary>
        /// Used for drawing the documentation link.
        /// </summary>
        public GUIContent DocumentationLabel => base[nameof(DocumentationLabel)];

        /// <summary>
        /// Used for drawing the package name.
        /// </summary>
        public GUIContent PackageNameLabel => base[nameof(PackageNameLabel)];

        /// <summary>
        /// Used for drawing the version.
        /// </summary>
        public GUIContent VersionLabel => base[nameof(VersionLabel)];

        /// <summary>
        /// Used for drawing the company name.
        /// </summary>
        public GUIContent DTTLabel => base[nameof(DTTLabel)];
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of this object, initializing the content.
        /// </summary>
        /// <param name="displayName">The name to display.</param>
        /// <param name="version">The version of the package to display.</param>
        public DTTHeaderContent(string displayName, string version)
        {
            Add(nameof(DocumentationLabel), () => new GUIContent("Open documentation"));

            Add(nameof(PackageNameLabel), () => new GUIContent(displayName));

            Add(nameof(VersionLabel), () => new GUIContent($"V{version}"));

            Add(nameof(DTTLabel), () => new GUIContent("DTT"));
        }
        #endregion
    }
}

#endif
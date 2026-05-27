#if UNITY_EDITOR

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Provides constants for unity build in names.
    /// </summary>
    public class PathNames
    {
        /// <summary>
        /// The name of the editor folder used to put editor scripts in.
        /// </summary>
        public const string EDITOR_FOLDER = "Editor";

        /// <summary>
        /// The name of the project folder.
        /// </summary>
        public const string PROJECT_FOLDER = "Assets";

        /// <summary>
        /// The name of the resources folder.
        /// </summary>
        public const string RESOURCES_FOLDER = "Resources";

        /// <summary>
        /// The name of the packages folder.
        /// </summary>
        public const string PACKAGES_FOLDER = "Packages";

        /// <summary>
        /// The extension for an asset file.
        /// </summary>
        public const string ASSET_EXTENSIONS = ".asset";

        /// <summary>
        /// The extensions for a scene file.
        /// </summary>
        public const string SCENE_EXTENSION = ".unity";

        /// <summary>
        /// The path for unity build in resources.
        /// </summary>
        public const string BUILDIN_RESOURCES = "Resources/unity_builtin_extra";
    }
}

#endif
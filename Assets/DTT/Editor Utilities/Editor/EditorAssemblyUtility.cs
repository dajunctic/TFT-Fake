#if UNITY_EDITOR

using UnityEditor.Compilation;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Provides utility methods for interaction with Assemblies in your project.
    /// </summary>
    /// <remarks>Assemblies are only found inside the project using the <see cref="CompilationPipeline"/></remarks>
    public static class EditorAssemblyUtility
    {
        #region Methods
        #region Public
        /// <summary>
        /// Returns whether a script is part of an assembly inside the project.
        /// </summary>
        /// <param name="scriptAssetPath">The asset path of the script.</param>
        /// <returns>Whether the script is part of an assembly inside the project.</returns>
        public static bool IsPartOfProjectAssembly(string scriptAssetPath) => !string.IsNullOrEmpty(GetAssemblyName(scriptAssetPath));

        /// <summary>
        /// Returns the assembly name a script is part of.
        /// </summary>
        /// <param name="scriptAssetPath">The asset path of the script.</param>
        /// <returns>The assembly name.</returns>
        public static string GetAssemblyName(string scriptAssetPath) => CompilationPipeline.GetAssemblyNameFromScriptPath(scriptAssetPath);
        #endregion
        #endregion
    }
}

#endif
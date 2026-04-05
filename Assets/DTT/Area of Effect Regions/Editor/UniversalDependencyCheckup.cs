using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace DTT.AreaOfEffectRegions.Editor
{
    /// <summary>
    /// CHeck if the universal renderer pipeline package is present. Set up the player defines symbols PlayerSettings.
    /// </summary>
    [InitializeOnLoad]
    public class UniversalDependencyCheckup : UnityEditor.Editor
    {
        /// <summary>
        /// Name of the class that is being checked.
        /// </summary>
        static string className = "DecalProjector.cs";

        /// <summary>
        /// Add define symbols as soon as Unity gets done compiling.
        /// </summary>
        static UniversalDependencyCheckup()
        {
            bool packageFound = AreUniversalFound();
            ModifyDefineSymbols(packageFound);
        }

        /// <summary>
        /// Check if the necessary class are in the assets.
        /// </summary>
        /// <returns>True if the package is in the project.</returns>
        internal static bool AreUniversalFound()
        {
            // Find all package files in the Assets folder.
            List<string> packages = AssetDatabase.FindAssets("DecalProjector", new[] { "Packages" })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(x => AssetDatabase.LoadAssetAtPath<TextAsset>(x) != null)
                .ToList();
            List<string> packageNames = new List<string>();
            if (packages != null)
            {
                foreach (string package in packages)
                {
                    if (package.Contains(className))
                    {
                        packageNames.Add(package);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Modify the define symbols depending on whether plugins were found or not.
        /// </summary>
        /// <param name="universalFound">Wether the plugins are present in the project</param>
        internal static void ModifyDefineSymbols(bool universalFound)
        {
            // Get the current define symbols.
            string definesString =
                PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<String> allDefines = definesString.Split(';').ToList();

            bool plugInDefines = allDefines.Contains(Constants.UNIVERSAL_PLUGIN_SYMBOL);
            // If the plugins are present, add the Google Plugin symbol to the symbols.
            if (universalFound && !plugInDefines)
                allDefines.Add(Constants.UNIVERSAL_PLUGIN_SYMBOL);
            // If not, remove it.
            else if (!universalFound && plugInDefines)
                allDefines.Remove(Constants.UNIVERSAL_PLUGIN_SYMBOL);
            else
                return;
            // Update the define symbols.
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";",allDefines.ToArray()));
            CompilationPipeline.RequestScriptCompilation();
        }
    }
}


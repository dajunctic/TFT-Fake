using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DTT.PublishingTools.Attributes;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Compilation;
using UnityEngine;
using System.Reflection;
using System.Text;
using DTT.Utils.Workflow;

using Assembly = System.Reflection.Assembly;
using UnityAssembly = UnityEditor.Compilation.Assembly;

namespace DTT.PublishingTools
{
    /// <summary>
    /// Handles DTT Asset attribute implementation.
    /// </summary>
    internal static class DTTAssetService 
    {
        /// <summary>
        /// Represents a scriptable object that uses an asset attribute.
        /// </summary>
        private readonly struct AssetWithAttribute
        {
            /// <summary>
            /// The attribute used by the scriptable object.
            /// </summary>
            public readonly DTTAssetAttribute attribute;

            /// <summary>
            /// The type of asset to be created.
            /// </summary>
            public readonly Type assetType;

            /// <summary>
            /// Creates a new instance of the stucture.
            /// </summary>
            /// <param name="attribute">The attribute used by the scriptable object.</param>
            /// <param name="assetType">The type of asset to be created.</param>
            public AssetWithAttribute(DTTAssetAttribute attribute, Type assetType)
            {
                this.attribute = attribute;
                this.assetType = assetType;
            }
        }
        
        /// <summary>
        /// Called when scripts are reloaded to go through dtt's runtime assemblies
        /// and create assets for scriptable objects with an Asset attribute.
        /// </summary>
        [DidReloadScripts]
        private static void CreateDTTAssetsIfNecessary()
        {
            AssetWithAttribute[] attributes = GetAssetsWithAttributes();
            
            for(int i = 0; i < attributes.Length; i++) 
                CreateDTTAssetIfNecessary(attributes[i]); 
        }

        /// <summary>
        /// Returns an array of scriptable object representations that implement
        /// an asset attribute.
        /// </summary>
        /// <returns>The array of scriptable object representations.</returns>
        private static AssetWithAttribute[] GetAssetsWithAttributes()
        {
            // Get all dtt assemblies.
            var assetsWithAttributes = new List<AssetWithAttribute>();
            var dttAssemblies = CompilationPipeline.GetAssemblies().Where(IsDTTRuntimeAssembly);

            // Start adding types with their asset attributes from assemblies to the result list.
            foreach (UnityAssembly assembly in dttAssemblies)
            {
                Type[] types;
                try
                {
                    string dataPathWithoutAssets = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/') + 1);
                    string combinedPath = Path.Combine(dataPathWithoutAssets, assembly.outputPath);
                    types  = Assembly.LoadFile(combinedPath).GetTypes();
                }
                catch(FileNotFoundException)
                {
                    // Catch file not found exceptions that sometimes occur when trying to load assembly's from unity.
                    continue;
                }
                
                for (int i = 0; i < types.Length; i++)
                {
                    var attribute = types[i].GetCustomAttributes(typeof(DTTAssetAttribute)).FirstOrDefault();
                    if (attribute != null)
                        assetsWithAttributes.Add(new AssetWithAttribute((DTTAssetAttribute)attribute, types[i]));
                }
            }

            return assetsWithAttributes.ToArray();
        }

        /// <summary>
        /// Creates a dtt asset if necessary based on whether there already is one created.
        /// </summary>
        /// <param name="asset">The asset that should be created.</param>
        private static void CreateDTTAssetIfNecessary(AssetWithAttribute asset)
        {
            string fullPackageName = asset.attribute.fullPackageName;
            if (fullPackageName == null)
            {
                Debug.LogWarning("DTT asset could not be created :: full package name was null.");
                return;
            }
            
            AssetJson assetJson = DTTEditorConfig.GetAssetJson(fullPackageName);
            if (assetJson == null)
                return;

            if (!asset.assetType.IsSubclassOf(typeof(ScriptableObject)))
            {
                Debug.LogWarning("DTT asset could not be created :: " +
                                 "Only scriptable objects can use the DTTAsset attribute.");
                return;
            }
            
            string projectFolderPath = DTTEditorConfig.DTTProjectFolder + "/" + assetJson.displayName;
            StringBuilder assetPathBuilder = new StringBuilder(projectFolderPath);

            // Apply relative path or resources path if necessary.
            if (asset.attribute.relativePath != null)
                AppendAssetPath(assetPathBuilder, asset.attribute.relativePath);
            else if (asset.attribute.isResource)
                AppendAssetPath(assetPathBuilder, "Resources");
            
            // Ensure the directory exists before creating the asset.
            EnsureDirectoryExistence(assetPathBuilder.ToString()); 
            
            // Append an asset name if none has been added manually.
            if(!Path.HasExtension(assetPathBuilder.ToString()))
                AppendAssetPath(assetPathBuilder, GetNameForAsset(asset));
            
            string assetPath = assetPathBuilder.ToString(); 
            
            // Do nothing if the file for the asset already exists.
            if (File.Exists(assetPath))
                return;

            // If the asset has not yet been created, create one at the asset path.
            var scriptableObject = ScriptableObject.CreateInstance(asset.assetType);
            AssetDatabase.CreateAsset(scriptableObject, assetPath);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// Returns whether a unity assembly is a dtt runtime assembly.
        /// </summary>
        /// <param name="assembly">The assembly to check.</param>
        /// <returns>Whether it is a dtt runtime assembly.</returns>
        private static bool IsDTTRuntimeAssembly(UnityAssembly assembly)
            => assembly.name.StartsWith("DTT") && assembly.name.EndsWith("Runtime");

        /// <summary>
        /// Ensures the existence of the directory at given asset path.
        /// </summary>
        /// <param name="assetPath">The asset path of which to ensure the directory's existence.</param>
        private static void EnsureDirectoryExistence(string assetPath)
        {
            if (Path.HasExtension(assetPath))
                assetPath = assetPath.Remove(assetPath.LastIndexOf('/'));
            
            PathUtility.EnsureDirectoryExistence(assetPath);
        }

        /// <summary>
        /// Returns the name for an asset with an attribute.
        /// </summary>
        /// <param name="asset">The asset with an attribute to get the name for.</param>
        /// <returns>The name for the asset.</returns>
        private static string GetNameForAsset(AssetWithAttribute asset)
        {
            string name = asset.attribute.assetName;
            if (name == null)
                return asset.assetType.Name + ".asset";

            if (!name.EndsWith(".asset"))
                name += ".asset";

            return name;
        }

        /// <summary>
        /// Appends a given path to an asset path builder.
        /// </summary>
        /// <param name="assetPathBuilder">The asset path builder to append the path to.</param>
        /// <param name="pathToAppend">The path to append.</param>
        private static void AppendAssetPath(StringBuilder assetPathBuilder, string pathToAppend)
        {
            assetPathBuilder.Append("/");
            assetPathBuilder.Append(pathToAppend);
        }
    }
}


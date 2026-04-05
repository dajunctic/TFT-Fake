#if UNITY_EDITOR

using DTT.Utils.EditorUtilities.Exceptions;
using System;
using System.IO;
using DTT.Utils.Exceptions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// A static utility class for retrieving data from the Unity Asset Database.
    /// </summary>
    public class AssetDatabaseUtility
    {
        /// <summary>
        /// Returns an array of assets loaded of <typeparamref name="T"/> from the 
        /// asset database using given filter argument.
        /// </summary>
        /// <param name="filter">The filter to use for finding the assets.</param>
        /// <param name="searchFolders">The folders to look in when searching.</param>
        /// <returns>The array of assets retrieved from the asset database.</returns>
        public static T[] LoadAssets<T>(string filter, string[] searchFolders = null) where T : UnityEngine.Object
        {
            // Guids are found in search folders if there are any given.
            string[] guids = searchFolders == null
                ? AssetDatabase.FindAssets(filter)
                : AssetDatabase.FindAssets(filter, searchFolders);

            T[] array = new T[guids.Length];

            for (int i = 0; i < array.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                array[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return array;
        }

        /// <summary>
        /// Returns an array of assets loaded of <typeparamref name="T"/> from the 
        /// asset database.
        /// </summary>
        /// <param name="searchFolders">The folders to look in when searching.</param>
        /// <returns>The array of assets retrieved from the asset database.</returns>
        public static T[] LoadAssets<T>(string[] searchFolders = null) where T : UnityEngine.Object
        {
            string filter = string.Format("t:{0}", typeof(T).Name);

            // Guids are found in search folders if there are any given.
            string[] guids = searchFolders != null
               ? AssetDatabase.FindAssets(filter, searchFolders)
               : AssetDatabase.FindAssets(filter);

            T[] array = new T[guids.Length];

            for (int i = 0; i < array.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                array[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return array;
        }

        /// <summary>
        /// Returns an asset of <typeparamref name="T"/> from the 
        /// asset database.
        /// <para>If there are multiple it will return the first one found.</para>
        /// </summary>
        /// <param name="filter">The filter to use for finding the asset</param>
        /// <param name="searchFolders">The folder to look in when searching.</param>
        /// <returns>The asset retrieved from the asset database.</returns>
        public static T LoadAsset<T>() where T : UnityEngine.Object
        {
            T[] assets = LoadAssets<T>();

            return assets.Length != 0 ? assets[0] : null;
        }

        /// <summary>
        /// Returns a loaded asset of <typeparamref name="T"/> from the 
        /// asset database using given filter argument.
        /// <para>If there are multiple it will return the first one found.</para>
        /// </summary>
        /// <param name="filter">The filter to use for finding the asset</param>
        /// <param name="searchFolders">The folder to look in when searching.</param>
        /// <returns>The asset retrieved from the asset database.</returns>
        public static T LoadAsset<T>(string filter) where T : UnityEngine.Object
        {
            if (filter == null)
                throw new AssetDatabaseException("Filter is null.");

            T[] assets = LoadAssets<T>(filter);

            return assets.Length != 0 ? assets[0] : null;
        }

        /// <summary>
        /// Returns a loaded asset of <typeparamref name="T"/> from the 
        /// asset database using given filter argument.
        /// <para>If there are multiple it will return the first one found.</para>
        /// </summary>
        /// <param name="filter">The filter to use for finding the asset</param>
        /// <param name="searchFolders">The folder to look in when searching.</param>
        /// <returns>The asset retrieved from the asset database.</returns>
        public static T LoadAsset<T>(string filter, string searchFolder) where T : UnityEngine.Object
        {
            T[] assets = LoadAssets<T>(filter, new string[] { searchFolder });

            return assets.Length != 0 ? assets[0] : null;
        }

        /// <summary>
        /// Saves and refreshes assets after the inspectors have been updated.
        /// Use a delayed call for saving assets when saving instantly causes a Unity-problem that
        /// will occur whenever an inspector previewing an asset is open while starting playmode.
        /// </summary>
        public static void SaveAndRefreshAssetsDelayed()
        {
            EditorApplication.delayCall += SaveAndRefresh;
            
            void SaveAndRefresh()
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                EditorApplication.delayCall -= SaveAndRefresh;
            }
        }

        /// <summary>
        /// Returns a component attached to a prefab at given path. Will return null
        /// if the component was not attached.
        /// </summary>
        /// <typeparam name="T">The type of component.</typeparam>
        /// <param name="prefabPath">The path the prefab can be found.</param>
        /// <returns>The component attached to the prefab.</returns>
        public static T GetComponentInPrefab<T>(string prefabPath) where T : Component
        {
            if (string.IsNullOrEmpty(prefabPath))
                throw new NullOrEmptyException(nameof(prefabPath));
            
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab == null)
            {
                if(File.Exists(prefabPath))
                    throw new AssetDatabaseException($"Failed to load asset even though it exists at path {prefabPath}. " +
                             "This can happen when changes outside of Unity have been made to your asset and you load it " +
                             "during project startup with attributes like [DidReloadScripts] and [InitializeOnLoad].");
                
                throw new AssetDatabaseException($"Prefab can't be loaded at path {prefabPath}");
            }

            return prefab.GetComponent<T>();
        }

        /// <summary>
        /// Tries returning a loaded scriptable object asset at given path but creates it if
        /// it didn't exist.
        /// <para>This operation will not save the asset if it has been created.</para>
        /// </summary>
        /// <typeparam name="T">The type of scriptable object asset.</typeparam>
        /// <param name="path">The path at which to load or create.</param>
        /// <returns>The loaded or created scriptable object asset reference.</returns>
        public static T GetOrCreateScriptableObjectAsset<T>(string path) where T : ScriptableObject
        {
            if (File.Exists(path))
            {
                T asset = AssetDatabase.LoadAssetAtPath<T>(path);
                if (asset == null)
                {
                    throw new AssetDatabaseException($"Failed to load asset even though it exists at path {path}. " +
                        "This can happen when changes outside of Unity have been made to your asset and you load it " +
                        "during project startup with attributes like [DidReloadScripts] and [InitializeOnLoad].");
                }

                return asset;
            }
            
            T instance = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(instance, path);
            return instance;
        }
        
        /// <summary>
        /// Tries returning a loaded scriptable object asset at given path but creates it if
        /// it didn't exist.
        /// <para>This operation will not save the asset if it has been created.</para>
        /// </summary>
        /// <typeparam name="T">The type of scriptable object asset.</typeparam>
        /// <param name="path">The path at which to load or create.</param>
        /// <param name="wasCreated">Whether the asset could not be found and was created.</param>
        /// <returns>The loaded or created scriptable object asset reference.</returns>
        public static T GetOrCreateScriptableObjectAsset<T>(string path, out bool wasCreated) where T : ScriptableObject
        {
            wasCreated = false;
            if (File.Exists(path))
            {
                T asset = AssetDatabase.LoadAssetAtPath<T>(path);
                if (asset == null)
                {
                    throw new AssetDatabaseException($"Failed to load asset even though it exists at path {path}. " +
                                                     "This can happen when changes outside of Unity have been made to your asset and you load it " +
                                                     "during project startup with attributes like [DidReloadScripts] and [InitializeOnLoad].");
                }

                return asset;
            }

            wasCreated = true;
            T instance = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(instance, path);
            return instance;
        }

        /// <summary>
        /// Creates a prefab asset at given path using given GameObject instance.
        /// Will destroy the instance if the creation was succesful.
        /// <para>
        /// This function doesn't return a prefab reference since there
        /// is no certainty it can be created instantly.
        /// </para>
        /// </summary>
        /// <param name="path">The path at which to create the prefab.</param>
        /// <param name="instanceRoot">
        /// The GameObject instance to use for creating the prefab.
        /// </param>
        public static void CreatePrefabAtPath(string path, GameObject instanceRoot)
        {
            try
            {
                var prefab = PrefabUtility.SaveAsPrefabAsset(instanceRoot, path, out bool succes);
                if (succes)
                {
                    Debug.Log($"Creating prefab at path: {path}");
                    if (prefab != null)
                        Debug.Log($"{prefab} has been instantly created.");
                    else
                        Debug.Log("The prefab will be created after imports have finished.");
                }
                else
                {
                    Debug.LogWarning("Failed creating prefab asset because saving failed.");
                }
            }
            catch (Exception e)
            {
                throw new AssetDatabaseException("Failed creating prefab.", e);
            }
            finally
            {
                GameObject.DestroyImmediate(instanceRoot);
            }
        }

        /// <summary>
        /// Opens a script asset.
        /// </summary>
        /// <param name="scriptAsset">The script asset to open.</param>
        public static void OpenScript(TextAsset scriptAsset) => OpenScript(AssetDatabase.GetAssetPath(scriptAsset), 0, 0);

        /// <summary>
        /// Opens a script at given path.
        /// </summary>
        /// <param name="scriptAssetPath">The script asset path.</param>
        public static bool OpenScript(string scriptAssetPath, int line = 0, int column = 0)
        {
            if (scriptAssetPath == null)
                throw new ArgumentNullException(nameof(scriptAssetPath));

            return InternalEditorUtility.OpenFileAtLineExternal(scriptAssetPath, line, column);
        }

        /// <summary>
        /// Opens a prefab asset in prefab mode.
        /// </summary>
        /// <param name="prefabAsset">The prefab to open in prefab mode.</param>
        public static bool OpenPrefab(GameObject prefabAsset)
        {
            if (prefabAsset == null)
                throw new ArgumentNullException(nameof(prefabAsset));
            
            return AssetDatabase.OpenAsset(prefabAsset);
        }
    }
}

#endif
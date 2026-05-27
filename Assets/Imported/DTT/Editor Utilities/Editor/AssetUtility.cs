#if UNITY_EDITOR

using System;
using System.IO;
using System.Linq;
using DTT.Utils.Exceptions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Provides an API and menu items to modify compile ability of a script asset using
    /// pre processing defines.
    /// </summary>
    public static class AssetUtility
    {
        /// <summary>
        /// The unity editor preprocessing define.
        /// </summary>
        public const string UNITY_EDITOR_PREPROCESS_NAME = "UNITY_EDITOR";

        /// <summary>
        /// The unity ios preprocessing define.
        /// </summary>
        public const string IOS_PREPROCESS_NAME = "UNITY_IOS";

        /// <summary>
        /// The unity android preprocessing define.
        /// </summary>
        public const string ANDROID_PREPROCESS_NAME = "UNITY_ANDROID";

        /// <summary>
        /// The base menu item name using when right-clicking an asset.
        /// </summary>
        private const string SCRIPT_PREPROCESS_MENU_ITEM_NAME = "Assets/PreProcess Selected/";

        /// <summary>
        /// The menu item name for adding a scene asset to build settings.
        /// </summary>
        private const string SCENE_BUILD_SETTINGS_MENU_ITEM_NAME = "Assets/Add to build settings";

        /// <summary>
        /// The preprocessing if statement.
        /// </summary>
        private const string PRE_PROCESS_IF = "#if ";

        /// <summary>
        /// The preprocessing end of an if statement.
        /// </summary>
        private const string PRE_PROCESS_END_IF = "#endif";

        /// <summary>
        /// Called when the unity editor preprocessing menu item is clicked,
        /// it will apply unity editor preprocessing on selected assets.
        /// </summary>
        [MenuItem(SCRIPT_PREPROCESS_MENU_ITEM_NAME + UNITY_EDITOR_PREPROCESS_NAME)]
        private static void OnUnityEditorAssetPreProcess()
        {
            string[] assetGuids = Selection.assetGUIDs;
            for (int i = 0; i < assetGuids.Length; i++)
                ApplyPreProcessingOnAsset(assetGuids[i], UNITY_EDITOR_PREPROCESS_NAME);
        }

        /// <summary>
        /// Called when the unity ios preprocessing menu item is clicked,
        /// it will apply unity ios preprocessing on selected assets.
        /// </summary>
        [MenuItem(SCRIPT_PREPROCESS_MENU_ITEM_NAME + IOS_PREPROCESS_NAME)]
        private static void OnIOSAssetPreProcess()
        {
            string[] assetGuids = Selection.assetGUIDs;
            for (int i = 0; i < assetGuids.Length; i++)
                ApplyPreProcessingOnAsset(assetGuids[i], IOS_PREPROCESS_NAME);
        }


        /// <summary>
        /// Called when the unity android preprocessing menu item is clicked,
        /// it will apply unity android preprocessing on selected assets.
        /// </summary>
        [MenuItem(SCRIPT_PREPROCESS_MENU_ITEM_NAME + ANDROID_PREPROCESS_NAME)]
        private static void OnAndroidAssetPreProcess()
        {
            string[] assetGuids = Selection.assetGUIDs;
            for (int i = 0; i < assetGuids.Length; i++)
                ApplyPreProcessingOnAsset(assetGuids[i], ANDROID_PREPROCESS_NAME);
        }

        /// <summary>
        /// Adds selected scene assets to build settings.
        /// </summary>
        [MenuItem(SCENE_BUILD_SETTINGS_MENU_ITEM_NAME)]
        private static void OnAddSceneAssetToBuildSettings()
        {
            string[] assetGuids = Selection.assetGUIDs;
            if (assetGuids.Length != 0)
                AddScenesToBuildSettings(assetGuids.Select(guid => AssetDatabase.GUIDToAssetPath(guid)).ToArray());
        }

        /// <summary>
        /// Adds a scene asset at given path to the build settings.
        /// </summary>
        /// <param name="sceneAssetPath">The scene asset's path.</param>
        public static void AddSceneToBuildSettings(string sceneAssetPath)
        {
            if (sceneAssetPath == null)
                throw new ArgumentNullException(nameof(sceneAssetPath));

            AddScenesToBuildSettings(new string[]{ sceneAssetPath });
        }

        /// <summary>
        /// Returns whether a given scene asset path corresponds with a scene in the build settings.
        /// </summary>
        /// <param name="sceneAssetPath">The scene asset path to check.</param>
        /// <returns>Whether the given scene asset path corresponds with a scene in the build settings.</returns>
        public static bool IsScenePartOfBuildSettings(string sceneAssetPath)
        {
            if (sceneAssetPath == null)
                throw new ArgumentNullException(nameof(sceneAssetPath));

            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            for(int i = 0; i < scenes.Length; i++)
                if (scenes[i].path == sceneAssetPath)
                    return true;

            return false;
        }

        /// <summary>
        /// Adds scene asset's at given paths to the build settings.
        /// </summary>
        /// <param name="sceneAssetPaths">The scene asset's paths.</param>
        public static void AddScenesToBuildSettings(string[] sceneAssetPaths)
        {
            if (sceneAssetPaths == null)
                throw new ArgumentNullException(nameof(sceneAssetPaths));
            
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            EditorBuildSettingsScene[] updated = new EditorBuildSettingsScene[scenes.Length + sceneAssetPaths.Length];
            
            Array.Copy(scenes, updated, scenes.Length);
            
            for (int i = 0, j = scenes.Length; i < sceneAssetPaths.Length; i++, j++)
                updated[j] = new EditorBuildSettingsScene(sceneAssetPaths[i], true);

            EditorBuildSettings.scenes = updated;

            string[] newSceneNames = sceneAssetPaths.Select(path => Path.GetFileNameWithoutExtension(path)).ToArray();
            Debug.Log($"Successfully added scenes to build settings: {string.Join(", ", newSceneNames)}");
        }

        /// <summary>
        /// Will apply preprocessing on an asset at a given path.
        /// </summary>
        /// <param name="assetPath">The asset path.</param>
        /// <param name="preprocessName">The preprocess name. (e.g. UNITY_EDITOR)</param>
        public static void ApplyPreProcessingOnAssetAtPath(string assetPath, string preprocessName)
        {
            if (string.IsNullOrEmpty(assetPath))
                throw new NullOrEmptyException(nameof(assetPath));

            if (string.IsNullOrEmpty(preprocessName))
                throw new NullOrEmptyException(nameof(preprocessName));
            
            MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);
            if (script == null)
            {
                Debug.LogWarning($"Failed applying pre processing on asset at path {assetPath} as it is not a script asset.");
                return;
            }

            if (string.IsNullOrEmpty(script.text))
            {
                Debug.Log("Can't apply pre processing if script file text is null or empty.");
                return;
            }

            using (StringReader reader = new StringReader(script.text))
            {
                string firstLine = reader.ReadLine();
                if (firstLine.Contains(preprocessName))
                {
                    Debug.LogWarning($"Asset at {assetPath} already has preprocessing statement {preprocessName}.");
                    return;
                }

                string text;
                if (firstLine.Contains(PRE_PROCESS_IF))
                {
                    text = script.text.Replace(firstLine, firstLine + $" && {preprocessName}");
                }
                else
                {
                    text = script.text.Insert(0, string.Format("{0}{1}\n\n", PRE_PROCESS_IF, preprocessName)) +
                           string.Format("\n{0}", PRE_PROCESS_END_IF);
                }
                
                File.WriteAllText(assetPath, text);
                AssetDatabase.ImportAsset(assetPath);
            }
        }

        /// <summary>
        /// Will apply preprocessing on an asset with a given guid.
        /// </summary>
        /// <param name="assetGuid">The asset guid.</param>
        /// <param name="preprocessName">The preprocess name. (e.g. UNITY_EDITOR)</param>
        public static void ApplyPreProcessingOnAsset(string assetGuid, string preprocessName) => ApplyPreProcessingOnAssetAtPath(AssetDatabase.GUIDToAssetPath(assetGuid), preprocessName);

        /// <summary>
        /// Returns the editor icon for a unity object.
        /// </summary>
        /// <param name="unityObject">The unity object to get the editor icon for.</param>
        /// <returns>The editor icon.</returns>
        public static Texture GetEditorIcon(this Object unityObject)
        {
            if (unityObject == null)
                throw new ArgumentNullException(nameof(unityObject));

            if (unityObject is ScriptableObject)
                return EditorGUIUtility.IconContent("ScriptableObject Icon").image;

            if (unityObject is Component || unityObject is MonoScript)
                return EditorGUIUtility.IconContent("cs Script Icon").image;
            
            return EditorGUIUtility.IconContent($"{unityObject.GetType().Name} Icon").image;
        }
        
        /// <summary>
        /// Returns the editor icon for an object type.
        /// </summary>
        /// <param name="objectType">The object type to get the editor icon for.</param>
        /// <returns>The editor icon.</returns>
        public static Texture GetEditorIcon(this Type objectType)
        {
            if (objectType == null)
                throw new ArgumentNullException(nameof(objectType));

            if(!objectType.IsSubclassOf(typeof(Object)))
                return EditorGUIUtility.IconContent("cs Script Icon").image; 
            
            if (objectType.IsSubclassOf(typeof(ScriptableObject)))
                return EditorGUIUtility.IconContent("ScriptableObject Icon").image;

            if (objectType.IsSubclassOf(typeof(Component)) || objectType == typeof(MonoScript))
                return EditorGUIUtility.IconContent("cs Script Icon").image;
            
            return EditorGUIUtility.IconContent($"{objectType.Name} Icon").image;
        }
    }
}

#endif
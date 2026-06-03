using UnityEditor;
using UnityEngine;
using System.IO;
using FishNet.Component.Transforming;

namespace Dajunctic.Editor
{
    [InitializeOnLoad]
    public static class AddNetworkTransformToPrefabs
    {
        static AddNetworkTransformToPrefabs()
        {
            EditorApplication.delayCall += RunAndSelfClean;
        }

        private static void RunAndSelfClean()
        {
            if (EditorApplication.isPlaying || EditorApplication.isCompiling) return;

            string key = "AddNetworkTransformToPrefabs_Done_v2";
            if (SessionState.GetBool(key, false)) return;
            SessionState.SetBool(key, true);

            AddNetworkTransform();
        }

        [MenuItem("Window/TFT-Fake/Add Network Transform to Champions")]
        public static void AddNetworkTransform()
        {
            string folder = "Assets/Prefabs/Champions";
            string[] prefabFiles = Directory.GetFiles(folder, "*.prefab", SearchOption.AllDirectories);

            int count = 0;
            foreach (string file in prefabFiles)
            {
                GameObject prefabRoot = PrefabUtility.LoadPrefabContents(file);
                var netObj = prefabRoot.GetComponent<FishNet.Object.NetworkObject>();
                if (netObj != null)
                {
                    var netTrans = prefabRoot.GetComponent<NetworkTransform>();
                    if (netTrans == null)
                    {
                        prefabRoot.AddComponent<NetworkTransform>();
                        PrefabUtility.SaveAsPrefabAsset(prefabRoot, file);
                        Debug.Log($"Added NetworkTransform to {file}");
                        count++;
                    }
                }
                PrefabUtility.UnloadPrefabContents(prefabRoot);
            }
            Debug.Log($"Finished. Added NetworkTransform to {count} prefabs.");
        }
    }
}

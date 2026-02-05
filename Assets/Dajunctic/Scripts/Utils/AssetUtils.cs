using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dajunctic
{
    public static class AssetUtils
    {
#if UNITY_EDITOR

        public static void ChangeAssetName(Object asset, string name)
        {
            var assetPath = AssetDatabase.GetAssetPath(asset.GetInstanceID());
            asset.name = name;
            AssetDatabase.RenameAsset(assetPath, name);
            AssetDatabase.SaveAssets();
        }

        public static T[] FindAssetAtFolder<T>(params string[] paths) where T: Object
        {
            var list = new List<T>();
            var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", paths);
            foreach (var guid in guids)
            {
                var asset = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
                if (asset)
                {
                    list.Add(asset);
                }
            }

            return list.ToArray();
        }

        public static string GetAssetFolderPath(Object asset)
        {
            return AssetDatabase.GetAssetPath(asset).Replace($"/{asset.name}.asset", "");
        }

#endif
    }
}
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using Object = UnityEngine.Object;
using Debug = UnityEngine.Debug;

namespace Dajunctic
{
    [InitializeOnLoad]
    public static class GuidReferenceHelper
    {
        static Dictionary<Type, List<string>> _ids;
        static Dictionary<Type, List<string>> _guids;
        static Dictionary<string, Object> _assetMap;

        public static Dictionary<Type, List<string>> Ids
        {
            get => _ids;
        }

        public static Dictionary<Type, List<string>> Guids
        {
            get => _guids;
        }

        public static Dictionary<string, Object> AssetMap
        {
            get => _assetMap;
        }

        static bool _initialize;

        static GuidReferenceHelper()
        {
            EditorApplication.update += HandleUpdate;
        }

        static void HandleUpdate()
        {
            if (!_initialize)
            {
                Refresh();
                _initialize = true;
            }
        
        }

        [MenuItem("Panthera/RefreshIds")]
        public static void Refresh()
        {
            _ids = new Dictionary<Type, List<string>>();
            _guids = new Dictionary<Type, List<string>>();
            _assetMap = new Dictionary<string, Object>();

            LoadAssetIds();
            LoadDummyIds();
            CheckDuplicateIds();

            // Debug.Log("<color=cyan>[Refresh Ids]</color> <color=green>Done!</color>");
        }

        public static void LoadAssetIds()
        {
            var assetGuids = AssetDatabase.FindAssets($"t:{typeof(AssetId).FullName}").Distinct().ToArray();

            foreach (var guid in assetGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);

                var assets = new List<AssetId>(
                    AssetDatabase.LoadAllAssetsAtPath(path).OfType<AssetId>()
                );

                foreach (var asset in assets)
                {
                    var id = asset.Id;

                    var types = asset.GetType().GetInterfaces()
                        .Where(t => typeof(IAssetId).IsAssignableFrom(t));

                    foreach (var type in types)
                    {
                        if (!_ids.TryGetValue(type, out var idList))
                        {
                            idList = new List<string>();
                            _ids[type] = idList;
                        }
                        idList.Add(id);

                        if (!_guids.TryGetValue(type, out var guidList))
                        {
                            guidList = new List<string>();
                            _guids[type] = guidList;
                        }
                        guidList.Add(guid);
                    }

                    if (!_assetMap.TryAdd(id, asset))
                    {
                        _assetMap[id] = asset;
                    }
                }
            }
        }

        public static void LoadDummyIds()
        {
            var assetGuids = AssetDatabase.FindAssets($"t:{typeof(IdDatabase).FullName}").Distinct().ToArray();

            foreach (var guid in assetGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var providers = new List<IdDatabase>(
                    AssetDatabase.LoadAllAssetsAtPath(path).OfType<IdDatabase>()
                );

                foreach (var provider in providers)
                {
                    var ids = provider.GetDummyIds();

                    var fakeGuids = Enumerable.Repeat("", ids.Count).ToList();
                    var dummyType = typeof(IDummyId);

                    if (!_ids.TryAdd(dummyType, ids))
                    {
                        _ids[dummyType].AddRange(ids);
                    }
                    if (!_guids.TryAdd(dummyType, fakeGuids))
                    {
                        _guids[dummyType].AddRange(fakeGuids);
                    }

                    foreach (var id in ids)
                    {
                        if (!_assetMap.TryAdd(id, provider))
                        {
                            _assetMap[id] = provider;
                        }
                    }

                    
                }
            }
        }

        public static List<string> GetIds()
        {
            var ids = new List<string>();
            foreach (var list in _ids.Values)
            {
                ids.AddRange(list);
            }
            return ids;
        }

        public static List<string> GetIds(string prefix, Type[] entityType)
        {
            var ids = new List<string>();
            foreach (var type in entityType)
            {
                if (_ids.TryGetValue(type, out var list))
                {
                    ids.AddRange(list.Where(x => x.StartsWith(prefix)));
                }
            }
            return ids;
        }

        public static Type[] GetTypes()
        {
            return _ids.Keys.ToArray();
        }

        public static void CheckDuplicateIds()
        {
            var idToAssetsMap = new Dictionary<string, List<Object>>();

            // Collect all AssetIds
            var assetGuids = AssetDatabase.FindAssets($"t:{typeof(AssetId).FullName}").Distinct().ToArray();
            foreach (var guid in assetGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var assets = AssetDatabase.LoadAllAssetsAtPath(path).OfType<AssetId>();

                foreach (var asset in assets)
                {
                    var id = asset.Id;
                    if (string.IsNullOrEmpty(id))
                    {
                        Debug.LogError($"Asset '{asset.name}' of type {asset.GetType().Name} has a null or empty ID.", asset);
                        continue;
                    }

                    if (!idToAssetsMap.TryGetValue(id, out var assetList))
                    {
                        assetList = new List<Object>();
                        idToAssetsMap[id] = assetList;
                    }
                    assetList.Add(asset);
                }
            }

            // Collect all DummyIds
            var dbGuids = AssetDatabase.FindAssets($"t:{typeof(IdDatabase).FullName}").Distinct().ToArray();
            foreach (var guid in dbGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var providers = AssetDatabase.LoadAllAssetsAtPath(path).OfType<IdDatabase>();

                foreach (var provider in providers)
                {
                    var ids = provider.GetDummyIds();
                    foreach (var id in ids)
                    {
                        if (string.IsNullOrEmpty(id)) continue;

                        if (!idToAssetsMap.TryGetValue(id, out var assetList))
                        {
                            assetList = new List<Object>();
                            idToAssetsMap[id] = assetList;
                        }
                        assetList.Add(provider);
                    }
                }
            }

            // Report duplicates
            foreach (var kvp in idToAssetsMap.Where(kvp => kvp.Value.Count > 1))
            {
                var distinctAssets = kvp.Value.Distinct().ToList();
                if (distinctAssets.Count > 1)
                {
                    var assetNames = string.Join(", ", distinctAssets.Select(asset => $"'{asset.name}' ({asset.GetType().Name})"));
                    Debug.LogError($"Duplicate ID found: '{kvp.Key}'. This ID is used by multiple assets: {assetNames}. Please ensure all IDs are unique.", distinctAssets.First());
                }
            }
        }
    }
}
#endif
using UnityEngine;
using System.Collections.Generic;

namespace Dajunctic
{
    public static class GameObjectExtension
    {
        /// <summary>
        /// Cache component to avoid repeated GetComponent calls.
        /// </summary>
        public static T GetAndCacheComponent<T>(this GameObject go, ref T cache) where T : Component
        {
            if (cache == null)
                cache = go.GetComponent<T>();
            return cache;
        }

        /// <summary>
        /// Enable/Disable a list of GameObjects.
        /// </summary>
        public static void SetActive(this List<GameObject> list, bool active)
        {
            foreach (var go in list)
                if (go != null) go.SetActive(active);
        }

        /// <summary>
        /// Try get component safely, return null if not found.
        /// </summary>
        public static T TryGet<T>(this GameObject go) where T : Component
        {
            return go.TryGetComponent<T>(out var comp) ? comp : null;
        }
    }
}
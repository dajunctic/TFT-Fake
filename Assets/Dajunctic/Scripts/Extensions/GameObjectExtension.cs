using UnityEngine;
using System.Collections.Generic;

namespace Dajunctic
{
    public static class GameObjectExtension
    {

        public static T GetAndCacheComponent<T>(this GameObject go, ref T cache) where T : Component
        {
            if (cache == null)
                cache = go.GetComponent<T>();
            return cache;
        }

        public static void SetActive(this List<GameObject> list, bool active)
        {
            foreach (var go in list)
                if (go != null) go.SetActive(active);
        }

        public static T TryGet<T>(this GameObject go) where T : Component
        {
            return go.TryGetComponent<T>(out var comp) ? comp : null;
        }
    }
}

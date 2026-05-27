using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic
{
    public static class ListExtension
    {

        public static T GetRandom<T>(this List<T> list)
        {
            if (list == null || list.Count == 0) return default;
            return list[Random.Range(0, list.Count)];
        }

        public static void Shuffle<T>(this List<T> list)
        {
            int n = list.Count;
            for (int i = 0; i < n - 1; i++)
            {
                int j = Random.Range(i, n);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        public static T GetOrDefault<T>(this List<T> list, int index)
        {
            if (index < 0 || index >= list.Count) return default;
            return list[index];
        }

        public static void TryRemove<T>(this List<T> list, T item)
        {
            if (list.Contains(item)) list.Remove(item);
        }

        public static void AddUnique<T>(this List<T> list, T item)
        {
            if (!list.Contains(item)) list.Add(item);
        }
    }
}

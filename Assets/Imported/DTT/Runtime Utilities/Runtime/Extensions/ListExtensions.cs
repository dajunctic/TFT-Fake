using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace DTT.Utils.Extensions
{
    /// <summary>
    /// Provides extensions methods for classes that implement the <see cref="IList{T}"/> interface.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Swaps values at 'first' index with value at 'second' index.
        /// </summary>
        /// <param name="list">The list to swap values of.</param>
        /// <param name="firstIndex">The first index.</param>
        /// <param name="secondIndex">The second index.</param>
        /// <typeparam name="T">The type of list.</typeparam>
        public static void Swap<T>(this IList<T> list, int firstIndex, int secondIndex)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count < 2)
                throw new ArgumentException("List count should be at least 2 for a swap.");
            
            T firstValue = list[firstIndex];
            
            list[firstIndex] = list[secondIndex];
            list[secondIndex] = firstValue;
        }

        /// <summary>
        /// Shuffles the list using the Fisher-Yates algorithm.
        /// </summary>
        /// <param name="list">The list to shuffle.</param>
        /// <typeparam name="T">The type of list.</typeparam>
        public static void Shuffle<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int randomIndex = Random.Range(i, list.Count);
                Swap(list, randomIndex, i);
            }
        }
        
        /// <summary>
        /// Shuffles the list using the Fisher-Yates algorithm.
        /// </summary>
        /// <param name="list">The list to shuffle.</param>
        /// <param name="seed">The seed to use for the random shuffle.</param>
        /// <typeparam name="T">The type of list.</typeparam>
        public static void Shuffle<T>(this IList<T> list, int seed)
        {
            var state = Random.state;
            Random.InitState(seed);
            
            Shuffle(list);

            Random.state = state;
        }

        /// <summary>
        /// Moves all items of a list to the left.
        /// </summary>
        /// <param name="list">The list to rotate.</param>
        /// <param name="count">The amount of times to move to the left.</param>
        /// <typeparam name="T">The type of list.</typeparam>
        public static void RotateLeft<T>(this IList<T> list, int count = 1)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count < 2)
                return;

            for (int current = 0; current < count; current++)
            {
                T first = list[0];
                list.RemoveAt(0);
                list.Add(first);
            }
        }

        /// <summary>
        /// Moves all items of a list to the right.
        /// </summary>
        /// <param name="list">The list to rotate.</param>
        /// <param name="count">The amount of times to move to the right.</param>
        /// <typeparam name="T">The type of list.</typeparam>
        public static void RotateRight<T>(this IList<T> list, int count = 1)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.Count < 2)
                return;

            int lastIndex = list.Count - 1;
            for (int current = 0; current < count; current++)
            {
                T last = list[lastIndex];
                list.RemoveAt(lastIndex);
                list.Insert(0, last);
            }
        }

        /// <summary>
        /// Removes null entries from a list.
        /// </summary>
        /// <typeparam name="T">The type of list.</typeparam>
        /// <param name="list">The list to remove null entries from.</param>
        public static void RemoveNullEntries<T>(this IList<T> list) where T : class
        {
            for (int i = list.Count - 1; i >= 0; i--)
                if (Equals(list[i], null))
                    list.RemoveAt(i);
        }

        /// <summary>
        /// Removes default values from a list.
        /// </summary>
        /// <typeparam name="T">The type of list.</typeparam>
        /// <param name="list">The list to remove default values from.</param>
        public static void RemoveDefaultValues<T>(this IList<T> list)
        {
            for (int i = list.Count - 1; i >= 0; i--)
                if (Equals(default(T), list[i]))
                    list.RemoveAt(i);
        }

        /// <summary>
        /// Returns whether an index is inside the bounds of the list.
        /// </summary>
        /// <typeparam name="T">The type of list to check the bounds of.</typeparam>
        /// <param name="list">The list to check the bounds of.</param>
        /// <param name="index">The index to check.</param>
        /// <returns>Whether the index is inside the bounds.</returns>
        public static bool HasIndex<T>(this IList<T> list, int index) => index.InRange(0, list.Count - 1);
    }
}
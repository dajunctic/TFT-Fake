using System.Collections.Generic;
using System.Linq;

namespace DTT.Utils.Extensions
{
    /// <summary>
    /// Provides extension methods for objects that implement the <see cref="IEnumerable{T}"/> interface.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Returns whether the enumerable is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of enumerable.</typeparam>
        /// <param name="enumerable">The enumerable to check.</param>
        /// <returns>Whether the enumerable is null or empty.</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) => enumerable == null || !enumerable.Any();
    }
}
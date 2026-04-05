namespace DTT.Utils.Extensions
{
    /// <summary>
    /// Provides extension methods for arrays.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Returns whether an index is inside the bounds of the array.
        /// </summary>
        /// <typeparam name="T">The type of array to check the bounds of.</typeparam>
        /// <param name="array">The array to check the bounds of.</param>
        /// <param name="index">The index to check.</param>
        /// <returns>Whether the index is inside the bounds.</returns>
        public static bool HasIndex<T>(this T[] array, int index) => index.InRange(0, array.Length - 1);
    }
}

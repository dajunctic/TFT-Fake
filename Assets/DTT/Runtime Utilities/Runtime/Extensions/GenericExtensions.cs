using System;

namespace DTT.Utils.Extensions
{
    /// <summary>
    /// Provides generic extension methods.
    /// </summary>
    public static class GenericExtensions
    {
        /// <summary>
        /// Checks a value by throwing an exception if it is equal to a faulty one.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="value">The value to check.</param>
        /// <param name="faultyValue">The value that should be excepted.</param>
        /// <param name="onCatch">The exception to throw if the catch failed.</param>
        /// <returns>The caught value.</returns>
        public static T ThrowIf<T>(this T value, T faultyValue, Exception onCatch = null)
        {
            if (value.Equals(faultyValue))
                throw onCatch ?? new InvalidOperationException($"Value {value} was faulty.");

            return value;
        }

        /// <summary>
        /// Replaces a value with a default value if it is equal to a faulty one.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="value">The value to check.</param>
        /// <param name="faultyValue">The value that should be excepted.</param>
        /// <param name="defaultValue">The default value to use as replacement.</param>
        /// <returns>The caught value.</returns>
        public static T ReplaceIf<T>(this T value, T faultyValue, T defaultValue = default(T)) => value.Equals(faultyValue) ? defaultValue : value;
    }
}
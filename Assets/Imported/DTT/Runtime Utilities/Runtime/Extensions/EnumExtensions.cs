using System;
using System.Linq;
using UnityEngine;

namespace DTT.Utils.Extensions
{
    /// <summary>
    /// Provides extensions methods for working with enumerations.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns the inspector name of an enum value. This method uses
        /// reflection internally so make sure to cache its result.
        /// </summary>
        /// <param name="enumValue">The enum value to get the inspector name of.</param>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <returns>The inspector name.</returns>
        public static string GetInspectorName<T>(this T enumValue) where T : Enum
        {
            if (enumValue == null)
                throw new ArgumentNullException(nameof(enumValue));
            
            // Get the attributes of the enum value.
            Type enumType = typeof(T);
            object[] attributes = enumType.GetMember(enumValue.ToString())
                .First(info => info.DeclaringType == enumType)
                .GetCustomAttributes(typeof(InspectorNameAttribute), false);

            if (attributes.Length == 0)
                throw new InvalidOperationException($"No attributes where found on the enum value {enumValue}.");

            // Return the display name stored by the first inspector name found on the enum value.
            return attributes.Cast<InspectorNameAttribute>().First().displayName;
        }
        
        /// <summary>
        /// Returns the next value in the enum value sequence. 
        /// Will loop back to the first value if the value is 
        /// the last.
        /// </summary>
        /// <typeparam name="T">The type of enum.</typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>The next value in the enum value sequence.</returns>
        public static T Next<T>(this T enumValue) where T : Enum
        {
            T[] array = (T[])Enum.GetValues(typeof(T));
            int i = Array.IndexOf(array, enumValue) + 1;
            return (i >= array.Length) ? array[0] : array[i];
        }

        /// <summary>
        /// Returns the previous value in the enum value sequence. 
        /// Will loop to the last value if the value is the first.
        /// </summary>
        /// <typeparam name="T">The type of enum.</typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>The previous value in the enum value sequence.</returns>
        public static T Previous<T>(this T enumValue) where T : Enum
        {
            T[] array = (T[])Enum.GetValues(typeof(T));
            int i = Array.IndexOf(array, enumValue) - 1;
            return (i < 0) ? array[array.Length - 1] : array[i];
        }

        /// <summary>
        /// Returns the underlying character value.
        /// </summary>
        /// <param name="enumValue">The enum value to get the underlying character value of.</param>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <returns>The underlying character value.</returns>
        public static char ToChar<T>(this T enumValue) where T : Enum
        {
            if (enumValue == null)
                throw new ArgumentNullException(nameof(enumValue));
            
            if (!typeof(char).IsAssignableFrom(Enum.GetUnderlyingType(typeof(T))))
                throw new ArgumentException("Underlying type of enum value isn't char.");

            return (char)(object)enumValue;
        }

        /// <summary>
        /// Returns the underlying byte value.
        /// </summary>
        /// <param name="enumValue">The enum value to get the underlying byte value of.</param>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <returns>The underlying byte value.</returns>
        public static byte ToByte<T>(this T enumValue) where T : Enum
        {
            if (enumValue == null)
                throw new ArgumentNullException(nameof(enumValue));
            
            if (!typeof(byte).IsAssignableFrom(Enum.GetUnderlyingType(typeof(T))))
                throw new ArgumentException("Underlying type of enum value isn't byte.");

            return (byte)(object)enumValue;
        }

        /// <summary>
        /// Returns the underlying integer value.
        /// </summary>
        /// <param name="enumValue">The enum value to get the underlying integer value of.</param>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <returns>The underlying integer value.</returns>
        public static int ToInt<T>(this T enumValue) where T : Enum
        {
            if (enumValue == null)
                throw new ArgumentNullException(nameof(enumValue));
            
            if (!typeof(int).IsAssignableFrom(Enum.GetUnderlyingType(typeof(T))))
                throw new ArgumentException("Underlying type of enum value isn't int.");

            return (int)(object)enumValue;
        }
    }
}

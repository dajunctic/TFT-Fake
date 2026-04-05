using UnityEngine;

namespace DTT.Utils.Extensions
{
    /// <summary>
    /// Provides extension methods for color values.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Converts the given <see cref="Color"/> to a hex <see cref="uint"/> value.
        /// Would turn <see cref="Color.red"/> to <c>0xffff0000</c> (the first two values being the alpha channel).
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <returns>The hexadecimal representation of a <see cref="Color"/> object.</returns>
        public static uint ToHex(this Color color)
        {
            uint r = (uint)(color.r * 255) << 24;
            uint g = (uint)(color.g * 255) << 16;
            uint b = (uint)(color.b * 255) << 8;
            uint a = (uint)(color.a * 255);
            return r + g + b + a;
        }
    }
}
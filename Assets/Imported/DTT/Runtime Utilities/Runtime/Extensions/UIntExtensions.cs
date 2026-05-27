using UnityEngine;

namespace DTT.Utils.Extensions
{
    /// <summary>
    /// Provides extension methods for uint values.
    /// </summary>
    public static class UIntExtensions
    {
        /// <summary>
        /// Converts <see cref="uint"/> into a Unity color. Expects bits to be placed rgba order.
        /// </summary>
        /// <param name="hex">Hex number to convert.</param>
        /// <returns>Unity color based on the <see cref="uint"/> value.</returns>
        public static Color ToColor(this uint hex)
        {
            Color color;
            color.r = ((hex & 0xff000000) >> 24)  / 255f;
            color.g = ((hex & 0x00ff0000) >> 16) / 255f;
            color.b = ((hex & 0x0000ff00) >> 8) / 255f;
            color.a = ( hex & 0x000000ff)      / 255f;
            return color;
        }
    }
}

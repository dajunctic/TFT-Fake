using System;
using System.Text;
using System.Text.RegularExpressions;
using DTT.Utils.Workflow;
using UnityEngine;

namespace DTT.Utils.Extensions
{
    /// <summary>
    /// Contains helper methods for adding rich text to for example debug logs.
    /// </summary>
    public static class RichTextExtensions
    {
        /// <summary>
        /// Renders the text in boldface.
        /// </summary>
        /// <param name="input">Text to render in boldface.</param>
        /// <returns>Text rendered in boldface.</returns>
        public static string Bold(this string input)
            => WrapAround("<b>", input, "</b>");

        /// <summary>
        /// Renders the text in italics.
        /// </summary>
        /// <param name="input">Text to render in italics.</param>
        /// <returns>Text rendered in italics.</returns>
        public static string Italics(this string input)
            => WrapAround("<i>", input, "</i>");

        /// <summary>
        /// Sets the size of the text according to the parameter value, given in pixels.
        /// Although this tag is available for console logging, 
        /// you will find that the line spacing in the window bar 
        /// and Console looks strange if the size is set too large.
        /// </summary>
        /// <param name="input">Text to apply sizing to.</param>
        /// <param name="size">The size for the text, given in pixels.</param>
        /// <returns>Text that has been sized.</returns>
        public static string Size(this string input, int size)
            => WrapAround($"<size={size}>", input, "</size>");

        /// <summary>
        /// Sets the color of the text according to the parameter value.
        /// </summary>
        /// <param name="input">Text to set the color of.</param>
        /// <param name="color">The color to set the text to.</param>
        /// <returns>Text that a color has been applied to.</returns>
        public static string Color(this string input, Color color)
            => WrapAround($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>", input, "</color>");

        /// <summary>
        /// Sets the color of the text according to the parameter value.
        /// </summary>
        /// <param name="input">Text to set the color of.</param>
        /// <param name="color">The color type to set the text to.</param>
        /// <returns>Text that a color has been applied to.</returns>
        public static string Color(this string input, RichTextColor color)
            => WrapAround($"<color={color}>", input, "</color>");

        /// <summary>
        /// Sets the color of the text according to the parameter value.
        /// </summary>
        /// <param name="input">Text to set the color of.</param>
        /// <param name="hexColor">The color type to set the text to.</param>
        /// <returns>Text that a color has been applied to.</returns>
        public static string Color(this string input, string hexColor)
        {
            StringBuilder sb = new StringBuilder(hexColor);
            if (sb[0] != '#')
                sb.Insert(0, '#');

            string newHex = sb.ToString();
            if (!StringUtility.IsHexadecimal(newHex))
                throw new ArgumentException(nameof(hexColor), $"Passed hex color string isn't a valid notation. Passed value: {hexColor}");

            return WrapAround($"<color={newHex}>", input, "</color>");
        }

        /// <summary>
        /// Wraps a start and end element around an input and returns the result.
        /// Contains additional behavior for returning just the input based on the platform.
        /// This can help with reducing rich text which doesn't get formatted in build logs.
        /// </summary>
        /// <param name="input">The string to elements around.</param>
        /// <param name="startElement">The first element.</param>
        /// <param name="endElement">The last element.</param>
        /// <returns>The new string with elements wrapped around the input.</returns>
        private static string WrapAround(string startElement, string input, string endElement)
            => string.Join(string.Empty, startElement, input, endElement);
    }
}

using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DTT.Utils.Extensions
{
    /// <summary>
    /// A static class providing extension methods for handling strings.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// The regex used for stripping html tags from strings.
        /// </summary>
        public static readonly Regex htmlRegex = new Regex(@"<.*?>", RegexOptions.Compiled);

        /// <summary>
        /// Prefixes used for private members.
        /// </summary>
        public static readonly string[] privateMemberPrefixes = new string[]
        {
            "_",
            "m_"
        };

        /// <summary>
        /// Returns the display name for a field removing for example an underscore (_) from the name.
        /// </summary>
        /// <param name="fieldName">The field name to convert.</param>
        /// <returns>The display name.</returns>
        public static string ToDisplayName(this string fieldName)
        {
            StringBuilder stringBuilder = new StringBuilder(fieldName);
            for (int i = 0; i < privateMemberPrefixes.Length; i++)
            {
                if (fieldName.StartsWith(privateMemberPrefixes[i]))
                {
                    stringBuilder.Remove(0, privateMemberPrefixes[i].Length);
                    break;
                }
            }

            stringBuilder[0] = char.ToUpper(stringBuilder[0]);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Returns the string with spaces before capitals.
        /// </summary>
        /// <param name="content">The string to base the new string on.</param>
        /// <returns>The string with spaces before capitals.</returns>
        public static string AddSpacesBeforeCapitals(this string content)
        {
            if (content == null)
                return null;

            StringBuilder sb = new StringBuilder(content);

            for (int i = sb.Length - 2; i >= 1; i--)
                if (char.IsUpper(sb[i]) && char.IsLower(sb[i + 1]) && sb[i - 1] != ' ')
                    sb.Insert(i, " ");

            return sb.ToString();
        }

        /// <summary>
        /// Converts a string of constant styling (MY_CONSTANT) to a 
        /// readable format (My Constant).
        /// </summary>
        /// <param name="content">The content to convert.</param>
        /// <returns>The converted string.</returns>
        public static string FromAllCapsToReadableFormat(this string content)
        {
            if (string.IsNullOrEmpty(content))
                return content;

            content = content.ToLower();
            string[] separate = content.Split('_');
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < separate.Length; i++)
            {
                separate[i] = separate[i][0].ToString().ToUpper() + separate[i].Substring(1);
                sb.Append(separate[i]);
                if (i != separate.Length - 1)
                    sb.Append(' ');
            }
            return sb.ToString();
        }

        /// <summary>
        /// Converts a string of readable format (My Constant) to a 
        /// constant styling (MY_CONSTANT).
        /// </summary>
        /// <param name="content">The content to convert.</param>
        /// <returns>The converted string.</returns>
        public static string FromReadableFormatToAllCaps(this string content)
        {
            if (string.IsNullOrEmpty(content))
                return content;

            return content.ToUpper().Replace(' ', '_'); ;
        }

        /// <summary>
        /// Returns the index of the 'nth' appearance of a string. 
        /// </summary>
        /// <param name="string">The value in which the string appears.</param>
        /// <param name="value">The string.</param>
        /// <param name="nth">The number of finds after which to stop.</param>
        /// <returns>The index of the 'nth' appearance.</returns>
        public static int IndexOfNth(this string @string, string value, int nth)
        {
            if (nth < 0)
                throw new ArgumentOutOfRangeException(nameof(nth));

            if (@string == null)
                throw new ArgumentNullException(nameof(@string));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            int offset = @string.IndexOf(value, StringComparison.Ordinal);
            for (int i = 0; i < nth - 1; i++)
            {
                if (offset == -1)
                    return -1;

                offset = @string.IndexOf(value, offset + 1, StringComparison.Ordinal);
            }

            return offset;
        }

        /// <summary>
        /// Returns the index of the 'nth' appearance of a character. 
        /// </summary>
        /// <param name="string">The value in which the character appears.</param>
        /// <param name="value">The character.</param>
        /// <param name="nth">The number of finds after which to stop.</param>
        /// <returns>The index of the 'nth' appearance.</returns>
        public static int IndexOfNth(this string @string, char value, int nth) => IndexOfNth(@string, char.ToString(value), nth);

        /// <summary>
        /// Returns whether the string corresponds with a valid guid.
        /// </summary>
        /// <param name="string">The string to check.</param>
        /// <returns>Whether the string corresponds with a valid guid.</returns>
        public static bool IsValidGuid(this string @string)
        {
            if (@string == null)
                throw new ArgumentNullException(nameof(@string));

            return Guid.TryParse(@string, out _);
        }

        /// <summary>
        /// Returns whether the string corresponds with a valid guid with exact format.
        /// </summary>
        /// <param name="string">The string to check.</param>
        /// <param name="format">The format to check.</param>
        /// <returns>Whether the string corresponds with a valid guid.</returns>
        public static bool IsValidGuid(this string @string, string format)
        {
            if (@string == null)
                throw new ArgumentNullException(nameof(@string));

            if (format == null)
                throw new ArgumentNullException(nameof(format));

            return Guid.TryParseExact(@string, format, out _);
        }

        /// <summary>
        /// Strips html tags from a string.
        /// </summary>
        /// <param name="text">The text string to remove html tags from.</param>
        /// <returns>The text stripped from tags.</returns>
        public static string StripHtmlTags(this string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            
            return htmlRegex.Replace(text, string.Empty);
        }

        /// <summary>
        /// Returns string with ellipsis characters at the end if the input string 
        /// its width is greater than the given max width value.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="maxWidth">The maximum width of the string.</param>
        /// <param name="font">The font that is used.</param>
        public static string Ellipsis(this string input, int maxWidth, Font font) => Ellipsis(input, maxWidth, '.', font);

        /// <summary>
        /// Returns string with ellipsis characters at the end if the input string 
        /// its width is greater than the given max width value.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="maxWidth">The maximum width of the string.</param>
        /// <param name="ellipsisChar">The ellipsis character that is used.</param>
        /// <param name="font">The font that is used.</param>
        public static string Ellipsis(this string input, int maxWidth, char ellipsisChar, Font font, int characterCount = 3)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (font == null)
                throw new ArgumentNullException(nameof(font));

            char[] chars = input.ToCharArray();
            int totalLength = 0;
            CharacterInfo info;
            string text = input;
            for (int j = 0; j < chars.Length; j++)
            {
                font.GetCharacterInfo(chars[j], out info);
                totalLength += info.advance;
                if (totalLength > maxWidth)
                {
                    text = text.Substring(0, Mathf.Max(j - characterCount, 0));
                    text += new string(Enumerable.Repeat(ellipsisChar, characterCount).ToArray());
                    break;
                }
            }

            return text;
        }
    }
}
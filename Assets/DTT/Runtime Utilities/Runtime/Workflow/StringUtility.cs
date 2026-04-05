using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Random = UnityEngine.Random;

namespace DTT.Utils.Workflow
{
    /// <summary>
    /// Provides utility methods for strings.
    /// </summary>
    public static class StringUtility
    {
        /// <summary>
        /// The regular expression used for determining whether a string is a valid web url.
        /// </summary>
        public static readonly Regex  webUrlRegex = new Regex(@"((www.?)|(https:\/\/))[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)", RegexOptions.Compiled);

        /// <summary>
        /// The regular expression used for determining whether a string is a valid hexadecimal.
        /// </summary>
        public static readonly Regex hexadecimalRegex = new Regex(@"^#([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$", RegexOptions.Compiled);
        
        /// <summary>
        /// The regular expression used for determining whether a string corresponds with a variable name 
        /// usable in the CSharp programming language.
        /// </summary>
        public static readonly Regex variableRegex = new Regex(@"^[a-zA-Z0-9_]+$", RegexOptions.Compiled);
        
        /// <summary>
        /// Returns whether the given string is a valid email address.
        /// </summary>
        /// <param name="emailAddress">The email address to check.</param>
        /// <returns>Whether the given string is a valid email address.</returns>
        public static bool IsEmail(string emailAddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailAddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /// <summary>
        /// Returns whether a string is a valid web url.
        /// </summary>
        /// <param name="webUrl">The web url to check.</param>
        /// <returns>Whether the string is a valid web url.</returns>
        public static bool IsWebUrl(string webUrl)
        {
            if (webUrl == null)
                throw new ArgumentNullException(nameof(webUrl));
            
            return webUrlRegex.IsMatch(webUrl);
        }
        
        /// <summary>
        /// Returns whether the string can be used as a valid variable name that doesn't cause compile errors.
        /// Useful for code generation scripts.
        /// </summary>
        /// <param name="variableName">The name to check.</param>
        /// <returns>Whether the name can be used.</returns>
        public static bool IsVariableName(string variableName)
        {
            if (variableName == null)
                throw new ArgumentNullException(nameof(variableName));

            return variableRegex.IsMatch(variableName);
        }

        /// <summary>
        /// Returns whether a string is a valid hexadecimal.
        /// </summary>
        /// <param name="hexadecimalString">The string to check.</param>
        /// <returns>Whether the string is a valid hexadecimal.</returns>
        public static bool IsHexadecimal(string hexadecimalString)
        {
            if (hexadecimalString == null)
                throw new ArgumentNullException(nameof(hexadecimalString));
            
            return hexadecimalRegex.IsMatch(hexadecimalString);
        }

        /// <summary>
        /// Generates a random insecure string which can be used when needing dummy data to test.
        /// </summary>
        /// <param name="length">The length of the string.</param>
        /// <returns>The random insecure string.</returns>
        public static string RandomInsecure(int length) => RandomInsecure(length, null);

        /// <summary>
        /// Generates a random insecure string which can be used when needing dummy data to test.
        /// </summary>
        /// <param name="length">The length of the string.</param>
        /// <param name="seed">The seed to initialize the 'Random' state with.</param>
        /// <returns>The random insecure string.</returns>
        public static string RandomInsecure(int length, int? seed)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            Random.State randomState = Random.state;
            
            if (seed.HasValue)
                Random.InitState(seed.Value);

            const string SELECTION = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            
            char[] result = new char[length];
            for (int i = 0; i < result.Length; i++)
                result[i] = SELECTION[Random.Range(0, SELECTION.Length)];
            
            if(seed.HasValue)
                Random.state = randomState;

            return new string(result);
        }
    }
}
using System;

namespace DTT.Utils.EditorUtilities.Exceptions
{
    /// <summary>
    /// The core exception class for EditorUtility exceptions used in the package.
    /// </summary>
    public abstract class EditorUtilityException : Exception
    {
        #region Variables
        #region Private
        /// <summary>
        /// The prefixed message in front of any
        /// <see cref="EditorUtilityException"/>
        /// </summary>
        private const string PREFIX = "[DTT] - [EditorUtilityException] ";
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Create a <see cref="EditorUtilityException"/> with the given message
        /// to be preceded by the prefix.
        /// <param name="message">The message to show.</param>
        public EditorUtilityException(string message) : base(Format(message)) { }

        /// <summary>
        /// Create a <see cref="EditorUtilityException"/> with the given message
        /// to be preceded by the prefix and inner exception.
        /// </summary>
        /// <param name="message">The message to show.</param>
        /// <param name="innerException">The inner exception thrown.</param>
        public EditorUtilityException(string message, Exception innerException)
            : base(Format(message), innerException)
        {
        }
        #endregion

        #region Methods
        #region Private
        /// <summary>
        /// Returns a formatted version of the given message using the <see cref="PREFIX"/>.
        /// </summary>
        /// <param name="message">The message to be formatted.</param>
        /// <returns>The formatted message</returns>
        private static string Format(string message) => message.Insert(0, PREFIX);
        #endregion
        #endregion
    }
}

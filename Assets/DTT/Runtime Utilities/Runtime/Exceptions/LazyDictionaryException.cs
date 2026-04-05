using System;

namespace DTT.Utils.Exceptions
{
    /// <summary>
    /// Thrown when an interaction the lazy dictionary object went wrong.
    /// </summary>
    public class LazyDictionaryException : RuntimeUtilityException
    {
        #region Variables
        #region Private
        /// <summary>
        /// The prefixed message in front of any
        /// <see cref="LazyDictionaryException"/>
        /// </summary>
        private const string PREFIX = "- [An error occurred during interaction with a lazy dictionary] - ";
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Create a <see cref="LazyDictionaryException"/> with the given message
        /// to be preceded by the prefix.
        /// </summary>
        /// <param name="message">The message to show.</param>
        public LazyDictionaryException(string message) : base(Format(PREFIX, message)) { }

        /// <summary>
        /// Create a <see cref="LazyDictionaryException"/> with the given message
        /// to be preceded by the prefix and inner exception.
        /// </summary>
        /// <param name="message">The message to show.</param>
        /// <param name="innerException">The inner exception thrown.</param>
        public LazyDictionaryException(string message, Exception innerException)
            : base(Format(PREFIX, message), innerException) { }
        #endregion
    }
}
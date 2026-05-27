using System;

namespace DTT.Utils.Exceptions
{
    /// <summary>
    /// The core exception class for RuntimeUtilityException exceptions used in the package.
    /// </summary>
    public abstract class RuntimeUtilityException : Exception
    {
        #region Variables
        #region Private
        /// <summary>
        /// The prefixed message in front of any
        /// <see cref="RuntimeUtilityException"/>
        /// </summary>
        private const string PREFIX = "[DTT] - [RuntimeUtilityException] ";
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Create a <see cref="RuntimeUtilityException"/> with the given message
        /// to be preceded by the prefix.
        /// </summary>
        /// <param name="message">The message to show.</param>
        public RuntimeUtilityException(string message) : base(Format(PREFIX, message)) { }

        /// <summary>
        /// Create a <see cref="RuntimeUtilityException"/> with the given message
        /// to be preceded by the prefix and inner exception.
        /// </summary>
        /// <param name="message">The message to show.</param>
        /// <param name="innerException">The inner exception thrown.</param>
        public RuntimeUtilityException(string message, Exception innerException)
            : base(Format(PREFIX, message), innerException)
        {
        }
        #endregion

        #region Methods
        #region Private
        /// <summary>
        /// Returns a formatted version of the given message using the <see cref="PREFIX"/>.
        /// </summary>
        /// <param name="prefix">The prefix value.</param>
        /// <param name="message">The message to be formatted.</param>
        /// <returns>The formatted message.</returns>
        protected static string Format(string prefix, string message)
            => message.Insert(0, prefix);
        #endregion
        #endregion
    }
}

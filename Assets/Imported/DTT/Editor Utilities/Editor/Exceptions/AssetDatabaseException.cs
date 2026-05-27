using System;

namespace DTT.Utils.EditorUtilities.Exceptions
{
    /// <summary>
    /// Thrown when an Asset databaseInteraction went wrong.
    /// </summary>
    public class AssetDatabaseException : EditorUtilityException
    {
        #region Variables
        #region Private
        /// <summary>
        /// The prefixed message in front of any
        /// <see cref="AssetDatabaseException"/>
        /// </summary>
        private const string PREFIX = "- [An error occured during AssetDatabase interaction] - ";
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Create a <see cref="AssetDatabaseException"/> with the given message
        /// to be preceded by the prefix.
        /// </summary>
        /// <param name="message">The message to show.</param>
        public AssetDatabaseException(string message) : base(Format(message)) { }

        /// <summary>
        /// Create a <see cref="AssetDatabaseException"/> with the given message
        /// to be preceded by the prefix and inner exception.
        /// </summary>
        /// <param name="message">The message to show.</param>
        /// <param name="innerException">The inner exception thrown.</param>
        public AssetDatabaseException(string message, Exception innerException)
            : base(Format(message), innerException) { }
        #endregion

        #region Methods
        #region Private
        /// <summary>
        /// Returns a formatted version of the given message using the <see cref="PREFIX"/>.
        /// </summary>
        /// <param name="message">The message to be formatted.</param>
        /// <returns>The formatted message.</returns>
        private static string Format(string message) => message.Insert(0, PREFIX);
        #endregion
        #endregion
    }
}
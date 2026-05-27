using System;

namespace DTT.Utils.Exceptions
{
    /// <summary>
    /// An exception used for when an argument was null or empty.
    /// </summary>
    public class NullOrEmptyException : RuntimeUtilityException
    {
        #region Variables
        #region Private
        /// <summary>
        /// The prefixed message in front of any
        /// <see cref="RuntimeUtilityException"/>
        /// </summary>
        private const string PREFIX = " - [A value was null or empty] - ";

        /// <summary>
        /// The suffix used to with the argument name.
        /// </summary>
        private const string SUFFIX = " was null or empty.";
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Create a <see cref="RuntimeUtilityException"/> with the given message
        /// to be preceded by the prefix.
        /// <param name="nameOfArgument">The message to show.</param>
        /// </summary>
        public NullOrEmptyException(string nameOfArgument) : base(Format(PREFIX, nameOfArgument + SUFFIX)) { }

        /// <summary>
        /// Create a <see cref="RuntimeUtilityException"/> with the given message
        /// to be preceded by the prefix and inner exception.
        /// </summary>
        /// <param name="nameOfArgument">The message to show.</param>
        /// <param name="innerException">The inner exception thrown.</param>
        public NullOrEmptyException(string nameOfArgument, Exception innerException)
            : base(Format(PREFIX, nameOfArgument + SUFFIX), innerException)
        {
        }
        #endregion
    }
}

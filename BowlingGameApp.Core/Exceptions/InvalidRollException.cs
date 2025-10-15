namespace BowlingGameApp.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when an invalid roll is attempted.
    /// Provides specific error information for bowling game violations.
    /// </summary>
    public class InvalidRollException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the InvalidRollException class.
        /// </summary>
        public InvalidRollException() : base("Invalid roll attempted.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the InvalidRollException class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public InvalidRollException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the InvalidRollException class with a specified error message and inner exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public InvalidRollException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
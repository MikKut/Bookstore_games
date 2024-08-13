namespace BG.LocalWebApp.Common.Exceptions
{
    /// <summary>
    /// Exception thrown for custom validation errors.
    /// </summary>
    public class CustomValidationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomValidationException"/> class with a custom error message.
        /// </summary>
        /// <param name="message">The custom error message.</param>
        public CustomValidationException(string message) : base(message)
        {
        }
    }
}

namespace BG.LocalWebApp.Common.Constants
{
    /// <summary>
    /// Enum to represent different types of error messages.
    /// </summary>
    public enum ErrorMessageType
    {
        Customer,
        Book,
        Author,
        User
    }

    /// <summary>
    /// Class containing error messages and a method to generate not found messages.
    /// </summary>
    public static class ErrorMessages
    {
        /// <summary>
        /// Common format for not found messages with a placeholder for the specific type.
        /// </summary>
        private readonly static string NotFoundFormat = "{0} not found.";

        /// <summary>
        /// Generates a not found message for the specified type.
        /// </summary>
        /// <param name="type">The type of the entity that was not found.</param>
        /// <returns>A formatted not found message.</returns>
        public static string NotFoundMessage(ErrorMessageType type)
        {
            return string.Format(NotFoundFormat, type.ToString());
        }
    }
}

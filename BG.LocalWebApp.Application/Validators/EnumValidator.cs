namespace BG.LocalApi.Application.Validators
{
    /// <summary>
    /// Provides utility methods for validating enum values.
    /// </summary>
    public static class EnumValidator
    {
        /// <summary>
        /// Determines whether a given string value corresponds to a valid member of the specified enum type.
        /// </summary>
        /// <typeparam name="TEnum">The enum type to validate against. Must be a struct and an enum.</typeparam>
        /// <param name="value">The string value to validate.</param>
        /// <returns>
        /// <c>true</c> if the value is a valid member of the specified enum type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidEnum<TEnum>(string value) where TEnum : struct, Enum
        {
            return Enum.TryParse(typeof(TEnum), value, true, out _);
        }
    }

}

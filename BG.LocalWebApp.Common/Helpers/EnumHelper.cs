using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.LocalWebApp.Common.Helpers
{
    /// <summary>
    /// Provides helper methods for working with enumerations.
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Determines whether a string value is a valid name or value for a given enumeration type.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type to check against.</typeparam>
        /// <param name="value">The string value to check.</param>
        /// <returns>
        /// <c>true</c> if the string value is a valid name or value for the specified enumeration type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidEnum<TEnum>(string value) where TEnum : struct, Enum
        {
            return Enum.TryParse(typeof(TEnum), value, true, out _);
        }

        /// <summary>
        /// Converts a string value to its corresponding enumeration value.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type to convert to.</typeparam>
        /// <param name="value">The string value to convert.</param>
        /// <returns>
        /// The enumeration value corresponding to the specified string value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the string value is null, whitespace, or not a valid value for the specified enumeration type.
        /// </exception>
        public static TEnum GetEnumValue<TEnum>(string? value) where TEnum : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));

            if (Enum.TryParse<TEnum>(value, true, out var result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException($"'{value}' is not a valid value for enum {typeof(TEnum).Name}.");
            }
        }
    }
}

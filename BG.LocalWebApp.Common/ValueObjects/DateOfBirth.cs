using System.ComponentModel.DataAnnotations.Schema;

namespace BG.LocalWebApp.Common.ValueObjects
{
    // <summary>
    /// Represents a date of birth value.
    /// </summary>
    public class DateOfBirth
    {
        /// <summary>
        /// Gets the value of the date of birth.
        /// </summary>
        [Column(TypeName = "date")]
        public DateTime Value { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateOfBirth"/> class.
        /// </summary>
        /// <param name="value">The date of birth value. Must not be in the future.</param>
        /// <exception cref="ArgumentException">Thrown when the date of birth is in the future.</exception>
        public DateOfBirth(DateTime value)
        {
            if (value > DateTime.Now)
                throw new ArgumentException("Date of birth cannot be in the future.", nameof(value));

            Value = value;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="DateOfBirth"/> instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified object is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is not DateOfBirth other)
                return false;

            return Value == other.Value;
        }

        /// <summary>
        /// Serves as a hash function for the <see cref="DateOfBirth"/> type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="DateOfBirth"/> instance.</returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Implicitly converts a <see cref="DateTime"/> to a <see cref="DateOfBirth"/>.
        /// </summary>
        /// <param name="value">The date and time value to convert.</param>
        /// <returns>A <see cref="DateOfBirth"/> instance with the specified value.</returns>
        public static implicit operator DateOfBirth(DateTime value) => new DateOfBirth(value);

        /// <summary>
        /// Implicitly converts a <see cref="DateOfBirth"/> to a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="dob">The <see cref="DateOfBirth"/> instance to convert.</param>
        /// <returns>The <see cref="DateTime"/> value of the <see cref="DateOfBirth"/>.</returns>
        public static implicit operator DateTime(DateOfBirth dob) => dob.Value;
    }
}

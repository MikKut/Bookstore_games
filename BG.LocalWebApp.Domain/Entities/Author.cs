using BG.LocalWebApp.Common.Entities;
using BG.LocalWebApp.Common.ValueObjects;

namespace BG.LocalApi.Domain.Entities
{
    /// <summary>
    /// Represents an author entity with details such as name, date of birth, and a collection of books they have written.
    /// </summary>
    public class Author : BaseEntity
    {
        /// <summary>
        /// Gets or sets the first name of the author.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name of the author.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date of birth of the author.
        /// </summary>
        public DateOfBirth DateOfBirth { get; set; } = default!;
    }
}

using BG.LocalWebApp.Common.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace BG.LocalWeb.Domain.Entities
{
    /// <summary>
    /// Represents a user in the system with identity and personal details.
    /// </summary>
    public class User : IdentityUser<Guid>
    {
        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date of birth of the user.
        /// </summary>
        public DateOfBirth DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the address of the user.
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the username for the user.
        /// </summary>
        public override string UserName { get; set; } = string.Empty;
    }
}
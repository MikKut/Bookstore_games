using BG.LocalWeb.Domain.Entities;

namespace BG.LocalWeb.Domain.Interfaces.Services
{
    /// <summary>
    /// Defines methods for hashing and verifying passwords.
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Hashes the specified password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>The hashed password as a string.</returns>
        string HashPassword(string password);

        /// <summary>
        /// Verifies that a provided password matches a hashed password.
        /// </summary>
        /// <param name="user">The user for whom the password is being verified.</param>
        /// <param name="hashedPassword">The hashed password to compare against.</param>
        /// <param name="providedPassword">The password provided for verification.</param>
        /// <returns><c>true</c> if the provided password matches the hashed password; otherwise, <c>false</c>.</returns>
        bool VerifyHashedPassword(User user, string hashedPassword, string providedPassword);
    }

}

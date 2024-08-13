using BG.LocalWeb.Domain.Entities;
using System.Security.Claims;

namespace BG.LocalWeb.Domain.Interfaces.Services
{
    /// <summary>
    /// Defines methods for generating and validating JWT tokens.
    /// </summary>
    public interface IJwtTokenService
    {
        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom the token will be generated.</param>
        /// <returns>A JWT token as a string.</returns>
        string GenerateToken(User user);

        /// <summary>
        /// Validates a JWT token and returns the associated claims principal.
        /// </summary>
        /// <param name="token">The JWT token to be validated.</param>
        /// <returns>A <see cref="ClaimsPrincipal"/> representing the claims in the token.</returns>
        ClaimsPrincipal ValidateToken(string token);
    }
}

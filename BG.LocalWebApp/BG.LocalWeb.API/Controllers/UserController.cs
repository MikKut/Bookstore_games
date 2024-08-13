using BG.LocalWeb.Application.Common.DTOs.User;
using BG.LocalWeb.Application.Handlers.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BG.LocalWeb.API.Controllers
{
    /// <summary>
    /// Controller responsible for profile info retrieval.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator instance used for dispatching commands and queries.</param>
        public UserController(IMediator mediator, ILogger<UserController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves the profile of the currently authenticated user.
        /// </summary>
        /// <returns>A response containing the user's profile information.</returns>
        /// <response code="200">The profile was successfully retrieved.</response>
        /// <response code="401">The user is not authenticated or the token is invalid.</response>
        /// <response code="404">The user was not found.</response>
        /// <response code="400">An error occurred while retrieving the profile.</response>
        [Authorize]
        [HttpGet("profile")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                _logger.LogInformation("GetProfile method started.");
                var userIdClaim = User.FindFirst(ClaimTypes.Sid)?.Value;
                if (userIdClaim == null)
                {
                    _logger.LogWarning("Invalid token. User claim not found.");
                    return Unauthorized("Invalid token.");
                }

                var userId = Guid.Parse(userIdClaim);
                _logger.LogInformation("Retrieving profile for UserId: {UserId}", userId);

                var result = await _mediator.Send(new GetUserByIdQuery(userId));

                if (result.Data == null)
                {
                    _logger.LogWarning("User not found. UserId: {UserId}", userId);
                    return NotFound("User not found.");
                }

                _logger.LogInformation("Profile retrieved successfully for UserId: {UserId}", userId);
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the profile.");
                return BadRequest("Unexpected error: " + ex.Message + ": " + ex?.InnerException?.Message);
            }
        }
    }
}

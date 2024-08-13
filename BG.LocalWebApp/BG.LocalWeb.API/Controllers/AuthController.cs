using BG.LocalWeb.Application.Common.DTOs.User;
using BG.LocalWeb.Application.Handlers.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BG.LocalWeb.API.Controllers
{
    /// <summary>
    /// Controller responsible for handling authentication-related operations such as registration and login.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator instance used for dispatching commands and queries.</param>
        public AuthController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request">The registration details of the new user.</param>
        /// <returns>A response indicating the result of the registration.</returns>
        /// <response code="200">Registration was successful.</response>
        /// <response code="400">The registration request is invalid or an error occurred.</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] UserCreateDto request)
        {
            try
            {
                _logger.LogInformation("Register method started.");
                var command = new RegisterUserCommand(request);
                var result = await _mediator.Send(command);

                if (result.HasErrors)
                {
                    _logger.LogWarning("Registration failed: {Errors}", string.Join(", ", result.Errors));
                    return BadRequest(result.Errors);
                }

                _logger.LogInformation("User registered successfully: {Username}", request.Username);
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during registration.");
                return BadRequest("Unexpected error: " + ex.Message + ": " + ex?.InnerException?.Message);
            }
        }

        /// <summary>
        /// Logs in an existing user.
        /// </summary>
        /// <param name="request">The login credentials of the user.</param>
        /// <returns>A response containing the authentication token and user details.</returns>
        /// <response code="200">Login was successful, and the token and user details are returned.</response>
        /// <response code="401">The login credentials are invalid.</response>
        /// <response code="400">An error occurred during the login process.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] UserLoginDto request)
        {
            try
            {
                _logger.LogInformation("Login method started for Username: {Username}", request.Username);
                var command = new LoginUserCommand { Username = request.Username, Password = request.Password };
                var result = await _mediator.Send(command);

                if (!result.Succeeded)
                {
                    _logger.LogWarning("Login failed for Username: {Username}", request.Username);
                    return Unauthorized(result.Errors);
                }

                _logger.LogInformation("User logged in successfully: {Username}", request.Username);
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for Username: {Username}", request.Username);
                return BadRequest("Unexpected error: " + ex.Message + ": " + ex?.InnerException?.Message);
            }
        }
    }
}

using BG.LocalApi.Application.Common.DTOs.Author;
using BG.LocalApi.Application.Common.Requests;
using BG.LocalApi.Application.Handlers.Authors.Commands;
using BG.LocalApi.Application.Handlers.Authors.Queries;
using BG.LocalWebApp.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BG.LocalApi.API.Controllers
{
    /// <summary>
    /// Controller to manage author-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthorController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator instance used to send commands and queries.</param>
        public AuthorController(IMediator mediator, ILogger<AuthorController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new author in the system.
        /// </summary>
        /// <param name="command">The command containing the details of the author to be created.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A response containing the newly created author's details.</returns>
        /// <response code="201">The author was successfully created and is returned in the response.</response>
        /// <response code="400">The request is invalid or an error occurred during the creation process.</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(AuthorDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string[]), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Creating a new author: {@Command}", command);
                var result = await _mediator.Send(command, cancellationToken);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Author created successfully: {@AuthorId}", result.Data.Id);
                    return CreatedAtAction(nameof(GetAuthorById), new { id = result.Data.Id }, result.Data);
                }

                _logger.LogWarning("Failed to create author: {@Errors}", result.Errors);
                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the author");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a paginated list of authors based on the provided filter criteria.
        /// </summary>
        /// <param name="request">The filter criteria for retrieving authors, including pagination options.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A response containing a paginated list of authors.</returns>
        /// <response code="200">The list of authors is successfully retrieved.</response>
        /// <response code="400">An error occurred while retrieving the authors.</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<AuthorDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAuthors([FromQuery] AuthorsPagedFilterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Retrieving all authors");
                var result = await _mediator.Send(new GetAllAuthorsQuery(request), cancellationToken);
                _logger.LogInformation("Successfully retrieved authors");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the authors");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the details of a specific author by their ID.
        /// </summary>
        /// <param name="id">The unique identifier of the author.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A response containing the author's details.</returns>
        /// <response code="200">The author was found and returned in the response.</response>
        /// <response code="404">The author with the specified ID was not found.</response>
        /// <response code="400">An error occurred while retrieving the author.</response>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AuthorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAuthorById(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Retrieving author with ID: {AuthorId}", id);
                var result = await _mediator.Send(new GetAuthorByIdQuery(id), cancellationToken);
                if (result == null)
                {
                    _logger.LogWarning("Author with ID {AuthorId} not found", id);
                    return NotFound();
                }

                _logger.LogInformation("Successfully retrieved author with ID: {AuthorId}", id);
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the author with ID: {AuthorId}", id);
                return BadRequest("Unexpected error: " + ex.Message + ": " + ex?.InnerException?.Message);
            }
        }

        /// <summary>
        /// Updates the details of an existing author.
        /// </summary>
        /// <param name="id">The unique identifier of the author to be updated.</param>
        /// <param name="command">The command containing the updated details of the author.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="204">The author was successfully updated.</response>
        /// <response code="400">The IDs do not match or an error occurred during the update.</response>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string[]), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAuthor(Guid id, [FromBody] UpdateAuthorCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (id != command.AuthorId)
                {
                    if (command.AuthorId == Guid.Empty && id != Guid.Empty)
                    {
                        command.AuthorId = id;
                    }
                    else
                    {
                        _logger.LogWarning("Author ID mismatch: URL ID {Id}, Command ID {CommandId}", id, command.AuthorId);
                        return BadRequest("Author ID mismatch");
                    }
                }

                _logger.LogInformation("Updating author with ID: {AuthorId}", id);
                var result = await _mediator.Send(command, cancellationToken);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Author with ID {AuthorId} updated successfully", id);
                    return NoContent();
                }

                _logger.LogWarning("Failed to update author with ID {AuthorId}: {@Errors}", id, result.Errors);
                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the author with ID: {AuthorId}", id);
                return BadRequest("Unexpected error: " + ex.Message + ": " + ex?.InnerException?.Message);
            }
        }

        /// <summary>
        /// Deletes an author by their ID from the system.
        /// </summary>
        /// <param name="id">The unique identifier of the author to be deleted.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="204">The author was successfully deleted.</response>
        /// <response code="400">An error occurred during the deletion process.</response>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string[]), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAuthor(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Deleting author with ID: {AuthorId}", id);
                var result = await _mediator.Send(new DeleteAuthorCommand(id), cancellationToken);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Author with ID {AuthorId} deleted successfully", id);
                    return NoContent();
                }

                _logger.LogWarning("Failed to delete author with ID {AuthorId}: {@Errors}", id, result.Errors);
                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the author with ID: {AuthorId}", id);
                return BadRequest("Unexpected error: " + ex.Message + ": " + ex?.InnerException?.Message);
            }
        }
    }
}

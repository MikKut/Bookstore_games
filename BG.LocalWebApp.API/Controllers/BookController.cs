using BG.LocalApi.Application.Common.DTOs.Book;
using BG.LocalApi.Application.Common.Requests;
using BG.LocalApi.Application.Handlers.Books.Commands;
using BG.LocalApi.Application.Handlers.Books.Queries;
using BG.LocalApi.Domain.Enums;
using BG.LocalWebApp.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BG.LocalApi.API.Controllers
{
    /// <summary>
    /// Controller to manage book-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BookController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator instance used to send commands and queries.</param>
        public BookController(IMediator mediator, ILogger<BookController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new book in the system.
        /// </summary>
        /// <param name="command">The command containing the details of the book to be created.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A response containing the newly created book's details.</returns>
        /// <response code="201">The book was successfully created and is returned in the response.</response>
        /// <response code="400">The request is invalid or an error occurred during the creation process.</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(BookDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string[]), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("CreateBook method started.");
                var result = await _mediator.Send(command, cancellationToken);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Book created successfully with ID: {BookId}", result.Data.Id);
                    return CreatedAtAction(nameof(GetBookById), new { id = result.Data.Id }, result.Data);
                }

                _logger.LogWarning("Failed to create book: {Errors}", string.Join(", ", result.Errors));
                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a book.");
                return BadRequest("Unexpected error: " + ex.Message + ": " + ex?.InnerException?.Message);
            }
        }

        /// <summary>
        /// Retrieves a paginated list of books based on the provided filter.
        /// </summary>
        /// <param name="filterRequest">The filter request containing pagination and search criteria.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> containing a <see cref="PagedResult{BookDto}"/> if successful, 
        /// or a bad request message if an error occurs.
        /// </returns>
        /// <response code="200">Returns a paginated result of books.</response>
        /// <response code="400">Returns a message indicating that an error occurred.</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<BookDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBooks([FromQuery] BooksPagedFilterRequest filterRequest, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("GetBooks method started.");
                var query = new GetAllBooksQuery(filterRequest);
                var result = await _mediator.Send(query, cancellationToken);
                _logger.LogInformation("Retrieved {Count} books", result.TotalCount);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving books.");
                return BadRequest("Unexpected error: " + ex.Message + ": " + ex?.InnerException?.Message);
            }
        }

        /// <summary>
        /// Retrieves the details of a specific book by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the book.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A response containing the book's details.</returns>
        /// <response code="200">The book was found and returned in the response.</response>
        /// <response code="404">The book with the specified ID was not found.</response>
        /// <response code="400">An error occurred while retrieving the book.</response>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BookDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBookById(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("GetBookById method started for ID: {BookId}", id);
                var result = await _mediator.Send(new GetBookByIdQuery(id), cancellationToken);
                if (result.Data == null)
                {
                    _logger.LogWarning("Book with ID {BookId} not found.", id);
                    return NotFound();
                }

                _logger.LogInformation("Book with ID {BookId} retrieved successfully.", id);
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving book with ID {BookId}.", id);
                return BadRequest("Unexpected error: " + ex.Message + ": " + ex?.InnerException?.Message);
            }
        }

        /// <summary>
        /// Updates the details of an existing book.
        /// </summary>
        /// <param name="id">The unique identifier of the book to be updated.</param>
        /// <param name="command">The command containing the updated details of the book.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="204">The book was successfully updated.</response>
        /// <response code="400">The IDs do not match or an error occurred during the update.</response>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateBook(Guid id, [FromBody] UpdateBookCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("UpdateBook method started for ID: {BookId}", id);
                if (id != command.Id)
                {
                    if (command.Id == Guid.Empty && id != Guid.Empty)
                    {
                        command.Id = id;
                    }
                    else
                    {
                        _logger.LogWarning("Book ID mismatch: {BookId}", id);
                        return BadRequest("Book ID mismatch");
                    }
                }

                var result = await _mediator.Send(command, cancellationToken);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Book with ID {BookId} updated successfully.", id);
                    return NoContent();
                }

                _logger.LogWarning("Failed to update book with ID {BookId}: {Errors}", id, string.Join(", ", result.Errors));
                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating book with ID {BookId}.", id);
                return BadRequest("Unexpected error: " + ex.Message + ": " + ex?.InnerException?.Message);
            }
        }

        /// <summary>
        /// Deletes a book by its ID from the system.
        /// </summary>
        /// <param name="id">The unique identifier of the book to be deleted.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="204">The book was successfully deleted.</response>
        /// <response code="400">An error occurred during the deletion process.</response>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteBook(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("DeleteBook method started for ID: {BookId}", id);
                var result = await _mediator.Send(new DeleteBookCommand(id), cancellationToken);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Book with ID {BookId} deleted successfully.", id);
                    return NoContent();
                }

                _logger.LogWarning("Failed to delete book with ID {BookId}: {Errors}", id, string.Join(", ", result.Errors));
                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting book with ID {BookId}.", id);
                return BadRequest("Unexpected error: " + ex.Message + ": " + ex?.InnerException?.Message);
            }
        }

        /// <summary>
        /// Get all possible genres as collection of strings.
        /// </summary>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>An empty response indicating success.</returns>
        /// <response code="400">An error occurred during conversion of the genres.</response>
        [HttpGet("genres")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetGenres(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("GetGenres method started.");
                var genres = Enum.GetValues(typeof(Genre)).Cast<Genre>().Select(g => g.ToString());
                if (genres.IsNullOrEmpty())
                {
                    _logger.LogWarning("Problem with retrieval genres.");
                    return BadRequest("Problem with retrieval genres");
                }

                _logger.LogInformation("Genres retrieved successfully.");
                return Ok(genres);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving genres.");
                return BadRequest("Unexpected error: " + ex.Message + ": " + ex?.InnerException?.Message);
            }
        }
    }
}

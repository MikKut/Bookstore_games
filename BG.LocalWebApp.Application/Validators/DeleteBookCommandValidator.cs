using BG.LocalApi.Application.Handlers.Books.Commands;
using FluentValidation;

namespace BG.LocalApi.Application.Validators
{
    /// <summary>
    /// Validator for <see cref="DeleteBookCommand"/> instances.
    /// </summary>
    /// <remarks>
    /// This class ensures that the data provided in a <see cref="DeleteBookCommand"/> meets the required criteria before processing.
    /// </remarks>
    public class DeleteBookCommandValidator : AbstractValidator<DeleteBookCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteBookCommandValidator"/> class.
        /// </summary>
        /// <remarks>
        /// Configures the validation rule for the <see cref="DeleteBookCommand"/>:
        /// - <see cref="DeleteBookCommand.Id"/>: Must not be empty.
        /// </remarks>
        public DeleteBookCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Book Id is required.");
        }
    }
}

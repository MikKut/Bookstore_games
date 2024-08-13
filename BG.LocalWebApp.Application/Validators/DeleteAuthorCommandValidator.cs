using BG.LocalApi.Application.Handlers.Authors.Commands;
using FluentValidation;

namespace BG.LocalApi.Application.Validators
{
    /// <summary>
    /// Validator for <see cref="DeleteAuthorCommand"/> instances.
    /// </summary>
    /// <remarks>
    /// This class ensures that the data provided in a <see cref="DeleteAuthorCommand"/> meets the required criteria before processing.
    /// </remarks>
    public class DeleteAuthorCommandValidator : AbstractValidator<DeleteAuthorCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteAuthorCommandValidator"/> class.
        /// </summary>
        /// <remarks>
        /// Configures the validation rule for the <see cref="DeleteAuthorCommand"/>:
        /// - <see cref="DeleteAuthorCommand.Id"/>: Must not be empty.
        /// </remarks>
        public DeleteAuthorCommandValidator()
        {
            // Validate that the AuthorId is not empty.
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Author Id is required.");
        }
    }
}

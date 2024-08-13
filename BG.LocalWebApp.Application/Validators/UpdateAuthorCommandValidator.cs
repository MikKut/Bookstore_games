using BG.LocalApi.Application.Handlers.Authors.Commands;
using FluentValidation;

namespace BG.LocalApi.Application.Validators
{
    /// <summary>
    /// Validator for <see cref="UpdateAuthorCommand"/> instances.
    /// </summary>
    /// <remarks>
    /// This class ensures that the data provided in an <see cref="UpdateAuthorCommand"/> meets the required criteria before processing.
    /// </remarks>
    public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateAuthorCommandValidator"/> class.
        /// </summary>
        /// <remarks>
        /// Configures the validation rules for the <see cref="UpdateAuthorCommand"/>:
        /// - <see cref="UpdateAuthorCommand.AuthorId"/>: Must not be empty.
        /// - <see cref="UpdateAuthorCommand.FirstName"/>: Must not be empty and cannot exceed 50 characters.
        /// - <see cref="UpdateAuthorCommand.LastName"/>: Must not be empty and cannot exceed 50 characters.
        /// - <see cref="UpdateAuthorCommand.DateOfBirth"/>: Must not be empty and must be in the past.
        /// </remarks>
        public UpdateAuthorCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required.")
                .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past.");
        }
    }
}

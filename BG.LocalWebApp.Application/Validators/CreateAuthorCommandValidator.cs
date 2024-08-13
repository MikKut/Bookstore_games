using BG.LocalApi.Application.Handlers.Authors.Commands;
using FluentValidation;

namespace BG.LocalApi.Application.Validators
{
    /// <summary>
    /// Validator for <see cref="CreateAuthorCommand"/> instances.
    /// </summary>
    /// <remarks>
    /// This class ensures that the data provided in a <see cref="CreateAuthorCommand"/> instance meets the required validation rules before processing.
    /// </remarks>
    public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAuthorCommandValidator"/> class.
        /// </summary>
        /// <remarks>
        /// Configures the validation rules for the <see cref="CreateAuthorCommand"/>:
        /// - <see cref="CreateAuthorCommand.FirstName"/>: Must not be empty and cannot exceed 50 characters.
        /// - <see cref="CreateAuthorCommand.LastName"/>: Must not be empty and cannot exceed 50 characters.
        /// - <see cref="CreateAuthorCommand.DateOfBirth"/>: Must not be empty and must be a date in the past.
        /// </remarks>
        public CreateAuthorCommandValidator()
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

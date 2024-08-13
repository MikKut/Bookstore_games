using BG.LocalWeb.Application.Handlers.Users.Commands;
using FluentValidation;

namespace BG.LocalWeb.Application.Validators
{
    /// <summary>
    /// Validator for <see cref="LoginUserCommand"/> instances.
    /// </summary>
    /// <remarks>
    /// This class ensures that the data provided in a <see cref="LoginUserCommand"/> meets the required criteria before processing.
    /// </remarks>
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginUserCommandValidator"/> class.
        /// </summary>
        /// <remarks>
        /// Configures the validation rules for the <see cref="LoginUserCommand"/>:
        /// - <see cref="LoginUserCommand.Username"/>: Must not be empty, must be between 3 and 50 characters.
        /// - <see cref="LoginUserCommand.Password"/>: Must not be empty, must be between 6 and 100 characters.
        /// </remarks>
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(100).WithMessage("Password cannot exceed 100 characters.");
        }
    }
}

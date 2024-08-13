using BG.LocalWeb.Application.Handlers.Users.Commands;
using FluentValidation;

namespace BG.LocalWeb.Application.Validators
{
    /// <summary>
    /// Validator for <see cref="RegisterUserCommand"/> instances.
    /// </summary>
    /// <remarks>
    /// This class ensures that the data provided in a <see cref="RegisterUserCommand"/> meets the required criteria before processing.
    /// It delegates the validation of user details to the <see cref="UserCreateDtoValidator"/>.
    /// </remarks>
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterUserCommandValidator"/> class.
        /// </summary>
        /// <remarks>
        /// Configures the validation rule for the <see cref="RegisterUserCommand"/>:
        /// - <see cref="RegisterUserCommand.Request"/>: Validates user details using <see cref="UserCreateDtoValidator"/>.
        /// </remarks>
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Request)
                .SetValidator(new UserCreateDtoValidator())
                .WithMessage("Invalid user details provided.");
        }
    }
}

using BG.LocalWeb.Application.Common.DTOs.User;
using FluentValidation;

namespace BG.LocalWeb.Application.Validators
{
    /// <summary>
    /// Validator for <see cref="UserCreateDto"/> instances.
    /// </summary>
    /// <remarks>
    /// This class ensures that the data provided in a <see cref="UserCreateDto"/> meets the required criteria before processing.
    /// It validates fields such as Username, Password, FirstName, LastName, DateOfBirth, and Address.
    /// </remarks>
    public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserCreateDtoValidator"/> class.
        /// </summary>
        /// <remarks>
        /// Configures the validation rules for the <see cref="UserCreateDto"/>:
        /// - <see cref="UserCreateDto.Username"/>: Must be provided, at least 3 characters long, and no more than 50 characters.
        /// - <see cref="UserCreateDto.Password"/>: Must be provided, at least 6 characters long, and no more than 100 characters.
        /// - <see cref="UserCreateDto.FirstName"/>: Must be provided and no more than 50 characters.
        /// - <see cref="UserCreateDto.LastName"/>: Must be provided and no more than 50 characters.
        /// - <see cref="UserCreateDto.DateOfBirth"/>: Must be provided and must be a past date.
        /// - <see cref="UserCreateDto.Address"/>: Must be provided and no more than 250 characters.
        /// </remarks>
        public UserCreateDtoValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(100).WithMessage("Password cannot exceed 100 characters.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required.")
                .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(250).WithMessage("Address cannot exceed 250 characters.");
        }
    }
}

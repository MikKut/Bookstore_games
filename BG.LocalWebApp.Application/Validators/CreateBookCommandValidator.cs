using BG.LocalApi.Application.Handlers.Books.Commands;
using BG.LocalApi.Domain.Enums;
using FluentValidation;

namespace BG.LocalApi.Application.Validators
{
    /// <summary>
    /// Validator for <see cref="CreateBookCommand"/> instances.
    /// </summary>
    /// <remarks>
    /// This class validates the data provided in a <see cref="CreateBookCommand"/> to ensure it meets the required criteria before processing.
    /// </remarks>

    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateBookCommandValidator"/> class.
        /// </summary>
        /// <remarks>
        /// Configures the validation rules for the <see cref="CreateBookCommand"/>:
        /// - <see cref="CreateBookCommand.Title"/>: Must not be empty and cannot exceed 100 characters.
        /// - <see cref="CreateBookCommand.PublicationYear"/>: Must be between 1500 and the current year.
        /// - <see cref="CreateBookCommand.Genre"/>: Must be a valid enum value.
        /// </remarks>
        public CreateBookCommandValidator()
        {
            // Validate that the Title is not empty and does not exceed 100 characters.
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100)
                .WithMessage("Title cannot exceed 100 characters.");

            // Validate that the PublicationYear is between 1500 and the current year.
            RuleFor(x => x.PublicationYear)
                .InclusiveBetween(800, DateTime.Now.Year)
                .WithMessage($"Publication year must be between 800 and {DateTime.Now.Year}.");

            // Validate that the Genre is a valid enum value.
            RuleFor(x => x.Genre)
                    .Must(value => EnumValidator.IsValidEnum<Genre>(value))
                    .WithMessage("Invalid enum value.");
        }
    }
}

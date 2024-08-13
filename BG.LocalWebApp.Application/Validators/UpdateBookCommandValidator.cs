using BG.LocalApi.Application.Handlers.Books.Commands;
using BG.LocalApi.Domain.Enums;
using FluentValidation;

namespace BG.LocalApi.Application.Validators
{
    /// <summary>
    /// Validator for <see cref="UpdateBookCommand"/> instances.
    /// </summary>
    /// <remarks>
    /// This class ensures that the data provided in an <see cref="UpdateBookCommand"/> meets the required criteria before processing.
    /// </remarks>
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateBookCommandValidator"/> class.
        /// </summary>
        /// <remarks>
        /// Configures the validation rules for the <see cref="UpdateBookCommand"/>:
        /// - <see cref="UpdateBookCommand.Id"/>: Must not be empty.
        /// - <see cref="UpdateBookCommand.Title"/>: Must not be empty and cannot exceed 100 characters.
        /// - <see cref="UpdateBookCommand.PublicationYear"/>: Must be between 1500 and the current year.
        /// - <see cref="UpdateBookCommand.Genre"/>: Must be a valid enum value.
        /// </remarks>
        public UpdateBookCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.PublicationYear)
                .InclusiveBetween(1500, DateTime.Now.Year).WithMessage($"Publication year must be between 1500 and {DateTime.Now.Year}.");

            RuleFor(x => x.Genre)
                    .Must(value => EnumValidator.IsValidEnum<Genre>(value))
                    .WithMessage("Invalid enum value.");
        }
    }
}

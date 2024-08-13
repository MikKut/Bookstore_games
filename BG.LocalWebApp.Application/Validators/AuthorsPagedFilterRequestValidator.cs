using BG.LocalApi.Application.Common.Requests;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.LocalApi.Application.Validators
{
    /// <summary>
    /// Validator for <see cref="AuthorsPagedFilterRequest"/> instances.
    /// </summary>
    /// <remarks>
    /// This class ensures that the data provided in a <see cref="AuthorsPagedFilterRequest"/> instance meets the required validation rules before processing.
    /// </remarks>
    public class AuthorsPagedFilterRequestValidator : AbstractValidator<AuthorsPagedFilterRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorsPagedFilterRequestValidator"/> class.
        /// </summary>
        /// <remarks>
        /// Configures the validation rules for the <see cref="AuthorsPagedFilterRequest"/>:
        /// - <see cref="AuthorsPagedFilterRequest.FirstName"/>: Optional, but if provided, cannot exceed 100 characters.
        /// - <see cref="AuthorsPagedFilterRequest.LastName"/>: Optional, but if provided, cannot exceed 100 characters.
        /// - <see cref="AuthorsPagedFilterRequest.PageNumber"/>: Must be greater than or equal to 1.
        /// - <see cref="AuthorsPagedFilterRequest.PageSize"/>: Must be between 1 and 100.
        /// </remarks>
        public AuthorsPagedFilterRequestValidator()
        {
            // FirstName validation: optional, but if provided, should not exceed 100 characters
            RuleFor(x => x.FirstName)
                .MaximumLength(100)
                .WithMessage("First name cannot be longer than 100 characters.");

            // LastName validation: optional, but if provided, should not exceed 100 characters
            RuleFor(x => x.LastName)
                .MaximumLength(100)
                .WithMessage("Last name cannot be longer than 100 characters.");

            // PageNumber validation: must be greater than or equal to 1
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page number must be at least 1.");

            // PageSize validation: must be between 1 and 100
            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100.");
        }
    }
}

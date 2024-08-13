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
    /// Validator for <see cref="BooksPagedFilterRequest"/> instances.
    /// </summary>
    /// <remarks>
    /// This class ensures that the data provided in a <see cref="BooksPagedFilterRequest"/> instance meets the required validation rules before processing.
    /// </remarks>
    public class BooksPagedFilterRequestValidator : AbstractValidator<BooksPagedFilterRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BooksPagedFilterRequestValidator"/> class.
        /// </summary>
        /// <remarks>
        /// Configures the validation rules for the <see cref="BooksPagedFilterRequest"/>:
        /// - <see cref="BooksPagedFilterRequest.Genre"/>: Optional, but if provided, cannot exceed 50 characters.
        /// - <see cref="BooksPagedFilterRequest.Title"/>: Optional, but if provided, cannot exceed 100 characters.
        /// - <see cref="BooksPagedFilterRequest.PageNumber"/>: Must be greater than 0.
        /// - <see cref="BooksPagedFilterRequest.PageSize"/>: Must be between 1 and 100.
        /// </remarks>
        public BooksPagedFilterRequestValidator()
        {
            // Genre validation: optional, but if provided, must not exceed 50 characters
            RuleFor(x => x.Genre)
                .MaximumLength(50)
                .WithMessage("Genre must not exceed 50 characters.");

            // Title validation: optional, but if provided, must not exceed 100 characters
            RuleFor(x => x.Title)
                .MaximumLength(100)
                .WithMessage("Title must not exceed 100 characters.");

            // PageNumber validation: must be greater than 0
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("PageNumber must be greater than 0.");

            // PageSize validation: must be between 1 and 100
            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("PageSize must be between 1 and 100.");
        }
    }
}

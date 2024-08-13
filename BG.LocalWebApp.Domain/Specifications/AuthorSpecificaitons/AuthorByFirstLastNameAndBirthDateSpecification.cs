using Ardalis.Specification;
using BG.LocalApi.Domain.Entities;

namespace BG.LocalApi.Domain.Specifications.AuthorSpecificaitons
{
    /// <summary>
    /// Specification for querying <see cref="Author"/> entities by their first name, last name, and date of birth.
    /// </summary>
    public class AuthorByFirstLastNameAndBirthDateSpecification : Specification<Author>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorByFirstLastNameAndBirthDateSpecification"/> class.
        /// </summary>
        /// <param name="author">The <see cref="Author"/> entity containing the criteria for the query.</param>
        public AuthorByFirstLastNameAndBirthDateSpecification(Author author)
        {
            Query.AsNoTracking()
                .Where(x =>
                    x.LastName == author.LastName &&
                    x.FirstName == author.FirstName &&
                    x.DateOfBirth.Value == author.DateOfBirth.Value);
        }
    }
}

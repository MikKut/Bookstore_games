using Ardalis.Specification;
using BG.LocalApi.Domain.Entities;

namespace BG.LocalApi.Domain.Specifications.BookSpecifications
{
    /// <summary>
    /// Specification for querying <see cref="Book"/> entities by their title, genre, and publication year.
    /// </summary>
    public class BookByTitleGenreAndYearSpecification : Specification<Book>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookByTitleGenreAndYearSpecification"/> class.
        /// </summary>
        /// <param name="book">The <see cref="Book"/> entity containing the criteria for the query.</param>
        public BookByTitleGenreAndYearSpecification(Book book)
        {
            Query.AsNoTracking()
                .Where(x =>
                    x.Title == book.Title &&
                    x.Genre == book.Genre &&
                    x.PublicationYear == book.PublicationYear);
        }
    }
}

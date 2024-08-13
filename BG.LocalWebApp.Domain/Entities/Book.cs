using BG.LocalApi.Domain.Enums;
using BG.LocalWebApp.Common.Entities;

namespace BG.LocalApi.Domain.Entities
{
    /// <summary>
    /// Represents a book entity with details such as title, publication year, genre, and author.
    /// </summary>
    public class Book : BaseEntity
    {
        /// <summary>
        /// Gets or sets the title of the book.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the year the book was published.
        /// </summary>
        public int PublicationYear { get; set; }

        /// <summary>
        /// Gets or sets the genre of the book.
        /// </summary>
        public Genre Genre { get; set; }
    }

}

using Ardalis.Specification;
using BG.LocalApi.Domain.Entities;
using BG.LocalApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.LocalApi.Domain.Specifications.BookSpecifications
{
    public class BooksPagedFilterSpecification : Specification<Book>
    {
        public BooksPagedFilterSpecification(Genre? genre, string? title, int pageNumber, int pageSize)
        {
            if (genre != null)
            {
                Query.Where(book => book.Genre == genre);
            }

            if (!string.IsNullOrWhiteSpace(title))
            {
                Query.Where(book => book.Title.Contains(title));
            }

            Query.Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize);

        }
    }
}

using Ardalis.Specification;
using BG.LocalApi.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG.LocalApi.Domain.Specifications.AuthorSpecificaitons
{
    public class AuthorsPagedFilterSpecification : Specification<Author>
    {
        public AuthorsPagedFilterSpecification(string? firstName, string? lastName, int pageSize, int pageNumber)
        {
            if (!string.IsNullOrEmpty(firstName))
            {
                Query.Where(author => author.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                Query.Where(author => author.LastName.Contains(lastName));
            }

            Query.Skip((pageNumber - 1) * pageSize)
                 .Take(pageSize);
        }
    }
}

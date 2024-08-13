using Ardalis.Specification.EntityFrameworkCore;
using BG.LocalApi.Domain.Entities;
using BG.LocalApi.Domain.Interfaces.Repositories;
using BG.LocalApi.Infrastructure.Data;

namespace BG.LocalApi.Infrastructure.Repositories
{
    public class AuthorRepository : RepositoryBase<Author>, IAuthorRepository
    {
        public AuthorRepository(AppDbContext dbContext)
           : base(dbContext)
        {

        }
    }
}

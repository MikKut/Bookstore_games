using Ardalis.Specification.EntityFrameworkCore;
using BG.LocalApi.Domain.Entities;
using BG.LocalApi.Domain.Interfaces.Repositories;
using BG.LocalApi.Infrastructure.Data;

namespace BG.LocalApi.Infrastructure.Repositories
{
    /// <inheritdoc cref="RepositoryBase{T}" for <see cref="Book"/>/>
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(AppDbContext dbContext)
           : base(dbContext)
        {

        }
    }
}

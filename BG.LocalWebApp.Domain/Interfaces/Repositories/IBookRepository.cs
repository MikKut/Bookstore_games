using Ardalis.Specification;
using BG.LocalApi.Domain.Entities;

namespace BG.LocalApi.Domain.Interfaces.Repositories
{
    /// <summary>
    /// /// <inheritdoc cref="IRepositoryBase{T}"/> for <see cref="Book"/>
    /// </summary>
    public interface IBookRepository : IRepositoryBase<Book>
    {
    }
}

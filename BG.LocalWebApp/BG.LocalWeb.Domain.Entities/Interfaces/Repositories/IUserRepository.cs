using Ardalis.Specification;
using BG.LocalWeb.Domain.Entities;

namespace BG.LocalWeb.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Defines methods for accessing and managing user data in the repository.
    /// </summary>
    public interface IUserRepository : IRepositoryBase<User>
    {
    }
}

using Ardalis.Specification.EntityFrameworkCore;
using BG.LocalWeb.Domain.Entities;
using BG.LocalWeb.Domain.Interfaces.Repositories;
using BG.LocalWeb.Infrastructure.Data;

namespace BG.LocalWeb.Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(UserDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}

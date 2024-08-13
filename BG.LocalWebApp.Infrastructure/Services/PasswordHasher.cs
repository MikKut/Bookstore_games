using BG.LocalWeb.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BG.LocalWeb.Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher<User>
    {
        private readonly IPasswordHasher<User> _passwordHasher;

        public PasswordHasher()
        {
            _passwordHasher = new PasswordHasher<User>();
        }

        public string HashPassword(User user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
        {
            return _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
        }
    }
}

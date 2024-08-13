using BG.LocalWeb.Domain.Entities;
using BG.LocalWeb.Domain.Interfaces.Repositories;
using BG.LocalWeb.Domain.Interfaces.Services;
using BG.LocalWeb.Infrastructure.Identity;
using BG.LocalWeb.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BG.LocalWeb.Infrastructure
{

    public static class LocalWebInfrastructureDependencyInjection
    {
        public static IServiceCollection AddLocalWebInfrastructureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            return services;
        }
    }
}

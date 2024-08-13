using BG.LocalApi.Domain.Interfaces.Repositories;
using BG.LocalApi.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BG.LocalApi.Infrastructure
{
    public static class LocalApiInfrastructureDependencyInjection
    {
        public static IServiceCollection AddLocalApiInfrastructureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            return services;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;

namespace BG.LocalApi.Application
{
    public static class LocalApiApplicationDependencyInjection
    {
        public static IServiceCollection AddLocalApiApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LocalApiApplicationDependencyInjection).Assembly));
            services.AddAutoMapper(typeof(LocalApiApplicationDependencyInjection).Assembly);
            // services.AddValidatorsFromAssembly(typeof(LocalApiApplicationDependencyInjection).Assembly);

            return services;
        }
    }
}

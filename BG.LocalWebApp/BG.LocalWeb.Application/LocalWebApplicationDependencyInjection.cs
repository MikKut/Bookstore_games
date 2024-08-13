using Microsoft.Extensions.DependencyInjection;

namespace BG.LocalWeb.Application
{
    public static class LocalWebAplicationDependencyInjection
    {
        public static IServiceCollection AddLocalWebApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LocalWebAplicationDependencyInjection).Assembly));
            services.AddAutoMapper(typeof(LocalWebAplicationDependencyInjection).Assembly);
            // services.AddValidatorsFromAssembly(typeof(LocalWebAplicationDependencyInjection).Assembly);

            return services;
        }
    }
}

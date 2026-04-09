using DavesArcade.Application.Interfaces;
using DavesArcade.Infrastructure.Persistence.InMemory;
using Microsoft.Extensions.DependencyInjection;

namespace DavesArcade.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Register caching
            services.AddMemoryCache();

            // Register repositories
            services.AddSingleton<IGameRepository, InMemoryGameRepository>();

            return services;
        }
    }
}
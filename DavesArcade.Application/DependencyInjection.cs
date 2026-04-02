using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DavesArcade.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DavesArcade.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register AutoMapper
            //services.AddAutoMapper(typeof(DependencyInjection).Assembly);

            // Register application services
            //services.AddScoped<IProductsService, ProductsService>();
            //services.AddScoped<IWelcomeService, WelcomeService>();

            // Register application services

            // NOTE: Repository registrations belong in Infrastructure layer, not here!
            //services.AddScoped<IGameRepository, InMemoryGameRepository>();

            return services;
        }
    }
}

using DavesArcade.Application.Interfaces;
using DavesArcade.Infrastructure.Persistence.InMemory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DavesArcade.Tests.Integration;

/// <summary>
/// Custom web application factory for integration testing.
/// Creates an in-memory test server with a singleton repository per factory instance.
/// Each test creates its own factory, ensuring complete test isolation.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing IGameRepository registration
            services.RemoveAll<IGameRepository>();

            // Register as SINGLETON so all requests within this factory instance
            // share the same repository (required for tests that create then read data)
            // Since each test creates its own factory, tests remain isolated
            services.AddSingleton<IGameRepository, InMemoryGameRepository>();
        });

        builder.UseEnvironment("Testing");
    }
}

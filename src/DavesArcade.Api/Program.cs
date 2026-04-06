using DavesArcade.Api.Endpoints.Games;
using DavesArcade.Api.Middleware;
using DavesArcade.Application;
using DavesArcade.Infrastructure;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Dave's Arcade API",
        Version = "v1",
        Description = "Game Catalog API showcasing Clean Architecture",
        Contact = new OpenApiContact
        {
            Name = "David Hatzenbuehler",
            Url = new Uri("https://github.com/DaveHatz23")
        }
    });
});

// Register layers following Clean Architecture
builder.Services.AddApplication();      // Application layer services
builder.Services.AddInfrastructure();   // Infrastructure layer services (repositories, data access)

// Add health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Add global exception handler
app.UseMiddleware<GlobalExceptionHandler>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dave's Arcade API v1");
    });
}

app.UseHttpsRedirection();

// Map health check endpoint
app.MapHealthChecks("/health");

app.MapGames();

app.Run();

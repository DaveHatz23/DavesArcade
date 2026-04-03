using DavesArcade.Api.Endpoints.Games;
using DavesArcade.Api.Middleware;
using DavesArcade.Application;
using DavesArcade.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register layers following Clean Architecture
builder.Services.AddApplication();      // Application layer services
builder.Services.AddInfrastructure();   // Infrastructure layer services (repositories, data access)

var app = builder.Build();

// Add global exception handler
app.UseMiddleware<GlobalExceptionHandler>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGames();

app.Run();

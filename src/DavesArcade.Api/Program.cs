using Asp.Versioning;
using DavesArcade.Api.Endpoints.Games;
using DavesArcade.Api.Middleware;
using DavesArcade.Application;
using DavesArcade.Infrastructure;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerGen;

// Configure Serilog BEFORE building the app
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .WriteTo.Console()
    .WriteTo.File("logs/davesarcade-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting Dave's Arcade API");

    var builder = WebApplication.CreateBuilder(args);

    // Replace default logging with Serilog
    builder.Host.UseSerilog();

    // Add API Versioning with API Explorer
    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    }).AddApiExplorer(options =>
    {
        // Format the version as 'v'major[.minor][-status]
        options.GroupNameFormat = "'v'VVV";
        // Substitute the version in the URL
        options.SubstituteApiVersionInUrl = true;
    });

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

        c.SwaggerDoc("v2", new OpenApiInfo
        {
            Title = "Dave's Arcade API",
            Version = "v2",
            Description = "Game Catalog API v2 with enhanced filtering and pagination",
            Contact = new OpenApiContact
            {
                Name = "David Hatzenbuehler",
                Url = new Uri("https://github.com/DaveHatz23")
            }
        });

        // Filter endpoints by API version for Swagger docs
        c.DocInclusionPredicate((docName, apiDesc) =>
        {
            if (!apiDesc.TryGetMethodInfo(out _))
                return false;

            var versions = apiDesc.ActionDescriptor.EndpointMetadata
                .OfType<ApiVersionMetadata>()
                .SelectMany(m => m.Map(Asp.Versioning.ApiVersionMapping.Explicit | Asp.Versioning.ApiVersionMapping.Implicit).SupportedApiVersions)
                .Distinct();

            return versions.Any(v => $"v{v.MajorVersion}" == docName);
        });
    });

    // Register layers following Clean Architecture
    builder.Services.AddApplication();      // Application layer services
    builder.Services.AddInfrastructure();   // Infrastructure layer services (repositories, data access)

    // Register middleware
    builder.Services.AddTransient<GlobalExceptionHandler>();

    // Add health checks
    builder.Services.AddHealthChecks();

    var app = builder.Build();

    // Create version sets
    var versionSet = app.NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1, 0))
        .HasApiVersion(new ApiVersion(2, 0))
        .ReportApiVersions()
        .Build();

    // Add request logging middleware
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
        };
    });

    // Add global exception handler
    app.UseMiddleware<GlobalExceptionHandler>();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dave's Arcade API v1");
            c.SwaggerEndpoint("/swagger/v2/swagger.json", "Dave's Arcade API v2");
        });
    }

    app.UseHttpsRedirection();

    // Map health check endpoint
    app.MapHealthChecks("/health");

    // Map versioned endpoints
    app.MapGames(versionSet);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

// Make Program accessible for integration tests
public partial class Program { }
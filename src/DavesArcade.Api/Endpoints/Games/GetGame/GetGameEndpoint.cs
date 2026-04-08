using DavesArcade.Api.Extensions;
using DavesArcade.Application.Interfaces;

namespace DavesArcade.Api.Endpoints.Games.GetGame;

public static class GetGameEndpoint
{
    /// <summary>
    /// Maps the GET /games/{id} endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapGetGame(this IEndpointRouteBuilder app, string? version = null)
    {
        return app.MapGet("/{id}", async (
            Guid id,
            IGameRepository gameRepository,
            ILogger<Program> logger) =>
        {
            logger.LogInformation("Fetching game with ID: {GameId}", id);

            var result = await gameRepository.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                logger.LogWarning("Game {GameId} not found", id);
            }
            else
            {
                logger.LogInformation("Successfully retrieved game {GameName} ({GameId})",
                    result.Value!.Name, id);
            }

            return result.ToHttpResult();
        })
        .Produces(200)
        .Produces(404)
        .Produces(500)
        .WithName(string.IsNullOrEmpty(version) ? "GetGameById" : $"GetGameById{version}")
        .WithSummary("Get game by ID")
        .WithDescription("Retrieves a specific game by its unique identifier");
    }
}
using DavesArcade.Api.Extensions;
using DavesArcade.Application.Interfaces;

namespace DavesArcade.Api.Endpoints.Games.GetGames;

public static class GetGamesEndpoint
{
    /// <summary>
    /// Maps the GET /games endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapGetGames(this IEndpointRouteBuilder app)
    {
        return app.MapGet("/", async (
            IGameRepository gameRepository,
            ILogger<Program> logger) =>
        {
            logger.LogInformation("Fetching all games");

            var result = await gameRepository.GetAllAsync();

            if (!result.IsSuccess)
            {
                logger.LogWarning("Failed to fetch games");
            }
            else
            {
                logger.LogInformation("Successfully retrieved {Count} games", result.Value!.Count());
            }

            return result.ToHttpResult();
        })
        .Produces(200)
        .Produces(500)
        .WithName("GetGames")
        .WithSummary("Get all games")
        .WithDescription("Retrieves all games from the catalog");
    }
}
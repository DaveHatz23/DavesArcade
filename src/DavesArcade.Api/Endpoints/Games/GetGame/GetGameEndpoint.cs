using DavesArcade.Api.Extensions;
using DavesArcade.Application.Interfaces;

namespace DavesArcade.Api.Endpoints.Games.GetGame;

public static class GetGameEndpoint
{
    public static void MapGetGame(this IEndpointRouteBuilder app)
    {
        // GET /games/122233-434d-43434....

        app.MapGet("/{id}", async (
                Guid id,
                IGameRepository gameRepository,
                ILogger<Program> logger) =>
            {
                logger.LogInformation($"Fetching game with {id}");

                var result = await gameRepository.GetByIdAsync(id);

                if (!result.IsSuccess)
                {
                    logger.LogWarning($"Game {id} not found");
                }
                else
                {
                    logger.LogInformation("Successfully retrieved game {GameName} ({GameId})",
                        result.Value!.Name, id);
                }

                return result.ToHttpResult();
            })
            .AllowAnonymous();
    }
}
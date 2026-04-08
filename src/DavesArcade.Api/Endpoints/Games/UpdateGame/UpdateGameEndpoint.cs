using DavesArcade.Api.Extensions;
using DavesArcade.Application.DTOs;
using DavesArcade.Application.Interfaces;

namespace DavesArcade.Api.Endpoints.Games.UpdateGame;

public static class UpdateGameEndpoint
{
    /// <summary>
    /// Maps the PUT /games/{id} endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapUpdateGame(this IEndpointRouteBuilder app, string? version = null)
    {
        return app.MapPut("/{id}", async (
            Guid id,
            UpdateGameRequest updateGameRequest,
            IGameRepository gameRepository,
            ILogger<Program> logger) =>
        {
            logger.LogInformation("Updating game {GameId} with new data", id);

            var result = await gameRepository.UpdateAsync(id, updateGameRequest);

            if (result.IsSuccess)
            {
                logger.LogInformation("Successfully updated game {GameId}: {GameName}",
                    id, result.Value!.Name);
            }
            else
            {
                logger.LogWarning("Failed to update game {GameId}: {ErrorCode} - {ErrorMessage}",
                    id, result.Error!.Code, result.Error.Description);
            }

            return result.ToHttpResult();
        })
        .Produces(200)
        .Produces(400)
        .Produces(404)
        .Produces(500)
        .WithName(string.IsNullOrEmpty(version) ? "UpdateGame" : $"UpdateGame{version}")
        .WithSummary("Update an existing game")
        .WithDescription("Updates an existing game in the catalog");
    }
}
using DavesArcade.Api.Extensions;
using DavesArcade.Application.Interfaces;

namespace DavesArcade.Api.Endpoints.Games.DeleteGame;

public static class DeleteGameEndpoint
{
    /// <summary>
    /// Maps the DELETE /games/{id} endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapDeleteGame(this IEndpointRouteBuilder app, string? version = null)
    {
        return app.MapDelete("/{id}", async (
            Guid id,
            IGameRepository gameRepository,
            ILogger<Program> logger) =>
        {
            logger.LogInformation("Deleting game {GameId}", id);

            var result = await gameRepository.DeleteByIdAsync(id);

            if (result.IsSuccess)
            {
                logger.LogInformation("Successfully deleted game {GameId}", id);
            }
            else
            {
                logger.LogWarning("Failed to delete game {GameId}: {ErrorCode} - {ErrorMessage}",
                    id, result.Error!.Code, result.Error.Description);
            }

            return result.ToNoContentResult();
        })
        .Produces(204)
        .Produces(404)
        .Produces(500)
        .WithName(string.IsNullOrEmpty(version) ? "DeleteGame" : $"DeleteGame{version}")
        .WithSummary("Delete a game")
        .WithDescription("Deletes a game from the catalog");
    }
}
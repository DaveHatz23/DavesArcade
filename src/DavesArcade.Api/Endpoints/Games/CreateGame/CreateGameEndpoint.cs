using DavesArcade.Api.Extensions;
using DavesArcade.Application.DTOs;
using DavesArcade.Application.Interfaces;

namespace DavesArcade.Api.Endpoints.Games.CreateGame;

public static class CreateGameEndpoint
{
    /// <summary>
    /// Maps the POST /games endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapCreateGame(this IEndpointRouteBuilder app, string? version = null)
    {
        return app.MapPost("/", async (
            CreateGameRequest createGameRequest,
            IGameRepository gameRepository,
            ILogger<Program> logger) =>
        {
            logger.LogInformation("Creating new game: {GameName} in genre {GenreId}",
                createGameRequest.Name, createGameRequest.GenreId);

            var result = await gameRepository.CreateAsync(createGameRequest);

            if (result.IsSuccess)
            {
                logger.LogInformation("Successfully created game {GameId}: {GameName}",
                    result.Value!.Id, result.Value.Name);
            }
            else
            {
                logger.LogWarning("Failed to create game {GameName}: {ErrorCode} - {ErrorMessage}",
                    createGameRequest.Name, result.Error!.Code, result.Error.Description);
            }

            return result.ToCreatedResult(game => $"/v1/games/{game.Id}");
        })
        .Produces(201)
        .Produces(400)
        .Produces(500)
        .WithName(string.IsNullOrEmpty(version) ? "CreateGame" : $"CreateGame{version}")
        .WithSummary("Create a new game")
        .WithDescription("Creates a new game in the catalog");
    }
}
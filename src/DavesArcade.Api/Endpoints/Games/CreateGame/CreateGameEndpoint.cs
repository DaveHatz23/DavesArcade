using DavesArcade.Api.Extensions;
using DavesArcade.Application.DTOs;
using DavesArcade.Application.Interfaces;

namespace DavesArcade.Api.Endpoints.Games.CreateGame;

public static class CreateGameEndpoint
{
    private const string DefaultImageUri = "https://placehold.co/100";

    public static void MapCreateGame(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (
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
                
                return result.ToCreatedResult(game => $"/games/{game.Id}");
            })
            .AllowAnonymous();
    }
}
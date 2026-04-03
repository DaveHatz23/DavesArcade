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
                IGameRepository gameRepository) =>
            {
                var result = await gameRepository.CreateAsync(createGameRequest);
                return result.ToCreatedResult(game => $"/games/{game.Id}");
            })
            .AllowAnonymous();
    }
}
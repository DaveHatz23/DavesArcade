using DavesArcade.Api.Extensions;
using DavesArcade.Application.Interfaces;

namespace DavesArcade.Api.Endpoints.Games.GetGames;

public static class GetGamesEndpoint
{
    public static void MapGetGames(this IEndpointRouteBuilder app)
    {
        // GET /games
        app.MapGet("/", async (IGameRepository gameRepository) =>
            {
                var result = await gameRepository.GetAllAsync();
                return result.ToHttpResult();
            })
            .AllowAnonymous();
    }
}
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
                IGameRepository gameRepository) =>
            {
                var result = await gameRepository.GetByIdAsync(id);
                return result.ToHttpResult();
            })
            .AllowAnonymous();
    }
}
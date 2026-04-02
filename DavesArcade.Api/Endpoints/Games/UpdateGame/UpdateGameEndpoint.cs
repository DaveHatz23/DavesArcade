using DavesArcade.Application.DTOs;
using DavesArcade.Application.Interfaces;

namespace DavesArcade.Api.Endpoints.Games.UpdateGame;

public static class UpdateGameEndpoint
{
    public static void MapUpdateGame(this IEndpointRouteBuilder app)
    {
        // PUT /games/122233-434d-43434....
        app.MapPut("/{id}", async (
                Guid id,
                UpdateGameRequest updateGameRequest,
                IGameRepository gameRepository) =>
            {
                var result = await gameRepository.UpdateAsync(id, updateGameRequest);
                return Results.Ok(result.Value); // or Results.NoContent() for REST convention
            })
            .WithParameterValidation()
            .DisableAntiforgery()
            //.RequireAuthorization(Policies.AdminAccess);
            .AllowAnonymous();
    }
}
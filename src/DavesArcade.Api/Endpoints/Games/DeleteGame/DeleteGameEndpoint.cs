using DavesArcade.Api.Extensions;
using DavesArcade.Application.Interfaces;

namespace DavesArcade.Api.Endpoints.Games.DeleteGame;

public static class DeleteGameEndpoint
{
    public static void MapDeleteGame(this IEndpointRouteBuilder app)
    {
        // DELETE /games/122233-434d-43434....

        app.MapDelete("/{id}", async (
            Guid id,
            IGameRepository gameRepository) =>
        {
            // Call the repository to delete the game by id.
            // Return 204 No Content if successful
            // Return 404 if the game with the specified id does not exist,
            // or an appropriate error response if the delete operation fails for some reason (e.g., database error).
            var result = await gameRepository.DeleteByIdAsync(id);
            
            if (!result.IsSuccess)
            {
                return result.ToHttpResult();
            }

            return Results.NoContent(); // REST convention for successful DELETE
        })
        .AllowAnonymous();

        // TODO : for now, allow anonymous access to delete games, but in the future, we should restrict this to admin users only
    }
}
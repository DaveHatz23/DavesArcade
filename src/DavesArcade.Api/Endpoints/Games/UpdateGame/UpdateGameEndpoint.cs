using DavesArcade.Api.Extensions;
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
                // Fetch / Modify / Save 

                // ExecuteUpdateAsync/ExecuteDeleteAsync ::
                // allow you to execute updates and deletes directly at the database level without first loading entities into memory.
                // No change tracking is involved, and the operations are performed directly on the database, which can lead to improved performance for bulk updates or deletes.
                // Immediate execution: The operations are executed immediately when the method is called, without the need for an explicit call to SaveChangesAsync().
                // they return the number of affected rows, which can be useful for determining the success of the operation or for logging purposes.


                // Call the repository to update the game.
                // Return a 404 if the game doesn't exist, otherwise return the updated game.
                var result = await gameRepository.UpdateAsync(id, updateGameRequest);
                    return result.ToHttpResult();
            })
            .AllowAnonymous();
    }
}
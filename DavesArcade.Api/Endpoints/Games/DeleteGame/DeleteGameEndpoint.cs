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
            var game = await gameRepository.DeleteByIdAsync(id);
            return Results.Ok(game);
        });

        // TODO : for now, allow anonymous access to delete games, but in the future, we should restrict this to admin users only

        //app.MapDelete("/{id}", async (Guid id, GameStoreContext dbContext) =>
        //    {
        //        await dbContext.Games
        //            .Where(game => game.Id == id)
        //            .ExecuteDeleteAsync();

        //        return Results.NoContent();
        //    })
        //    .RequireAuthorization(Policies.AdminAccess);
    }
}
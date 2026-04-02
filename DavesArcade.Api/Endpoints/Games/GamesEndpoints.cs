using DavesArcade.Api.Endpoints.Games.CreateGame;
using DavesArcade.Api.Endpoints.Games.DeleteGame;
using DavesArcade.Api.Endpoints.Games.GetGame;
using DavesArcade.Api.Endpoints.Games.GetGames;

namespace DavesArcade.Api.Endpoints.Games;

public static class GamesEndpoints
{
    public static void MapGames(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/games");

        group.MapGetGames();
        group.MapGetGame();
        group.MapCreateGame();
        //group.MapUpdateGame();
        group.MapDeleteGame();
    }
}
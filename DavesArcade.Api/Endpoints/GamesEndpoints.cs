using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DavesArcade.Api.Endpoints.GetGame;
using DavesArcade.Api.Endpoints.GetGames;

namespace DavesArcade.Api.Endpoints
{
    public static class GamesEndpoints
    {
        public static void MapGames(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/games");

            group.MapGetGames();
            group.MapGetGame();
            //group.MapCreateGame();
            //group.MapUpdateGame();
            //group.MapDeleteGame();
        }
    }
}

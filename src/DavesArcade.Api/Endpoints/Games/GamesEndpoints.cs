using DavesArcade.Api.Endpoints.Games.CreateGame;
using DavesArcade.Api.Endpoints.Games.DeleteGame;
using DavesArcade.Api.Endpoints.Games.GetGame;
using DavesArcade.Api.Endpoints.Games.GetGames;
using DavesArcade.Api.Endpoints.Games.UpdateGame;
using Asp.Versioning.Builder;

namespace DavesArcade.Api.Endpoints.Games;

public static class GamesEndpoints
{
    public static void MapGames(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        var gamesGroup = app.MapGroup("/v{version:apiVersion}/games")
            .WithApiVersionSet(versionSet)
            .WithTags("Games");

        // V1 endpoints - Original implementation
        gamesGroup.MapGetGames()
            .MapToApiVersion(1.0);
        
        gamesGroup.MapGetGame()
            .MapToApiVersion(1.0);
        
        gamesGroup.MapCreateGame()
            .MapToApiVersion(1.0);
        
        gamesGroup.MapUpdateGame()
            .MapToApiVersion(1.0);
        
        gamesGroup.MapDeleteGame()
            .MapToApiVersion(1.0);

        // V2 endpoints - Enhanced with additional features
        gamesGroup.MapGetGamesV2()
            .MapToApiVersion(2.0);
        
        gamesGroup.MapGetGame("V2")
            .MapToApiVersion(2.0);
        
        gamesGroup.MapCreateGame("V2")
            .MapToApiVersion(2.0);
        
        gamesGroup.MapUpdateGame("V2")
            .MapToApiVersion(2.0);
        
        gamesGroup.MapDeleteGame("V2")
            .MapToApiVersion(2.0);
    }
}
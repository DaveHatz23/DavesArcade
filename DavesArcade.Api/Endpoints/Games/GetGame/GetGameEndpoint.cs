using DavesArcade.Application.Interfaces;
using DavesArcade.Application.Results;

namespace DavesArcade.Api.Endpoints.Games.GetGame;

public static class GetGameEndpoint
{
    public static void MapGetGame(this IEndpointRouteBuilder app)
    {
        // GET /games/122233-434d-43434....

        //app.MapGet("/{id}", () => "Hello World")
        //    .AllowAnonymous();

        app.MapGet("/{id}", async (
                Guid id,
                IGameRepository gameRepository) =>
            {
                var result = await gameRepository.GetByIdAsync(id);

                if (!result.IsSuccess)
                {
                    return result.Error!.Type switch
                    {
                        ErrorType.NotFound => Results.NotFound(new { error = result.Error.Description, code = result.Error.Code }),
                        ErrorType.Validation => Results.BadRequest(new { error = result.Error.Description, code = result.Error.Code }),
                        ErrorType.Conflict => Results.Conflict(new { error = result.Error.Description, code = result.Error.Code }),
                        _ => Results.Problem("An unexpected error occurred.")
                    };
                }

                return Results.Ok(result.Value);
            })
            .AllowAnonymous();

        //    app.MapGet("/{id}", async (
        //            Guid id,
        //            GameStoreContext dbContext,
        //            CdnUrlTransformer cdnUrlTransformer,
        //            ILogger<Program> logger) =>
        //        {
        //            Game? game = await dbContext.Games.FindAsync(id);

        //            return game is null ? Results.NotFound() : Results.Ok(
        //                new GameDetailsDto(
        //                    game.Id,
        //                    game.Name,
        //                    game.GenreId,
        //                    game.Price,
        //                    game.ReleaseDate,
        //                    game.Description,
        //                    cdnUrlTransformer.TransformToCdnUrl(game.ImageUri),
        //                    game.LastUpdatedBy
        //                ));
        //        })
        //        .WithName(EndpointNames.GetGame)
        //        .AllowAnonymous();
        //}
    }
}
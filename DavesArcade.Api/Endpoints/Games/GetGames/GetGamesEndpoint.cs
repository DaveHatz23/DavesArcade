    using DavesArcade.Application.Interfaces;
using DavesArcade.Application.Results;

namespace DavesArcade.Api.Endpoints.Games.GetGames;

public static class GetGamesEndpoint
{
    public static void MapGetGames(this IEndpointRouteBuilder app)
    {
        // GET /games
        app.MapGet("/", async (IGameRepository gameRepository) =>
            {
                var result = await gameRepository.GetAllAsync();

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

        /*
        app.MapGet("/", async (
                GameStoreContext dbContext,
                [AsParameters] GetGamesDto request,
                CdnUrlTransformer cdnUrlTransformer) =>
            {
                var skipCount = (request.PageNumber - 1) * request.PageSize;

                var filteredGames = dbContext.Games
                    .Where(game => string.IsNullOrWhiteSpace(request.Name)
                                   || EF.Functions.Like(game.Name, $"%{request.Name}%"));

                var gamesOnPage = await filteredGames
                    .OrderBy(game => game.Name)
                    .Skip(skipCount)
                    .Take(request.PageSize)
                    .Include(game => game.Genre)
                    .Select(game => new GameSummaryDto(
                        game.Id,
                        game.Name,
                        game.Genre!.Name,
                        game.Price,
                        game.ReleaseDate,
                        cdnUrlTransformer.TransformToCdnUrl(game.ImageUri),
                        game.LastUpdatedBy
                    ))
                    .AsNoTracking()
                    .ToListAsync();

                var totalGames = await filteredGames.CountAsync();
                var totalPages = (int)Math.Ceiling(totalGames / (double)request.PageSize);

                return new GamesPageDto(totalPages, gamesOnPage);
            })
            .AllowAnonymous();
        */
    }
}
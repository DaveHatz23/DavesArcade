using DavesArcade.Api.Extensions;
using DavesArcade.Application.DTOs;
using DavesArcade.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DavesArcade.Api.Endpoints.Games.GetGames;

public static class GetGamesV2Endpoint
{
    /// <summary>
    /// Maps the GET /v2/games endpoint with enhanced filtering capabilities.
    /// V2 adds support for genre filtering and pagination.
    /// </summary>
    public static RouteHandlerBuilder MapGetGamesV2(this IEndpointRouteBuilder app)
    {
        return app.MapGet("/", async (
            IGameRepository gameRepository,
            ILogger<Program> logger,
            [FromQuery] string? genre = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10) =>
        {
            logger.LogInformation("V2: Fetching games with filters - Genre: {Genre}, Page: {PageNumber}, PageSize: {PageSize}", 
                genre ?? "All", pageNumber, pageSize);

            var result = await gameRepository.GetAllAsync();

            if (!result.IsSuccess)
            {
                logger.LogWarning("V2: Failed to fetch games");
                return result.ToHttpResult();
            }

            var games = result.Value!;

            // Apply genre filter if specified
            if (!string.IsNullOrWhiteSpace(genre))
            {
                games = games.Where(g => g.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase));
                logger.LogInformation("V2: Filtered to {Count} games in genre {Genre}", games.Count(), genre);
            }

            // Apply pagination
            var totalCount = games.Count();
            var pagedGames = games
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            logger.LogInformation("V2: Returning {Count} of {Total} games (Page {Page})", 
                pagedGames.Count, totalCount, pageNumber);

            var response = new GamesV2Response(
                Data: pagedGames,
                Pagination: new PaginationInfo(
                    CurrentPage: pageNumber,
                    PageSize: pageSize,
                    TotalCount: totalCount,
                    TotalPages: (int)Math.Ceiling(totalCount / (double)pageSize)
                ),
                Filters: new FilterInfo(Genre: genre)
            );

            return Results.Ok(response);
        })
        .Produces<GamesV2Response>(200)
        .Produces(500)
        .WithName("GetGamesV2")
        .WithSummary("Get all games with filtering and pagination (V2)")
        .WithDescription("Enhanced version with genre filtering and pagination support");
    }
}

using DavesArcade.Api.Endpoints.Games.GetGame;
using DavesArcade.Application.Interfaces;
using DavesArcade.Application.DTOs;
using DavesArcade.Application.Interfaces;
using DavesArcade.Application.Results;
using DavesArcade.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DavesArcade.Application.Results;

namespace DavesArcade.Api.Endpoints.Games.CreateGame
{
    public static class CreateGameEndpoint
    {
        private const string DefaultImageUri = "https://placehold.co/100";

        public static void MapCreateGame(this IEndpointRouteBuilder app)
        {
            app.MapPost("/", async (
                    CreateGameRequest createGameRequest,
                    IGameRepository gameRepository) =>
                {
                    var result = await gameRepository.CreateAsync(createGameRequest);

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

                    return Results.Created($"/games/{result.Value!.Id}", result.Value);
                })
                .AllowAnonymous();

        /*
        app.MapPost("/", async(
            [FromForm] CreateGameRequest gameDto,
            GameStoreContext dbContext,
            ILogger<Program> logger,
            FileUploader fileUploader,
            ClaimsPrincipal user,
            CdnUrlTransformer cdnUrlTransformer) =>
        {
            if (user?.Identity?.IsAuthenticated == false)
            {
                return Results.Unauthorized();
            }

            var currentUserId = user?.FindFirstValue(JwtRegisteredClaimNames.Email)
                                ?? user?.FindFirstValue(GameStoreClaimTypes.UserId);

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Results.Unauthorized();
            }

var imageUri = DefaultImageUri;

if (gameDto.ImageFile is not null)
{
    var fileUploadResult = await fileUploader.UploadFileAsync(
        gameDto.ImageFile,
        StorageNames.GameImagesFolder
    );

    if (!fileUploadResult.IsSucess)
    {
        return Results.BadRequest(new { message = fileUploadResult.ErrorMessage });
    }

    imageUri = fileUploadResult.FileUrl;
}

var game = new Game
{
    Name = gameDto.Name,
    GenreId = gameDto.GenreId,
    Price = gameDto.Price,
    ReleaseDate = gameDto.ReleaseDate,
    Description = gameDto.Description,
    ImageUri = imageUri!,
    LastUpdatedBy = currentUserId
};

dbContext.Games.Add(game);

await dbContext.SaveChangesAsync();

logger.LogInformation(
    "Created game {GameName} with price {GamePrice}",
    game.Name,
    game.Price);

return Results.CreatedAtRoute(
    EndpointNames.GetGame,
    new { id = game.Id },
    new GameDetailsDto(
        game.Id,
        game.Name,
        game.GenreId,
        game.Price,
        game.ReleaseDate,
        game.Description,
        cdnUrlTransformer.TransformToCdnUrl(game.ImageUri),
        game.LastUpdatedBy
    ));
        })
        .WithParameterValidation()
        .DisableAntiforgery()
        .RequireAuthorization(Policies.AdminAccess);

        */
    }
    }
       
}

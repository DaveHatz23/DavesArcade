using System.ComponentModel.DataAnnotations;

namespace DavesArcade.Api.Endpoints.Games.CreateGame;

// TODO : Currently ImageFile is optional, but we may want to make it required in the future.
// If we do, we should add a validation attribute to enforce that.
// *** Currently NOT Implemented **** 
// we should also add validation to ensure that the uploaded file is an image and is below a certain size limit (e.g., 5MB)
// to prevent abuse and ensure optimal performance. We can create a custom validation attribute for this purpose. ***

public record CreateGameDto(
    [Required][StringLength(50)] string Name,
    Guid GenreId,
    [Range(1, 100)] decimal Price,
    DateOnly ReleaseDate,
    [Required][StringLength(500)] string Description
)
{
    public IFormFile? ImageFile { get; set; }
}

public record GameDetailsDto(
    Guid Id,
    string Name,
    Guid GenreId,
    decimal Price,
    DateOnly ReleaseDate,
    string Description,
    string ImageUri,
    string LastUpdatedBy);
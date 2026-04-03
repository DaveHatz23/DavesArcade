namespace DavesArcade.Application.DTOs;

public record CreateGameRequest(
    string Name,
    Guid GenreId,
    decimal Price,
    DateOnly ReleaseDate,
    string Description,
    string? ImageUri = null,
    string LastUpdatedBy = "system"
);

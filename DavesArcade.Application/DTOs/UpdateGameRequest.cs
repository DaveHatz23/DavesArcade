using System.ComponentModel.DataAnnotations;

namespace DavesArcade.Application.DTOs;

public record UpdateGameRequest(
    [Required][StringLength(50)] string Name,
    Guid GenreId,
    [Range(1, 100)] decimal Price,
    DateOnly ReleaseDate,
    [Required][StringLength(500)] string Description
);
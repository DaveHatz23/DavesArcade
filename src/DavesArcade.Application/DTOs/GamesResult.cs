namespace DavesArcade.Application.DTOs;

public record GameResultDto(
    Guid Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate,
    string ImageUri,
    string LastUpdatedBy
);

/// <summary>
/// Response for V2 GetGames endpoint with pagination and filtering.
/// </summary>
public record GamesV2Response(
    IEnumerable<GameResultDto> Data,
    PaginationInfo Pagination,
    FilterInfo Filters
);

/// <summary>
/// Pagination metadata for V2 responses.
/// </summary>
public record PaginationInfo(
    int CurrentPage,
    int PageSize,
    int TotalCount,
    int TotalPages
);

/// <summary>
/// Filter information for V2 responses.
/// </summary>
public record FilterInfo(
    string? Genre
);
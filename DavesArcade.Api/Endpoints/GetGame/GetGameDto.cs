using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavesArcade.Api.Endpoints.GetGame;

public record GameDetailsDto(
    Guid Id,
    string Name,
    Guid GenreId,
    decimal Price,
    DateOnly ReleaseDate,
    string Description,
    string ImageUri,
    string LastUpdatedBy);

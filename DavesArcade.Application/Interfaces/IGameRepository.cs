using DavesArcade.Application.DTOs;
using DavesArcade.Application.Results;

namespace DavesArcade.Application.Interfaces;

public interface IGameRepository
{
    Task<Result<IEnumerable<GameResultDto>>> GetAllAsync();

    Task<Result<GameResultDto>> GetByIdAsync(Guid id);
    Task<bool> DeleteByIdAsync(Guid id);
}
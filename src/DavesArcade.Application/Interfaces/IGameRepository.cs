using DavesArcade.Application.DTOs;
using DavesArcade.Application.Results;

namespace DavesArcade.Application.Interfaces;

public interface IGameRepository
{
    Task<Result<IEnumerable<GameResultDto>>> GetAllAsync();

    Task<Result<GameResultDto>> GetByIdAsync(Guid id);

    Task<Result<GameResultDto>> CreateAsync(CreateGameRequest createGameRequest);

    Task<Result<GameResultDto>> UpdateAsync(Guid id, UpdateGameRequest updateGameRequest);

    Task<Result<bool>> DeleteByIdAsync(Guid id);
}
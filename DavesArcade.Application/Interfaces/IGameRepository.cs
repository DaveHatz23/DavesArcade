using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DavesArcade.Application.DTOs;

namespace DavesArcade.Application.Interfaces
{
    public interface IGameRepository
    {
        Task<IEnumerable<GameResultDto>> GetAllAsync();

        Task<GameResultDto?> GetByIdAsync(Guid id);
    }
}

// Services/Interfaces/IVideoGameQueryService.cs
using VideoGame.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VideoGame.Services.Interfaces
{
    public interface IVideoGameQueryService
    {
        Task<IEnumerable<VideoGameModel>> GetAllGamesAsync();
        Task<VideoGameModel?> GetGameByIdAsync(int id);
    }
}

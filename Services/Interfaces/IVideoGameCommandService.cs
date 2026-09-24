
// Services/Interfaces/IVideoGameCommandService.cs
using VideoGame.Domain.Entities;
using System.Threading.Tasks;

namespace VideoGame.Services.Interfaces
{
    public interface IVideoGameCommandService
    {
        Task<VideoGameModel> CreateGameAsync(VideoGameModel game);
        Task<bool> UpdateGameAsync(int id, VideoGameModel game);
    }
}

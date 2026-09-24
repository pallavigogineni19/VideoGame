

// Data/Repositories/Interfaces/IVideoGameReadOnlyRepository.cs
using VideoGame.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VideoGame.Data.Repositories.Interfaces
{
    public interface IVideoGameReadOnlyRepository
    {
        Task<IEnumerable<VideoGameModel>> GetAllAsync();
        Task<VideoGameModel?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}

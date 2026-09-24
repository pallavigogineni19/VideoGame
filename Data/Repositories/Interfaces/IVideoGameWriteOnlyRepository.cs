// Data/Repositories/Interfaces/IVideoGameWriteOnlyRepository.cs
using VideoGame.Domain.Entities;
using System.Threading.Tasks;

namespace VideoGame.Data.Repositories.Interfaces
{
    public interface IVideoGameWriteOnlyRepository
    {
        Task AddAsync(VideoGameModel game);
        Task UpdateAsync(VideoGameModel game);
    }
}




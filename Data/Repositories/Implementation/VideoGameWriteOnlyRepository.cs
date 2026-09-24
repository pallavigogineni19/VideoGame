// Data/Repositories/Implementations/VideoGameWriteOnlyRepository.cs
using VideoGame.Domain.Entities;
using VideoGame.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace VideoGame.Data.Repositories.Implementations
{
    public class VideoGameWriteOnlyRepository : IVideoGameWriteOnlyRepository
    {
        private readonly ApplicationDbContext _context;

        public VideoGameWriteOnlyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(VideoGameModel game)
        {
            await _context.VideoGames.AddAsync(game);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(VideoGameModel game)
        {
            _context.Entry(game).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}

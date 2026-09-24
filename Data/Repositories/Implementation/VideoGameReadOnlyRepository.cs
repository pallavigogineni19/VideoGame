

// Data/Repositories/Implementations/VideoGameReadOnlyRepository.cs
using VideoGame.Domain.Entities;
using VideoGame.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VideoGame.Data.Repositories.Implementations
{
    public class VideoGameReadOnlyRepository : IVideoGameReadOnlyRepository
    {
        private readonly ApplicationDbContext _context;

        public VideoGameReadOnlyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VideoGameModel>> GetAllAsync()
        {
            // AsNoTracking completely optimizes read-only performance
            return await _context.VideoGames.AsNoTracking().ToListAsync();
        }

        public async Task<VideoGameModel?> GetByIdAsync(int id)
        {
            // We use AsNoTracking here as well so the entity isn't cached in memory
            return await _context.VideoGames.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.VideoGames.AnyAsync(e => e.Id == id);
        }
    }
}





// Services/Implementations/VideoGameCommandService.cs
using VideoGame.Data.Repositories.Interfaces;
using VideoGame.Domain.Entities;
using VideoGame.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace VideoGame.Services.Implementations
{
    public class VideoGameCommandService : IVideoGameCommandService
    {
        private readonly IVideoGameWriteOnlyRepository _writeRepository;
        private readonly IVideoGameReadOnlyRepository _readRepository; // Needed for state checks
        private readonly ILogger<VideoGameCommandService> _logger;

        public VideoGameCommandService(
            IVideoGameWriteOnlyRepository writeRepository,
            IVideoGameReadOnlyRepository readRepository,
            ILogger<VideoGameCommandService> logger)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
            _logger = logger;
        }

        public async Task<VideoGameModel> CreateGameAsync(VideoGameModel game)
        {
            _logger.LogInformation("Business Command: Processing verification bounds for new title entry: {Title}", game.Title);

            // Custom business logic validation rule check example
            if (game.ReleaseDate > DateTime.UtcNow.AddYears(5))
            {
                throw new ArgumentException("Release date cannot be set too far in the future.");
            }

            await _writeRepository.AddAsync(game);
            return game;
        }

        public async Task<bool> UpdateGameAsync(int id, VideoGameModel game)
        {
            _logger.LogInformation("Business Command: Committing modification requests for Target ID: {Id}", id);

            if (id != game.Id) return false;

            // Safe lookup checks using your optimized performance method
            if (!await _readRepository.ExistsAsync(id))
            {
                _logger.LogWarning("Business Command Update failed: Target ID {Id} does not exist in the system database context.", id);
                return false;
            }

            await _writeRepository.UpdateAsync(game);
            return true;
        }
    }
}

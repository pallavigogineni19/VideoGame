// Services/Implementations/VideoGameQueryService.cs
using VideoGame.Data.Repositories.Interfaces;
using VideoGame.Domain.Entities;
using VideoGame.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VideoGame.Services.Implementations
{
    public class VideoGameQueryService : IVideoGameQueryService
    {
        private readonly IVideoGameReadOnlyRepository _readRepository;
        private readonly ILogger<VideoGameQueryService> _logger;

        public VideoGameQueryService(IVideoGameReadOnlyRepository readRepository, ILogger<VideoGameQueryService> logger)
        {
            _readRepository = readRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<VideoGameModel>> GetAllGamesAsync()
        {
            _logger.LogInformation("Business Query: Retrieving full game list for frontend browse dashboard.");
            return await _readRepository.GetAllAsync();
        }

        public async Task<VideoGameModel?> GetGameByIdAsync(int id)
        {
            _logger.LogInformation("Business Query: Fetching operational details for Game ID: {Id}", id);
            return await _readRepository.GetByIdAsync(id);
        }
    }
}

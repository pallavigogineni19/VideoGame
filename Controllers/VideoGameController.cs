// Controllers/VideoGamesController.cs
using VideoGame.Domain.Entities;
using VideoGame.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VideoGame.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideoGamesController : ControllerBase
    {
        private readonly IVideoGameQueryService _queryService;
        private readonly IVideoGameCommandService _commandService;

        // Controller cleanly demands both separated operation services
        public VideoGamesController(IVideoGameQueryService queryService, IVideoGameCommandService commandService)
        {
            _queryService = queryService;
            _commandService = commandService;
        }

        // PAGE 1 REQUIREMENT: Browse Inventory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VideoGameModel>>> GetGames()
        {
            var games = await _queryService.GetAllGamesAsync();
            return Ok(games);
        }

        // PAGE 2 REQUIREMENT: Fetch existing entry details for editing injection
        [HttpGet("{id}")]
        public async Task<ActionResult<VideoGameModel>> GetGame(int id)
        {
            var game = await _queryService.GetGameByIdAsync(id);
            if (game == null) return NotFound();
            return Ok(game);
        }

        // PAGE 2 REQUIREMENT: Create New Entry
        [HttpPost]
        public async Task<ActionResult<VideoGameModel>> PostGame(VideoGameModel game)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var createdGame = await _commandService.CreateGameAsync(game);
                return CreatedAtAction(nameof(GetGame), new { id = createdGame.Id }, createdGame);
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PAGE 2 REQUIREMENT: Save Changes to Existing Entry
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, VideoGameModel game)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var success = await _commandService.UpdateGameAsync(id, game);
            if (!success) return NotFound($"Game record with ID {id} could not be updated.");

            return NoContent();
        }
    }
}

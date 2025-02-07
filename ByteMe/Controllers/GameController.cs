using ByteMe.API.Services.Interfaces;
using ByteMe.API.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using ByteMe.API.DTOs;

namespace ByteMe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartGame([FromBody] StartGameRequest request)
        {
            var game = await _gameService.StartNewGameAsync(request.PlayerOne, request.PlayerTwo);
            return Ok(game);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetGameStatus(int id)
        {
            var game = await _gameService.GetGameStatusAsync(id);
            if (game == null) return NotFound();
            return Ok(game);
        }

        [HttpPost("{id}/submit-scores")]
        public async Task<IActionResult> SubmitScores(int id, [FromBody] SubmitScoresRequest request)
        {
            await _gameService.SubmitScoreAsync(id, request.PlayerOneScore, request.PlayerTwoScore);
            return NoContent();
        }

    }
}

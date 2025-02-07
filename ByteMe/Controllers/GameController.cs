using ByteMe.API.Services.Interfaces;
using ByteMe.API.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using ByteMe.Shared.DTOs;
using ByteMe.API.Services;

namespace ByteMe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly GameSessionService _gameSessionService;


        public GameController(IGameService gameService, GameSessionService gameSessionService)
        {
            _gameService = gameService;
            _gameSessionService = gameSessionService;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartGame([FromBody] StartGameRequest request)
        {
            var game = await _gameService.StartNewGameAsync(request.PlayerOne, request.PlayerTwo);
            return Ok(game);
        }


        [HttpGet("{id}/status")]
        public IActionResult GetGameStatus(string id)
        {
            if (_gameSessionService.TryGetGameSession(id, out var gameSession))
            {
                return Ok(gameSession);
            }
            return NotFound();
        }


        [HttpPost("{id}/submit-scores")]
        public async Task<IActionResult> SubmitScores(int id, [FromBody] SubmitScoresRequest request)
        {
            await _gameService.SubmitScoreAsync(id, request.PlayerOneScore, request.PlayerTwoScore);
            return NoContent();
        }

    }
}

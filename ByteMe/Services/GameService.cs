using ByteMe.API.Data.Entities;
using ByteMe.API.Repositories.Interfaces;
using ByteMe.API.Services.Interfaces;

namespace ByteMe.API.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<Game> StartNewGameAsync(string playerOne, string playerTwo)
        {
            var game = new Game
            {
                PlayerOne = playerOne,
                PlayerTwo = playerTwo,
                IsCompleted = false
            };

            return await _gameRepository.CreateGameAsync(game);
        }

        public async Task<Game> GetGameStatusAsync(int gameId)
        {
            return await _gameRepository.GetGameByIdAsync(gameId);
        }

        public async Task SubmitScoreAsync(int gameId, int playerOneScore, int playerTwoScore)
        {
            var game = await _gameRepository.GetGameByIdAsync(gameId);
            if (game == null) throw new Exception("Game not found");

            game.PlayerOneScore = playerOneScore;
            game.PlayerTwoScore = playerTwoScore;
            game.IsCompleted = true;

            await _gameRepository.UpdateGameAsync(game);
        }
    }
}

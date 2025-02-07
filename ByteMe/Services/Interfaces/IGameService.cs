using ByteMe.API.Data.Entities;

namespace ByteMe.API.Services.Interfaces
{
    public interface IGameService
    {
        Task<Game> StartNewGameAsync(string playerOne, string playerTwo);
        Task<Game> GetGameStatusAsync(int gameId);
        Task SubmitScoreAsync(int gameId, int playerOneScore, int playerTwoScore);
    }
}

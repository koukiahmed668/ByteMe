using ByteMe.API.Data.Entities;

namespace ByteMe.API.Repositories.Interfaces
{
    public interface IGameRepository
    {
        Task<Game> CreateGameAsync(Game game);
        Task<Game> GetGameByIdAsync(int id);
        Task UpdateGameAsync(Game game);
    }
}

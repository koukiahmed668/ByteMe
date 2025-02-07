using ByteMe.Shared.DTOs;
using System.Collections.Concurrent;

namespace ByteMe.API.Services
{
    public class GameSessionService
    {
        private readonly ConcurrentDictionary<string, GameSession> _activeGames = new();

        public bool TryGetGameSession(string gameId, out GameSession gameSession)
        {
            return _activeGames.TryGetValue(gameId, out gameSession);
        }

        public void AddGameSession(GameSession gameSession)
        {
            _activeGames[gameSession.GameId] = gameSession;
        }

        public void RemoveGameSession(string gameId)
        {
            _activeGames.TryRemove(gameId, out _);
        }
    }
}

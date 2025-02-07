using ByteMe.Shared.DTOs;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent; // Reference the shared DTO

public class GameHub : Hub
{
    private readonly ConcurrentQueue<string> _waitingPlayers;
    private readonly ConcurrentDictionary<string, ByteMe.Shared.DTOs.GameSession> _activeGames;

    public GameHub(
        ConcurrentQueue<string> waitingPlayers,
        ConcurrentDictionary<string, ByteMe.Shared.DTOs.GameSession> activeGames)
    {
        _waitingPlayers = waitingPlayers;
        _activeGames = activeGames;
    }

    public async Task JoinMatchmaking(string playerName)
    {
        _waitingPlayers.Enqueue(playerName);

        if (_waitingPlayers.Count >= 2)
        {
            if (_waitingPlayers.TryDequeue(out string playerOne) &&
                _waitingPlayers.TryDequeue(out string playerTwo))
            {
                var gameId = Guid.NewGuid().ToString();
                var gameSession = new ByteMe.Shared.DTOs.GameSession
                {
                    GameId = gameId,
                    PlayerOne = playerOne,
                    PlayerTwo = playerTwo,
                    Questions = GenerateQuestions()
                };

                _activeGames[gameId] = gameSession;

                await Clients.Client(Context.ConnectionId).SendAsync("MatchFound", gameId, playerOne, playerTwo);
                await Clients.All.SendAsync("GameStarted", gameSession);
            }
        }
    }

    public async Task SubmitAnswer(string gameId, string playerName, string answer)
    {
        if (!_activeGames.TryGetValue(gameId, out var gameSession))
            return;

        gameSession.SubmitAnswer(playerName, answer);

        await Clients.Group(gameId).SendAsync("ScoreUpdated", gameSession);
    }

    private List<ByteMe.Shared.DTOs.QuestionDto> GenerateQuestions()
    {
        return new List<ByteMe.Shared.DTOs.QuestionDto>
        {
            new ByteMe.Shared.DTOs.QuestionDto { Question = "2 + 2", CorrectAnswer = "4" },
            new ByteMe.Shared.DTOs.QuestionDto { Question = "5 * 3", CorrectAnswer = "15" }
        };
    }
}

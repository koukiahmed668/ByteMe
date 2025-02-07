using Moq;
using Microsoft.AspNetCore.SignalR;
using ByteMe.Shared.DTOs;
using System.Collections.Concurrent;

namespace ByteMe.Tests
{
    public class GameHubTests
    {
        private readonly Mock<IHubCallerClients> _mockClients;
        private readonly Mock<HubCallerContext> _mockContext;
        private readonly Mock<IClientProxy> _mockClientProxy;
        private readonly Mock<ISingleClientProxy> _mockSingleClientProxy;
        private readonly Mock<IClientProxy> _mockGroupClientProxy; // Mock for the Group method
        private readonly ConcurrentQueue<string> _waitingPlayers;
        private readonly ConcurrentDictionary<string, GameSession> _activeGames;
        private readonly GameHub _gameHub;

        public GameHubTests()
        {
            _mockClients = new Mock<IHubCallerClients>();
            _mockContext = new Mock<HubCallerContext>();
            _mockClientProxy = new Mock<IClientProxy>();
            _mockSingleClientProxy = new Mock<ISingleClientProxy>();
            _mockGroupClientProxy = new Mock<IClientProxy>(); // Create mock for the group client

            // Mock Clients.Client to return a single client proxy
            _mockClients.Setup(clients => clients.Client(It.IsAny<string>())).Returns(_mockSingleClientProxy.Object);

            // Mock Clients.Group to return a group client proxy
            _mockClients.Setup(clients => clients.Group(It.IsAny<string>())).Returns(_mockGroupClientProxy.Object);

            _mockClients.Setup(clients => clients.All).Returns(_mockClientProxy.Object);

            _waitingPlayers = new ConcurrentQueue<string>();
            _activeGames = new ConcurrentDictionary<string, GameSession>();

            _gameHub = new GameHub(_waitingPlayers, _activeGames)
            {
                Clients = _mockClients.Object,
                Context = _mockContext.Object
            };
        }

        [Fact]
        public async Task JoinMatchmaking_WithTwoPlayers_ShouldTriggerMatchFoundAndGameStarted()
        {
            // Arrange
            var playerOne = "PlayerOne";
            var playerTwo = "PlayerTwo";
            var connectionIdOne = "connectionId1";
            var connectionIdTwo = "connectionId2";

            _mockContext.Setup(c => c.ConnectionId).Returns(connectionIdOne);

            // Act
            await _gameHub.JoinMatchmaking(playerOne);
            _mockContext.Setup(c => c.ConnectionId).Returns(connectionIdTwo);
            await _gameHub.JoinMatchmaking(playerTwo);

            // Assert
            _mockSingleClientProxy.Verify(client => client.SendCoreAsync(
                "MatchFound", It.IsAny<object[]>(), default), Times.Once);
            _mockClientProxy.Verify(client => client.SendCoreAsync(
                "GameStarted", It.IsAny<object[]>(), default), Times.Once);
        }

        [Fact]
        public async Task SubmitAnswer_ValidGameSession_ShouldNotifyScoreUpdate()
        {
            // Arrange
            var gameId = "test-game-id";
            var playerOne = "PlayerOne";
            var playerTwo = "PlayerTwo";
            var question = new QuestionDto { Question = "2 + 2", CorrectAnswer = "4" };
            var gameSession = new GameSession
            {
                GameId = gameId,
                PlayerOne = playerOne,
                PlayerTwo = playerTwo,
                Questions = new List<QuestionDto> { question }
            };

            _activeGames[gameId] = gameSession;

            // Act
            await _gameHub.SubmitAnswer(gameId, playerOne, "4");

            // Assert
            _mockGroupClientProxy.Verify(client => client.SendCoreAsync(
                "ScoreUpdated", It.IsAny<object[]>(), default), Times.Once);
        }
    }
}

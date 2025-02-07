using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ByteMe.API.Hubs
{
    public class GameHub : Hub
    {
        // Notify when a player joins the game
        public async Task JoinGame(string playerName)
        {
            await Clients.All.SendAsync("PlayerJoined", playerName);
        }

        // Broadcast scores to all connected clients
        public async Task SubmitScore(string playerName, int score)
        {
            await Clients.All.SendAsync("ScoreSubmitted", playerName, score);
        }

        // Notify when the game ends
        public async Task EndGame(string winner)
        {
            await Clients.All.SendAsync("GameEnded", winner);
        }
    }
}

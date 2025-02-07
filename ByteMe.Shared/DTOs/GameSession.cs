namespace ByteMe.Shared.DTOs
{
    public class GameSession
    {
        public string GameId { get; set; }
        public string PlayerOne { get; set; }
        public string PlayerTwo { get; set; }
        public List<QuestionDto> Questions { get; set; } = new();
        public Dictionary<string, int> Scores { get; set; } = new();

        public void SubmitAnswer(string playerName, string answer)
        {
            var question = Questions.FirstOrDefault(q => q.CorrectAnswer == answer);
            if (question != null)
            {
                Scores[playerName] = Scores.GetValueOrDefault(playerName, 0) + 10;
            }
        }
    }
}

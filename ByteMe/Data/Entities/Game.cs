using System.ComponentModel.DataAnnotations;

namespace ByteMe.API.Data.Entities
{
    public class Game
    {
        public int Id { get; set; }

        [MaxLength(255)] 
        public string PlayerOne { get; set; }

        [MaxLength(255)]
        public string PlayerTwo { get; set; }

        public int PlayerOneScore { get; set; }
        public int PlayerTwoScore { get; set; }
        public bool IsCompleted { get; set; }
    }
}

namespace CornHoleRevamp.Models
{
    public class Stats
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TotalGames { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Ties { get; set; }
        public int TotalPoints { get; set; }
        public int TotalRounds { get; set; }

        // Navigation property
        public User? User { get; set; }
    }
}
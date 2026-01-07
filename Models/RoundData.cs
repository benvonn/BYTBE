namespace CornHoleRevamp.Models
{
    public class RoundData
    {
        public int Id { get; set; }
        public int GameMatchId { get; set; }
        public int RoundNumber { get; set; }
        public int Player1RoundScore { get; set; }
        public int Player2RoundScore { get; set; }
        public int Player1TotalBefore { get; set; }
        public int Player2TotalBefore { get; set; }
        public int? Player1RoundBagsIn { get; set; }
        public int? Player1RoundBagsOn { get; set; }
        public int? Player2RoundBagsIn { get; set; }
        public int? Player2RoundBagsOn { get; set; }
        public DateTime Timestamp { get; set; }

        // Navigation property
        public GameMatch GameMatch { get; set; } = null!;
    }
}
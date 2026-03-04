namespace CornHoleRevamp.Models
{
    public class GameMatch
    {
        public int Id { get; set; }
        public int Player1Id { get; set; }        // From frontend player1.id
        public int Player2Id { get; set; }        // From frontend player2.id
        public string Player1Name { get; set; } = string.Empty;  // From frontend player1.name
        public string Player2Name { get; set; } = string.Empty;  // From frontend player2.name
        public int Player1Score { get; set; }     // From frontend player1.score
        public int Player2Score { get; set; }     // From frontend player2.score
        public int? WinnerId { get; set; }
        public string WinnerName { get; set; } = string.Empty;   // From frontend winner
        public DateTime PlayedAt { get; set; }
        public int TotalRounds { get; set; }      // From frontend totalRounds

        // Navigation properties
        public User? Player1 { get; set; }
        public User? Player2 { get; set; }

        // Navigation property for rounds
        public List<RoundData> Rounds { get; set; } = new List<RoundData>();
    }
}
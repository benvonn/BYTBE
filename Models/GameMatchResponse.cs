namespace CornHoleRevamp.Models
{
    public class GameMatchResponseDto
    {
        public int Id { get; set; }
        public int Player1Id { get; set; }
        public int Player2Id { get; set; }
        public string Player1Name { get; set; } = string.Empty;
        public string Player2Name { get; set; } = string.Empty;
        public int Player1Score { get; set; }
        public int Player2Score { get; set; }
        public string WinnerName { get; set; } = string.Empty;
        public DateTime PlayedAt { get; set; }
        public string BoardType { get; set; } = string.Empty;
        public int TotalRounds { get; set; }
        public List<RoundDataDto> Rounds { get; set; } = new List<RoundDataDto>();
    }
}
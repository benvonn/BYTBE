using System.Text.Json.Serialization;

namespace CornHoleRevamp.Models
{
    public class RoundDataDto
    {
        [JsonPropertyName("roundNumber")]
        public int RoundNumber { get; set; }

        [JsonPropertyName("player1RoundScore")]
        public int Player1RoundScore { get; set; }

        [JsonPropertyName("player2RoundScore")]
        public int Player2RoundScore { get; set; }

        [JsonPropertyName("player1TotalBefore")]
        public int Player1TotalBefore { get; set; }

        [JsonPropertyName("player2TotalBefore")]
        public int Player2TotalBefore { get; set; }

        [JsonPropertyName("player1RoundBagsIn")]
        public int? Player1RoundBagsIn { get; set; }

        [JsonPropertyName("player1RoundBagsOn")]
        public int? Player1RoundBagsOn { get; set; }

        [JsonPropertyName("player2RoundBagsIn")]
        public int? Player2RoundBagsIn { get; set; }

        [JsonPropertyName("player2RoundBagsOn")]
        public int? Player2RoundBagsOn { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime? Timestamp { get; set; }
    }
}
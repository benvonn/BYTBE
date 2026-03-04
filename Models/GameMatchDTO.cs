using System.Text.Json.Serialization;

namespace CornHoleRevamp.Models
{
    public class GameMatchDto
    {
        [JsonPropertyName("date")]
        public string Date { get; set; } = string.Empty;

        [JsonPropertyName("totalRounds")]
        public int TotalRounds { get; set; }

        [JsonPropertyName("winner")]
        public string Winner { get; set; } = string.Empty;

        [JsonPropertyName("player1")]
        public PlayerData Player1 { get; set; } = new PlayerData();

        [JsonPropertyName("player2")]
        public PlayerData Player2 { get; set; } = new PlayerData();

        [JsonPropertyName("rounds")]
        public List<RoundDataDto> Rounds { get; set; } = new List<RoundDataDto>();

        // Add the extra fields that exist in your frontend data
        [JsonPropertyName("player1TotalBagsIn")]
        public int Player1TotalBagsIn { get; set; }

        [JsonPropertyName("player1TotalBagsOn")]
        public int Player1TotalBagsOn { get; set; }

        [JsonPropertyName("player2TotalBagsIn")]
        public int Player2TotalBagsIn { get; set; }

        [JsonPropertyName("player2TotalBagsOn")]
        public int Player2TotalBagsOn { get; set; }
    }

    public class PlayerData
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("score")]
        public int Score { get; set; }
    }
}
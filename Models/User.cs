using CornHoleRevamp.Service;
using System.Text.Json.Serialization;

namespace CornHoleRevamp.Models
{

    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        [JsonIgnore]
        public string PasswordHash { get; set; }

        public DateTime? OfflineTokenExpiry { get; set; }
    }
}

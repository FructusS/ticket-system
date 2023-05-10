using System.Text.Json.Serialization;

namespace TicketSystem.Shared.ViewModels
{
    public class UserRequestModel
    {
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
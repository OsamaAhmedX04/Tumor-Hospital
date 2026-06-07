using System.Text.Json.Serialization;

namespace TumorHospital.Application.DTOs.Request.ML
{
    public class ChatRequestDto
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }
}

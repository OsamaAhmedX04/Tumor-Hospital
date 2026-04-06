using Newtonsoft.Json;

namespace TumorHospital.Application.DTOs.Response.Meeting
{
    public class CreateMeetingDto
    {
        public long Id { get; set; }

        [JsonProperty("join_url")]
        public string JoinUrl { get; set; }

        [JsonProperty("start_url")]
        public string StartUrl { get; set; }

        public string Password { get; set; }
    }
}

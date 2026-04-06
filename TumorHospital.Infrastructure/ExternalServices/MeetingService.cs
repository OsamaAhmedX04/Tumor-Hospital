using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using TumorHospital.Application.DTOs.Response.Meeting;
using TumorHospital.Application.Intefaces.ExternalServices;
using TumorHospital.Infrastructure.Settings;

namespace TumorHospital.Infrastructure.ExternalServices
{
    public class MeetingService : IMeetingService
    {
        private readonly HttpClient _httpClient;
        private readonly ZoomSettings _settings;

        public MeetingService(HttpClient httpClient, IOptions<ZoomSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<CreateMeetingDto> CreateMeeting(DateTime startTime)
        {
            var token = await GetAccessToken();

            var meeting = new
            {
                topic = "Doctor-Patient Meeting",
                type = 2,
                start_time = startTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"),
                duration = 60, // minutes
                settings = new
                {
                    host_video = true,
                    participant_video = true,
                    join_before_host = false,
                    waiting_room = true,
                    mute_upon_entry = true,
                    approval_type = 0,
                    audio = "both",
                    auto_recording = "none"
                }
            };

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://api.zoom.us/v2/users/me/meetings"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            request.Content = new StringContent(
                JsonConvert.SerializeObject(meeting),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.SendAsync(request);

            var json = await response.Content.ReadAsStringAsync();

            var meetingResponse =
                JsonConvert.DeserializeObject<CreateMeetingDto>(json);

            return meetingResponse;
        }

        private async Task<string> GetAccessToken()
        {
            var url =
            $"https://zoom.us/oauth/token?grant_type=account_credentials&account_id={_settings.AccountId}";

            var request = new HttpRequestMessage(HttpMethod.Post, url);

            var auth = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{_settings.ClientId}:{_settings.ClientSecret}")
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Basic", auth);

            var response = await _httpClient.SendAsync(request);

            var json = await response.Content.ReadAsStringAsync();

            dynamic data = JsonConvert.DeserializeObject(json);

            return data.access_token;
        }
    }
}

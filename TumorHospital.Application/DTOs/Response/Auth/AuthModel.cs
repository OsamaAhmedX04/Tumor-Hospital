namespace TumorHospital.Application.DTOs.Response.Auth
{
    public class AuthModel
    {
        public string Message { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}

namespace TumorHospital.WebAPI.DTOs.AuthDto
{
    public class RefreshTokenDto
    {
        public string ExpiredToken { get; set; }
        public string RefreshToken { get; set; }
    }
}

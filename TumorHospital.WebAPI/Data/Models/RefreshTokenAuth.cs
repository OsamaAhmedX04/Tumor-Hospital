using System.ComponentModel.DataAnnotations.Schema;

namespace TumorHospital.WebAPI.Data.Models
{
    public class RefreshTokenAuth
    {
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; } = DateTime.UtcNow.AddDays(20);
    }
}

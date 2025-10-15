using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TumorHospital.WebAPI.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(40)]
        public string FirstName { get; set; }

        [Required, MaxLength(40)]
        public string LasttName { get; set; }

        public RefreshTokenAuth RefreshTokenAuth { get; set; }
    }
}

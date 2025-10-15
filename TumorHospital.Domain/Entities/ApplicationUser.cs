using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TumorHospital.Domain.Entities
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

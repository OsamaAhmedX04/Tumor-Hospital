using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TumorHospital.Application.DTOs.Response.User
{
    public class ReceptionistProfileResponse
    {
        public string ApplicationUserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public string Gender { get; set; }
        public string Address { get; set; }
    }
}

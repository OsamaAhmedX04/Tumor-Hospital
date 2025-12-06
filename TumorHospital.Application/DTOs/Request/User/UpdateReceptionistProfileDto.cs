using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Application.DTOs.Request.User
{
    public class UpdateReceptionistProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public string Gender { get; set; }
        public string Address { get; set; }
    }
}

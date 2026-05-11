using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TumorHospital.Application.DTOs.Request.User
{
    public class UpdateSpecializationDto
    {
        public string Name { get; set; }
        public string Description { get; set; } = "N/A";
    }
}

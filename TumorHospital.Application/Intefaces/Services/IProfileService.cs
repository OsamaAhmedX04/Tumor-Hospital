using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IProfileService
    {
        Task UploadProfilePicture(IFormFile file, string userId);
    }
}

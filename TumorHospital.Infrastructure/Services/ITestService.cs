using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TumorHospital.Infrastructure.Services
{
    public interface ITestService
    {
        Task UploadFile(IFormFile file);
        Task<List<string>> GetAllFiles();
    }
}

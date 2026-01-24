using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Application.DTOs.Response.Donation;
using TumorHospital.Application.DTOs.Response.Pagination;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IVolunteerService
    {
        Task<PageSourcePagination<VolunteerInfoDto>> GetAllVolunteers(int pageSize, int pageNumber);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Application.DTOs.Request.Auth;
using TumorHospital.Application.DTOs.Response.Pagination;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;

namespace TumorHospital.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PageSourcePagination<UserDto>> GetAllAdmins(int pageSize, int pageNumber)
        {
            return await _unitOfWork.Admins.GetAllPaginatedEnhancedAsync(
                selector: admin => new UserDto
                {
                    Name = admin.User.FirstName + " " + admin.User.LastName,
                    Email = admin.User.Email!,
                    Role = "Admin"
                },
                pageSize: pageSize,
                pageNumber: pageNumber
                );
        }

        public async Task<PageSourcePagination<UserDto>> GetAllDoctors(int pageSize, int pageNumber)
        {
            return await _unitOfWork.Doctors.GetAllPaginatedEnhancedAsync(
                filter: doctor => doctor.User.IsActive,
                selector: doctor => new UserDto
                {
                    Name = doctor.User.FirstName + " " + doctor.User.LastName,
                    Email = doctor.User.Email!,
                    Role = "Doctor"
                },
                pageSize: pageSize,
                pageNumber: pageNumber
                );
        }

        public async Task<PageSourcePagination<UserDto>> GetAllInActiveDoctorRoles(int pageSize, int pageNumber)
        {
            return await _unitOfWork.Doctors.GetAllPaginatedEnhancedAsync(
                filter: doctor => !doctor.User.IsActive,
                selector: doctor => new UserDto
                {
                    Name = doctor.User.FirstName + " " + doctor.User.LastName,
                    Email = doctor.User.Email!,
                    Role = "InActive Doctor"
                },
                pageSize: pageSize,
                pageNumber: pageNumber
                );
        }

        public async Task<PageSourcePagination<UserDto>> GetAllPatients(int pageSize, int pageNumber)
        {
            return await _unitOfWork.Patients.GetAllPaginatedEnhancedAsync(
                selector: patient => new UserDto
                {
                    Name = patient.User.FirstName + " " + patient.User.LastName,
                    Email = patient.User.Email!,
                    Role = "Patient"
                },
                pageSize: pageSize,
                pageNumber: pageNumber
                );
        }

        public async Task<PageSourcePagination<UserDto>> GetAllReceptionist(int pageSize, int pageNumber)
        {
            return await _unitOfWork.Receptionists.GetAllPaginatedEnhancedAsync(
                filter: receptionist => receptionist.User.IsActive,
                selector: receptionist => new UserDto
                {
                    Name = receptionist.User.FirstName + " " + receptionist.User.LastName,
                    Email = receptionist.User.Email!,
                    Role = "Receptionist"
                },
                pageSize: pageSize,
                pageNumber: pageNumber
                );
        }
        public async Task<PageSourcePagination<UserDto>> GetAllInActiveReceptionistRoles(int pageSize, int pageNumber)
        {
            return await _unitOfWork.Receptionists.GetAllPaginatedEnhancedAsync(
                filter: receptionist => !receptionist.User.IsActive,
                selector: receptionist => new UserDto
                {
                    Name = receptionist.User.FirstName + " " + receptionist.User.LastName,
                    Email = receptionist.User.Email!,
                    Role = "InActive Receptionist"
                },
                pageSize: pageSize,
                pageNumber: pageNumber
                );
        }
    }
}

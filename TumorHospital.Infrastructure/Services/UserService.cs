using TumorHospital.Application.DTOs.Response.Pagination;
using TumorHospital.Application.DTOs.Response.User;
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
        public async Task<PageSourcePagination<UserWithIdDto>> GetAllAdmins(int pageNumber)
        {
            return await _unitOfWork.Admins.GetAllPaginatedEnhancedAsync(
                selector: admin => new UserWithIdDto
                {
                    Id = admin.ApplicationUserId,
                    Name = admin.User.FirstName + " " + admin.User.LastName,
                    Email = admin.User.Email!,
                    Role = "Admin"
                },
                pageSize: 20,
                pageNumber: pageNumber
                );
        }

        public async Task<PageSourcePagination<UserWithIdDto>> GetAllDoctors(int pageNumber)
        {
            return await _unitOfWork.Doctors.GetAllPaginatedEnhancedAsync(
                filter: doctor => doctor.User.IsActive,
                selector: doctor => new UserWithIdDto
                {
                    Id = doctor.ApplicationUserId,
                    Name = doctor.User.FirstName + " " + doctor.User.LastName,
                    Email = doctor.User.Email!,
                    Role = "Doctor"
                },
                pageSize: 20,
                pageNumber: pageNumber
                );
        }

        public async Task<PageSourcePagination<UserWithIdDto>> GetAllInActiveDoctorRoles(int pageNumber)
        {
            return await _unitOfWork.Doctors.GetAllPaginatedEnhancedAsync(
                filter: doctor => !doctor.User.IsActive,
                selector: doctor => new UserWithIdDto
                {
                    Id = doctor.ApplicationUserId,
                    Name = doctor.User.FirstName + " " + doctor.User.LastName,
                    Email = doctor.User.Email!,
                    Role = "InActive Doctor"
                },
                pageSize: 20,
                pageNumber: pageNumber
                );
        }

        public async Task<PageSourcePagination<UserWithIdDto>> GetAllPatients(int pageNumber)
        {
            return await _unitOfWork.Patients.GetAllPaginatedEnhancedAsync(
                selector: patient => new UserWithIdDto
                {
                    Id = patient.ApplicationUserId,
                    Name = patient.User.FirstName + " " + patient.User.LastName,
                    Email = patient.User.Email!,
                    Role = "Patient"
                },
                pageSize: 20,
                pageNumber: pageNumber
                );
        }

        public async Task<PageSourcePagination<UserWithIdDto>> GetAllReceptionist(int pageNumber)
        {
            return await _unitOfWork.Receptionists.GetAllPaginatedEnhancedAsync(
                filter: receptionist => receptionist.User.IsActive,
                selector: receptionist => new UserWithIdDto
                {
                    Id = receptionist.ApplicationUserId,
                    Name = receptionist.User.FirstName + " " + receptionist.User.LastName,
                    Email = receptionist.User.Email!,
                    Role = "Receptionist"
                },
                pageSize: 20,
                pageNumber: pageNumber
                );
        }
        public async Task<PageSourcePagination<UserWithIdDto>> GetAllInActiveReceptionistRoles(int pageNumber)
        {
            return await _unitOfWork.Receptionists.GetAllPaginatedEnhancedAsync(
                filter: receptionist => !receptionist.User.IsActive,
                selector: receptionist => new UserWithIdDto
                {
                    Id = receptionist.ApplicationUserId,
                    Name = receptionist.User.FirstName + " " + receptionist.User.LastName,
                    Email = receptionist.User.Email!,
                    Role = "InActive Receptionist"
                },
                pageSize: 20,
                pageNumber: pageNumber
                );
        }
    }
}

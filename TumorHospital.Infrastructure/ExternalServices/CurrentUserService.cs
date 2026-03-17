using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TumorHospital.Application.Intefaces.ExternalServices;

namespace TumorHospital.Infrastructure.ExternalServices
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public CurrentUserService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string? UserId =>
           _contextAccessor.HttpContext?
           .User?
           .FindFirst(ClaimTypes.NameIdentifier)?
           .Value;

        public string? UserRole =>
           _contextAccessor.HttpContext?
           .User?
           .FindFirst(ClaimTypes.Role)?
           .Value;

        public string? Username =>
           _contextAccessor.HttpContext?
           .User?
           .FindFirst(ClaimTypes.Name)?
           .Value;

        public string? UserEmail =>
           _contextAccessor.HttpContext?
           .User?
           .FindFirst(ClaimTypes.Email)?
           .Value;
    }
}

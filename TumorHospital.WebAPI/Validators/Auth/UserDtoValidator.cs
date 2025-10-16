using FluentValidation;
using TumorHospital.WebAPI.DTOs.AuthDto;

namespace TumorHospital.WebAPI.Validators.Auth
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            
        }
    }
}

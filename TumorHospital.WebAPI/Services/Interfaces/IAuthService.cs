using TumorHospital.WebAPI.DTOs.AuthDto;

namespace TumorHospital.WebAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task Register(RegisterDto model);
        Task<AuthModel> ConfirmEmail(ConfirmEmailDto model);
        Task<AuthModel> Login(LoginDto model);
        Task Logout(string userId);
        Task ChangePassword(ChangePasswordDto model);
        Task ForgotPassword(string email);
        Task ResetPassword(ResetPasswordDto model);
        Task<AuthModel> RefreshToken(string refreshToken);
    }
}

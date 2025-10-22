using TumorHospital.Application.DTOs.Request.Auth;
using TumorHospital.Application.DTOs.Response.Auth;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IAuthService
    {
        Task Register(RegisterDto model);
        Task<AuthModel> ConfirmEmail(ConfirmEmailDto model);
        Task<AuthModel> Login(LoginDto model);
        Task Logout(string userId);
        Task ChangePassword(ChangePasswordDto model);
        Task ForgotPassword(ForgotPasswordDto model);
        Task ResetPassword(ResetPasswordDto model);
        Task<AuthModel> RefreshToken(RefreshTokenRequest request);
    }
}

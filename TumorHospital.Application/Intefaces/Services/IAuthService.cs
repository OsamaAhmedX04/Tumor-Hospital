using TumorHospital.Application.DTOs.Request.Auth;
using TumorHospital.Application.DTOs.Response.Auth;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IAuthService
    {
        Task Register(RegisterDto model);
        Task<AuthModel> ConfirmEmail(ConfirmEmailDto model);
        Task ResendConfirmEmailToken(string email);
        Task<AuthModel> Login(LoginDto model);
        Task Logout(string userId);
        Task ChangePassword(ChangePasswordDto model);
        Task<AuthModel> ChangeInActiveRolePassword(ChangePasswordDto model);
        Task ForgotPassword(EmailDto model);
        Task ResetPassword(ResetPasswordDto model);
        Task ResendResetPasswordToken(string email);
        Task<AuthModel> RefreshToken(RefreshTokenRequest request);
    }
}

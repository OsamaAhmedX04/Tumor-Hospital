using TumorHospital.WebAPI.DTOs.AuthDto;

namespace TumorHospital.WebAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task Register(RegisterDto user);
        Task<AuthModel> ConfirmEmail(string email, string confirmToken);
        Task<AuthModel> Login(LoginDto login);
        Task Logout(string userId);
        Task ChangePassword(ChangePasswordDto change);
        Task ForgotPassword(string email);
        Task ResetPassword(ResetPasswordDto reset);
        Task<AuthModel> RefreshToken(RefreshTokenDto refreshTokenDto);
    }
}

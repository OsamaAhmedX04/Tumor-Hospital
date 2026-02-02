using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;
using TumorHospital.Application.DTOs.Request.Auth;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.WebAPI.Documentation;
using TumorHospital.WebAPI.Extensions;

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;
        private readonly IValidator<ChangePasswordDto> _ChangePasswordValidator;

        public AuthController(
            IAuthService authService, IValidator<RegisterDto> registerModelValidator,
            IValidator<LoginDto> loginValidator, IValidator<ChangePasswordDto> changePasswordValidator)
        {
            _authService = authService;
            _registerValidator = registerModelValidator;
            _loginValidator = loginValidator;
            _ChangePasswordValidator = changePasswordValidator;
        }

        [SwaggerOperation(Summary = AuthDocs.RegisterSummary, Description = AuthDocs.RegisterDescription)]
        [HttpPost("Register")]
        [EnableRateLimiting("strict")]
        public async Task<IActionResult> Register(RegisterDto user)
        {
            var result = _registerValidator.Validate(user);
            if (result.IsValid)
            {
                try
                {
                    await _authService.Register(user);
                    return Ok(new { Message = "User Registered Successfully ,We Sent Email Confirmation" });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Identity", ex.Message);
                }
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return BadRequest(new { Errors = ModelState.ToErrorResponse() });

        }



        [SwaggerOperation(Summary = AuthDocs.ConfirmEmailSummary, Description = AuthDocs.ConfirmEmailDescription)]
        [HttpPost("Confirm-Email")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto model)
        {
            try
            {
                var authModel = await _authService.ConfirmEmail(model);
                return Ok(authModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Identity", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }



        [SwaggerOperation(Summary = AuthDocs.ResetConfirmEmailTokenSummary, Description = AuthDocs.ResetConfirmEmailTokenDescription)]
        [HttpPut("Resend-Confirm-Email-Token")]
        public async Task<IActionResult> ResendConfirmEmailToken(EmailDto model)
        {
            try
            {
                await _authService.ResendConfirmEmailToken(model.Email);
                return Ok(new { Message = "Confirm Email Token Resent To Your Email" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Identity", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }


        [SwaggerOperation(Summary = AuthDocs.LoginSummary, Description = AuthDocs.LoginDescription)]
        [HttpPost("Login")]
        [EnableRateLimiting("strict")]
        public async Task<IActionResult> Login(LoginDto user)
        {
            var result = _loginValidator.Validate(user);
            if (result.IsValid)
            {
                try
                {
                    var authModel = await _authService.Login(user);
                    return Ok(authModel);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Identity", ex.Message);
                }
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }



        [SwaggerOperation(Summary = AuthDocs.LogoutSummary, Description = AuthDocs.LogoutDescription)]
        //[Authorize(Roles = SystemRole.ActiveRole)]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout(string userId)
        {
            await _authService.Logout(userId);
            return Ok(new { Message = "Loged Out" });
        }



        [SwaggerOperation(Summary = AuthDocs.ChangePasswordSummary, Description = AuthDocs.ChangePasswordDescription)]
        //[Authorize(Roles = SystemRole.Patient)]
        [HttpPut("Change-Password")]
        [EnableRateLimiting("strict")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            var result = _ChangePasswordValidator.Validate(model);
            if (result.IsValid)
            {
                try
                {
                    await _authService.ChangePassword(model);
                    return Ok(new { Message = "Password Changed Successfully" });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Identity", ex.Message);
                }
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }



        [SwaggerOperation(Summary = AuthDocs.ChangeInActiveRolePasswordSummary, Description = AuthDocs.ChangeInActiveRolePasswordDescription)]
        //[Authorize(Roles = SystemRole.InActiveRole)]
        [HttpPut("Change-InActiveRole-Password")]
        public async Task<IActionResult> ChangeInActiveRolePassword(ChangePasswordDto model)
        {
            var result = _ChangePasswordValidator.Validate(model);
            if (result.IsValid)
            {
                try
                {
                    var authModel = await _authService.ChangeInActiveRolePassword(model);
                    return Ok(authModel);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Identity", ex.Message);
                }
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }


        [SwaggerOperation(Summary = AuthDocs.ForgotPasswordSummary, Description = AuthDocs.ForgotPasswordDescription)]
        [HttpPost("Forgot-Password")]
        [EnableRateLimiting("strict")]
        public async Task<IActionResult> ForgotPassword(EmailDto model)
        {
            try
            {
                await _authService.ForgotPassword(model);
                return Ok(new { Message = "Reset Password Confirmation Token Sent To Your Email" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Identity", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }


        [SwaggerOperation(Summary = AuthDocs.ResetPasswordSummary, Description = AuthDocs.ResetPasswordDescription)]
        [HttpPost("Reset-Password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            try
            {
                await _authService.ResetPassword(model);
                return Ok(new { Message = "Password Reseted Successfully" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Identity", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }



        [SwaggerOperation(Summary = AuthDocs.ResendResetPasswordTokenSummary, Description = AuthDocs.ResendResetPasswordTokenDescription)]
        [HttpPut("Resend-Reset-Password-Token")]
        [EnableRateLimiting("strict")]
        public async Task<IActionResult> ResetResetPasswordToken(EmailDto model)
        {
            try
            {
                await _authService.ResendResetPasswordToken(model.Email);
                return Ok(new { Message = "Reset Password Token Resent To Your Eamil" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Identity", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }


        [SwaggerOperation(Summary = AuthDocs.RefreshTokenSummary, Description = AuthDocs.RefreshTokenDescription)]
        //[Authorize(Roles = SystemRole.InActiveRole + "," + SystemRole.ActiveRole)]
        [HttpPost("Refresh-Token")]
        [EnableRateLimiting("strict")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            try
            {
                var authModel = await _authService.RefreshToken(request);
                return Ok(authModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Identity", ex.Message);
            }
            return BadRequest(new { Errors = ModelState.ToErrorResponse() });
        }



    }
}

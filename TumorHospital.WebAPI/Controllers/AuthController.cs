using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TumorHospital.WebAPI.Data;
using TumorHospital.WebAPI.DTOs.AuthDto;
using TumorHospital.WebAPI.Extensions;
using TumorHospital.WebAPI.Services.Interfaces;

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

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto user)
        {
            var result = _registerValidator.Validate(user);
            if(result.IsValid)
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

        [HttpPost("Login")]
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

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout(string userId)
        {
            await _authService.Logout(userId);
            return Ok(new { Message = "Loged Out" });
        }

        [HttpPost("Change-Password")]
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

        [HttpPost("Forgot-Password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto model)
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

        [HttpPost("Refresh-Token")]
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

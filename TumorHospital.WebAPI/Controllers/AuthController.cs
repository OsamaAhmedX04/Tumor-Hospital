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
using TumorHospital.WebAPI.Data.Models;
using TumorHospital.WebAPI.DTOs.AuthDto;
using TumorHospital.WebAPI.ExternalServices.Implementation;
using TumorHospital.WebAPI.ExternalServices.Interfaces;

namespace TumorHospital.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly JWTService _jwtService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext context,
            IEmailService emailService,
            JWTService jwtService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _emailService = emailService;
            _jwtService = jwtService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto user)
        {
            var newUser = new ApplicationUser
            {
                FirstName = user.FirstName,
                LasttName = user.LastName,
                UserName = user.Email,
                Email = user.Email,
            };

            var result = await _userManager.CreateAsync(newUser, user.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if (!await _roleManager.RoleExistsAsync(user.Role))
                await _roleManager.CreateAsync(new IdentityRole(user.Role));
            await _userManager.AddToRoleAsync(newUser, user.Role);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token)); // مهم جدًا عشان الرموز
            var body = $@"
            <a href=""https://localhost:7088/api/Auth/Confirm-Email?email={newUser.Email}&token={encodedToken}""
               style=""display:inline-block;
                      padding:10px 20px;
                      background-color:#28a745;
                      color:white;
                      text-decoration:none;
                      border-radius:5px;
                      font-size:16px;"">
                Confirm Email
            </a>";

            await _emailService.SendEmailAsync(
                newUser.Email,
                "Email Confirmation",
                body);

            return Ok("Registerd Succefully Please Confirm Your Email ,We Sent token to your email");
        }

        [HttpGet("Confirm-Email")]
        public async Task<IActionResult> ConfirmEmail(string email, string confirmToken)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("User Not Found");

            if (await _userManager.IsEmailConfirmedAsync(user))
                return BadRequest("Email Is Already Confirmed Before");


            var decodedTokenBytes = WebEncoders.Base64UrlDecode(confirmToken);
            var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!result.Succeeded)
                return BadRequest("Invalid Token");

            var userRoles = await _userManager.GetRolesAsync(user);
            var jwtToken = _jwtService.GenerateToken(new UserDto
            {
                Email = user.Email!,
                Name = user.FirstName + " " + user.LasttName,
                Role = userRoles[0]
            });
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var refreshToken = Guid.NewGuid().ToString();

            var tokenRow = await _context.RefreshTokenAuths.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (tokenRow == null)
            {
                await _context.RefreshTokenAuths.AddAsync(new RefreshTokenAuth
                {
                    UserId = user.Id,
                    Token = token,
                    RefreshToken = refreshToken,
                });
            }
            else
            {
                tokenRow.Token = token;
                tokenRow.RefreshToken = refreshToken;
                tokenRow.RefreshTokenExpiration = DateTime.Now.AddDays(20);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Your Email Confirmed Succefully",
                UserId = user.Id,
                Token = token,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
                return BadRequest("User Not Found");

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return BadRequest("Email Not Confirmed Yet");

            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
                return Unauthorized("Email Or Password Wrong");

            var userRoles = await _userManager.GetRolesAsync(user);

            var jwtToken = _jwtService.GenerateToken(new UserDto
            {
                Email = user.Email!,
                Name = user.FirstName + " " + user.LasttName,
                Role = userRoles[0]
            });
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var refreshToken = Guid.NewGuid().ToString();

            var tokenRow = await _context.RefreshTokenAuths.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (tokenRow == null)
            {
                await _context.RefreshTokenAuths.AddAsync(new RefreshTokenAuth
                {
                    UserId = user.Id,
                    Token = token,
                    RefreshToken = refreshToken,
                });
            }
            else
            {
                tokenRow.Token = token;
                tokenRow.RefreshToken = refreshToken;
                tokenRow.RefreshTokenExpiration = DateTime.Now.AddDays(20);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                UserId = user.Id,
                Token = token,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout(string userId)
        {
            var token = await _context.RefreshTokenAuths.FirstOrDefaultAsync(x => x.UserId == userId);
            if (token != null)
            {
                _context.RefreshTokenAuths.Remove(token);
                await _context.SaveChangesAsync();
            }
            return Ok("Loged Out");
        }

        [HttpPost("Change-Password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto change)
        {
            var user = await _userManager.FindByEmailAsync(change.Email);

            if (user == null) return BadRequest("User Not Exist");

            var result = await _userManager.ChangePasswordAsync(user, change.OldPassword, change.NewPassword);
            if (!result.Succeeded) return BadRequest("Please Enter Right Password");

            return Ok("Password Changed Succefully");
        }

        [HttpPost("Forgot-Password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return BadRequest("User Not Exist");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _emailService.SendEmailAsync(
                email,
                "Reset Password",
                $"Reset Password Confirmation Token : {token}"
                );

            return Ok("We Sent you confirmation token");
        }

        [HttpPost("Reset-Password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto reset)
        {
            var user = await _userManager.FindByEmailAsync(reset.Email);
            if (user == null) return BadRequest("User Not Exist");

            var result = await _userManager.ResetPasswordAsync(user, reset.Token, reset.NewPassword);
            if (!result.Succeeded) return BadRequest("Invalid Token");

            return Ok("Password Reset Succefully");
        }

        [HttpPost("Refresh-Token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var tokenRow = await _context.RefreshTokenAuths
                .FirstOrDefaultAsync(x => x.RefreshToken == refreshTokenDto.RefreshToken);

            if (tokenRow == null)
                return BadRequest("Invalid Refresh Token");

            if (tokenRow.RefreshTokenExpiration <= DateTime.UtcNow)
                return BadRequest("Refresh Token Expired");

            var user = await _userManager.FindByIdAsync(tokenRow.UserId);
            if (user == null)
                return BadRequest("User Not Exist");

            var userRoles = await _userManager.GetRolesAsync(user);

            var jwtToken = _jwtService.GenerateToken(new UserDto
            {
                Email = user.Email!,
                Name = user.FirstName + " " + user.LasttName,
                Role = userRoles.FirstOrDefault()
            });

            var newAccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            var newRefreshToken = Guid.NewGuid().ToString();

            tokenRow.RefreshToken = newRefreshToken;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }



    }
}

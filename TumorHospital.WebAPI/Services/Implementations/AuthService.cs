using Microsoft.AspNetCore.Authorization;
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
using TumorHospital.WebAPI.Services.Interfaces;
using TumorHospital.WebAPI.UOW;

namespace TumorHospital.WebAPI.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly JWTService _jwtService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            JWTService jwtService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _jwtService = jwtService;
        }

        public async Task Register(RegisterDto user)
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
                throw new Exception(result.Errors.FirstOrDefault()?.Description ?? "Unknown error");

            if (!await _roleManager.RoleExistsAsync(user.Role))
                await _roleManager.CreateAsync(new IdentityRole(user.Role));
            await _userManager.AddToRoleAsync(newUser, user.Role);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token)); // مهم جدًا عشان الرموز
            var body = $@"
            <a href=""https://localhost:7114/api/Auth/Confirm-Email?email={newUser.Email}&confirmToken={encodedToken}""
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
                encodedToken);
        }

        public async Task<AuthModel> ConfirmEmail(string email, string confirmToken)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new Exception("User Not Found");

            if (await _userManager.IsEmailConfirmedAsync(user))
                throw new Exception("Email Is Already Confirmed Before");


            var decodedTokenBytes = WebEncoders.Base64UrlDecode(confirmToken);
            var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!result.Succeeded)
                throw new Exception("Invalid Token");

            var userRoles = await _userManager.GetRolesAsync(user);
            var jwtToken = _jwtService.GenerateToken(new UserDto
            {
                Email = user.Email!,
                Name = user.FirstName + " " + user.LasttName,
                Role = userRoles[0]
            });

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var refreshToken = Guid.NewGuid().ToString();

            var tokenRow = await _unitOfWork.RefreshTokenAuths.GetAllAsIQueryable().FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (tokenRow == null)
            {
                await _unitOfWork.RefreshTokenAuths.AddAsync(new RefreshTokenAuth
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

            await _unitOfWork.CompleteAsync();

            return new AuthModel
            {
                Message = "Your Email Confirmed Succefully",
                UserId = user.Id,
                Token = token,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthModel> Login(LoginDto login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
                throw new Exception("User Not Found");

            if (!await _userManager.IsEmailConfirmedAsync(user))
                throw new Exception("Email Not Confirmed Yet");

            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
                throw new Exception("Email Or Password Wrong");

            var userRoles = await _userManager.GetRolesAsync(user);

            var jwtToken = _jwtService.GenerateToken(new UserDto
            {
                Email = user.Email!,
                Name = user.FirstName + " " + user.LasttName,
                Role = userRoles[0]
            });
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var refreshToken = Guid.NewGuid().ToString();

            var tokenRow = await _unitOfWork.RefreshTokenAuths.GetAllAsIQueryable().FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (tokenRow == null)
            {
                await _unitOfWork.RefreshTokenAuths.AddAsync(new RefreshTokenAuth
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

            await _unitOfWork.CompleteAsync();

            return new AuthModel
            {
                Message = "Login Succefully",
                UserId = user.Id,
                Token = token,
                RefreshToken = refreshToken
            };
        }

        public async Task Logout(string userId)
        {
            var token = await _unitOfWork.RefreshTokenAuths.GetAllAsIQueryable().FirstOrDefaultAsync(x => x.UserId == userId);
            if (token != null)
            {
                _unitOfWork.RefreshTokenAuths.Delete(token.UserId);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task ChangePassword(ChangePasswordDto change)
        {
            var user = await _userManager.FindByEmailAsync(change.Email);

            if (user == null) throw new Exception("User Not Exist");

            var result = await _userManager.ChangePasswordAsync(user, change.OldPassword, change.NewPassword);
            if (!result.Succeeded) throw new Exception("Please Enter Right Password");
        }

        public async Task ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new Exception("User Not Exist");

            if(!await _userManager.IsEmailConfirmedAsync(user)) throw new Exception("Email Not Confirmed Yet");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _emailService.SendEmailAsync(
                email,
                "Reset Password",
                $"Reset Password Confirmation Token : {token}"
                );

            //return Ok("We Sent you confirmation token");
        }

        public async Task ResetPassword(ResetPasswordDto reset)
        {
            var user = await _userManager.FindByEmailAsync(reset.Email);
            if (user == null) throw new Exception("User Not Exist");

            var result = await _userManager.ResetPasswordAsync(user, reset.Token, reset.NewPassword);
            if (!result.Succeeded) throw new Exception("Invalid Token");

            //return Ok("Password Reset Succefully");
        }

        public async Task<AuthModel> RefreshToken(string refreshToken)
        {
            var tokenRow = await _unitOfWork.RefreshTokenAuths
                .GetAllAsIQueryable()
                .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

            if (tokenRow == null)
                throw new Exception("Invalid Refresh Token");

            if (tokenRow.RefreshTokenExpiration <= DateTime.UtcNow)
                throw new Exception("Refresh Token Expired");

            var user = await _userManager.FindByIdAsync(tokenRow.UserId);
            if (user == null)
                throw new Exception("User Not Exist");

            var userRoles = await _userManager.GetRolesAsync(user);

            var jwtToken = _jwtService.GenerateToken(new UserDto
            {
                Email = user.Email!,
                Name = user.FirstName + " " + user.LasttName,
                Role = userRoles.FirstOrDefault()!
            });

            var newAccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            var newRefreshToken = Guid.NewGuid().ToString();

            tokenRow.RefreshToken = newRefreshToken;
            await _unitOfWork.CompleteAsync();

            return new AuthModel
            {
                Message = "Token Refreshed Successfully",
                UserId = user.Id,
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }




    }
}

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

        public async Task Register(RegisterDto model)
        {
            var newUser = new ApplicationUser
            {
                FirstName = model.FirstName,
                LasttName = model.LastName,
                UserName = model.Email,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
                throw new Exception(result.Errors.FirstOrDefault()?.Description ?? "Unknown error");

            if (!await _roleManager.RoleExistsAsync(model.Role))
                await _roleManager.CreateAsync(new IdentityRole(model.Role));
            await _userManager.AddToRoleAsync(newUser, model.Role);

            var token = new Random().Next(100000,999999).ToString();
            var confirmTokenResult = await _userManager.SetAuthenticationTokenAsync(
                newUser, "Default", "EmailConfirmation", token
                );
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 500px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; background-color: #f9f9f9;'>
                    <h2 style='text-align: center; color: #333;'>Confirm Your Email</h2>

                    <p style='font-size: 15px; color: #555;'>
                        Thanks for registering! Please use the code below to confirm your email:
                    </p>

                    <div style='text-align: center; margin: 30px 0;'>
                        <span style='display: inline-block; background-color: #007bff; color: white; padding: 14px 28px; font-size: 22px; letter-spacing: 3px; border-radius: 6px;'>
                            {token}
                        </span>
                    </div>

                    <p style='font-size: 14px; color: #666;'>
                        If you didn’t create this account, you can ignore this email.
                    </p>

                    <p style='margin-top: 30px; font-size: 14px; color: #333;'>
                        Best regards,<br/>
                        <strong>Your App Team</strong>
                    </p>
                </div>
                ";

            await _emailService.SendEmailAsync(
                newUser.Email,
                "Email Confirmation",
                body);
        }

        public async Task<AuthModel> ConfirmEmail(ConfirmEmailDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                throw new Exception("User Not Found");

            if (await _userManager.IsEmailConfirmedAsync(user))
                throw new Exception("Email Is Already Confirmed Before");


            var savedToken = await _userManager.GetAuthenticationTokenAsync(
                user, "Default", "EmailConfirmation"
                );
            if (savedToken != model.Token) throw new Exception("Invalid Token");
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            await _userManager.RemoveAuthenticationTokenAsync(user, "Default", "EmailConfirmation");


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

        public async Task<AuthModel> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                throw new Exception("User Not Found");

            if (!await _userManager.IsEmailConfirmedAsync(user))
                throw new Exception("Email Not Confirmed Yet");

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);
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

        public async Task ChangePassword(ChangePasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null) throw new Exception("User Not Exist");

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!result.Succeeded) throw new Exception("Please Enter Right Password");
        }

        public async Task ForgotPassword(ForgotPasswordDto model)
        {
            if (string.IsNullOrEmpty(model.Email)) throw new Exception("Please Send Refresh Token");

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) throw new Exception("User Not Exist");

            if(!await _userManager.IsEmailConfirmedAsync(user)) throw new Exception("Email Not Confirmed Yet");

            var token = new Random().Next(100000, 999999).ToString();
            var confirmTokenResult = await _userManager.SetAuthenticationTokenAsync(
                user, "Default", "ResetPasswordConfirmation", token
                );
            var body = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 500px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; background-color: #f9f9f9;'>
                        <h2 style='text-align: center; color: #333;'>Reset Your Password</h2>
                        <p style='font-size: 15px; color: #555;'>
                            We received a request to reset your password. Use the code below to continue:
                        </p>
                        <div style='text-align: center; margin: 30px 0;'>
                            <span style='display: inline-block; background-color: #4CAF50; color: white; padding: 14px 28px; font-size: 22px; letter-spacing: 3px; border-radius: 6px;'>
                                {token}
                            </span>
                        </div>
                        <p style='font-size: 14px; color: #666;'>
                            If you didn’t request this, you can safely ignore this email.
                        </p>
                        <p style='margin-top: 30px; font-size: 14px; color: #333;'>
                            Best regards,<br/>
                            <strong>Your App Team</strong>
                        </p>
                    </div>
                    ";
            await _emailService.SendEmailAsync(
                model.Email,
                "Reset Password",
                body
                );

        }

        public async Task ResetPassword(ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) throw new Exception("User Not Exist");

            var savedToken = await _userManager.GetAuthenticationTokenAsync(
                user, "Default", "ResetPasswordConfirmation"
                );
            if (savedToken != model.Token) throw new Exception("Invalid Token");

            await _userManager.RemoveAuthenticationTokenAsync(user, "Default", "ResetPasswordConfirmation");

            var removeResult = await _userManager.RemovePasswordAsync(user);
            if(!removeResult.Succeeded) throw new Exception($"Failed to remove Password");

            var addResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
            if (!addResult.Succeeded) throw new Exception($"Failed to add new Password");
        }

        public async Task<AuthModel> RefreshToken(RefreshTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.RefreshToken)) throw new Exception("Please Send Refresh Token");
            var tokenRow = await _unitOfWork.RefreshTokenAuths
                .GetAllAsIQueryable()
                .FirstOrDefaultAsync(x => x.RefreshToken == request.RefreshToken);

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

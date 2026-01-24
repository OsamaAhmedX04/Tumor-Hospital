using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using TumorHospital.Application.DTOs.Request.Auth;
using TumorHospital.Application.DTOs.Response.Auth;
using TumorHospital.Application.Intefaces.ExternalServices;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Constants;
using TumorHospital.Domain.Entities;
using TumorHospital.Domain.Enums;
using TumorHospital.Infrastructure.ExternalServices;
using TumorHospital.Infrastructure.Persistence.Context;

namespace TumorHospital.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly JWTService _jwtService;
        private readonly AppDbContext _db;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            JWTService jwtService,
            AppDbContext db
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _jwtService = jwtService;
            _db = db;
        }

        public async Task Register(RegisterDto model)
        {
            var newUser = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
                throw new Exception(result.Errors.FirstOrDefault()?.Description ?? "Unknown error");

            await _userManager.AddToRoleAsync(newUser, Role.Patient.ToString());
            await _db.Patients.AddAsync(new Patient
            {
                ApplicationUserId = newUser.Id,
                Gender = model.Gender,
                RegistrationDate = DateTime.Now
            });

            var token = new Random().Next(100000, 999999).ToString();
            var confirmTokenResult = await _userManager.SetAuthenticationTokenAsync(
                newUser, "Default", "EmailConfirmation", token
                );

            var setterToken = await _db
                .UserTokens
                .FirstOrDefaultAsync(u => u.UserId == newUser.Id);
            setterToken.ExpireDate = DateTime.Now.AddHours(1);
            await _db.SaveChangesAsync();

            await _emailService.SendEmailAsync(
                newUser.Email,
                "Email Confirmation",
                EmailBody.GetPatientEmailCreatedBody(token));
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

            var tokenToCheck = await _db.UserTokens
                .FirstOrDefaultAsync(u => u.Value == savedToken);
            var isTokenExpired = tokenToCheck.ExpireDate < DateTime.Now;

            if (savedToken != model.Token || isTokenExpired) throw new Exception("Invalid Or Expired Token");
            user.EmailConfirmed = true;
            user.IsActive = true;
            await _userManager.UpdateAsync(user);

            await _userManager.RemoveAuthenticationTokenAsync(user, "Default", "EmailConfirmation");


            var userRoles = await _userManager.GetRolesAsync(user);
            var jwtToken = _jwtService.GenerateToken(new UserDto
            {
                Email = user.Email!,
                Name = user.FirstName + " " + user.LastName,
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

        public async Task ResendConfirmEmailToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new Exception("User Not Exist");
            if (await _userManager.IsEmailConfirmedAsync(user))
                throw new Exception("This Email Is Aleardy Confirmed Before");

            var token = new Random().Next(100000, 999999).ToString();
            var confirmTokenResult = await _userManager.SetAuthenticationTokenAsync(
                user, "Default", "EmailConfirmation", token
                );

            var setterToken = await _db
                .UserTokens.FirstOrDefaultAsync(
                u => u.Value == token && u.UserId == user.Id);
            setterToken.ExpireDate = DateTime.Now.AddHours(1);
            await _db.SaveChangesAsync();

            await _emailService.SendEmailAsync(
                user.Email,
                "Email Confirmation",
                EmailBody.GetResendConfirmEmailBody(token));
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
                Name = user.FirstName + " " + user.LastName,
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
            user.IsActive = true;
            await _userManager.UpdateAsync(user);

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

        public async Task<AuthModel> ChangeInActiveRolePassword(ChangePasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) throw new Exception("User Not Exist");

            var isInActiveDoctor = await _userManager.IsInRoleAsync(user, Role.InActiveDoctorRole.ToString());
            var isInActiveReceptionist = await _userManager.IsInRoleAsync(user, Role.InActiveReceptionistRole.ToString());
            if (!isInActiveDoctor || !isInActiveReceptionist) throw new Exception("User Already Active");

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!result.Succeeded) throw new Exception("Please Enter Right Password");

            if (isInActiveDoctor)
            {
                await _userManager.RemoveFromRoleAsync(user, Role.InActiveDoctorRole.ToString());
                await _userManager.AddToRoleAsync(user, Role.Doctor.ToString());
            }
            else
            {
                await _userManager.RemoveFromRoleAsync(user, Role.InActiveReceptionistRole.ToString());
                await _userManager.AddToRoleAsync(user, Role.Receptionist.ToString());
            }


            var jwtToken = _jwtService.GenerateToken(new UserDto
            {
                Email = user.Email!,
                Name = user.FirstName + " " + user.LastName,
                Role = isInActiveDoctor ? Role.Doctor.ToString() : Role.Receptionist.ToString()
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

            user.IsActive = true;
            await _userManager.UpdateAsync(user);

            return new AuthModel
            {
                Message = "Password Changed Succefully",
                UserId = user.Id,
                Token = token,
                RefreshToken = refreshToken
            };
        }

        public async Task ForgotPassword(EmailDto model)
        {
            if (string.IsNullOrEmpty(model.Email)) throw new Exception("Please Send Refresh Token");

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) throw new Exception("User Not Exist");

            if (!await _userManager.IsEmailConfirmedAsync(user)) throw new Exception("Email Not Confirmed Yet");

            var token = new Random().Next(100000, 999999).ToString();
            var confirmTokenResult = await _userManager.SetAuthenticationTokenAsync(
                user, "Default", "ResetPasswordConfirmation", token
                );

            var setterToken = await _db
                .UserTokens
                .FirstOrDefaultAsync(u => u.UserId == user.Id);
            setterToken.ExpireDate = DateTime.Now.AddHours(1);
            await _db.SaveChangesAsync();

            await _emailService.SendEmailAsync(
                model.Email,
                "Reset Password",
                EmailBody.GetForgetPasswordEmailBody(token));
        }

        public async Task ResetPassword(ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) throw new Exception("User Not Exist");

            var savedToken = await _userManager.GetAuthenticationTokenAsync(
                user, "Default", "ResetPasswordConfirmation"
                );

            var tokenToCheck = await _db.UserTokens
                .FirstOrDefaultAsync(u => u.Value == savedToken);
            var isTokenExpired = tokenToCheck.ExpireDate < DateTime.Now;

            if (savedToken != model.Token || isTokenExpired) throw new Exception("Invalid Or Expired Token");

            var removeResult = await _userManager.RemovePasswordAsync(user);
            if (!removeResult.Succeeded) throw new Exception($"Failed to remove Password");

            var addResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
            if (!addResult.Succeeded) throw new Exception(addResult.Errors.FirstOrDefault()?.Description ?? "Unknown error");

            await _userManager.RemoveAuthenticationTokenAsync(user, "Default", "ResetPasswordConfirmation");
        }


        public async Task ResendResetPasswordToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new Exception("User Not Exist");
            var token = new Random().Next(100000, 999999).ToString();
            var confirmTokenResult = await _userManager.SetAuthenticationTokenAsync(
                user, "Default", "ResetPasswordConfirmation", token
                );

            var setterToken = await _db
                .UserTokens.FirstOrDefaultAsync(
                u => u.Value == token && u.UserId == user.Id);

            setterToken.ExpireDate = DateTime.Now.AddHours(1);
            await _db.SaveChangesAsync();



            await _emailService.SendEmailAsync(
                user.Email,
                "Reset Password",
                EmailBody.GetResendResetPasswordEmailBody(token)
                );
        }

        public async Task<AuthModel> RefreshToken(RefreshTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.RefreshToken)) throw new Exception("Please Send Refresh Token");
            var tokenRow = await _unitOfWork.RefreshTokenAuths
                .GetAllAsIQueryable()
                .FirstOrDefaultAsync(x => x.RefreshToken == request.RefreshToken);

            if (tokenRow == null)
                throw new Exception("Invalid Refresh Token");

            if (tokenRow.RefreshTokenExpiration <= DateTime.Now)
                throw new Exception("Refresh Token Expired");

            var user = await _userManager.FindByIdAsync(tokenRow.UserId);
            if (user == null)
                throw new Exception("User Not Exist");

            var userRoles = await _userManager.GetRolesAsync(user);

            var jwtToken = _jwtService.GenerateToken(new UserDto
            {
                Email = user.Email!,
                Name = user.FirstName + " " + user.LastName,
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

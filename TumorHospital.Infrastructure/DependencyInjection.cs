using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Supabase;
using System.Text;
using TumorHospital.Application.Intefaces.ExternalServices;
using TumorHospital.Application.Intefaces.Repositories;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Entities;
using TumorHospital.Infrastructure.ExternalServices;
using TumorHospital.Infrastructure.Persistence.Context;
using TumorHospital.Infrastructure.Persistence.Repositories;
using TumorHospital.Infrastructure.Services;
using TumorHospital.Infrastructure.Settings;
using TumorHospital.Infrastructure.UOW;

namespace TumorHospital.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region DBContext And Identity
            // Register DbContext
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Register Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders(); // For email confirm, reset password
            #endregion

            #region JWT
            // Register JWT
            services.AddScoped<JWTService>();
            var jwtSettings = configuration.GetSection("JWT");
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey)
                };
            });
            #endregion

            #region UnitOfWork And Repository Pattern
            // Register UOW
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register Repository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            #endregion

            #region EmailService
            // Register EmailService
            services.Configure<SMTPSettings>(configuration.GetSection("SmtpSettings"));
            services.AddScoped<IEmailService, EmailService>();
            #endregion

            #region FileService
            // Bind Supabase settings from appsettings.json
            services.Configure<SupabaseSettings>(configuration.GetSection("Supabase"));

            // Register Supabase.Client as singleton
            services.AddSingleton<Client>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<SupabaseSettings>>().Value;
                return new Client(settings.Url, settings.ServiceKey, new SupabaseOptions
                {
                    AutoRefreshToken = true,
                    AutoConnectRealtime = true
                });
            });

            services.AddScoped<IFileService, FileService>();
            #endregion

            #region BuisenessService
            // Register Seevices
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAdminSevice, AdminService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IReceptionService, ReceptionService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IDonationService, DonationService>();
            services.AddScoped<IUserService, UserService>();
            #endregion

            return services;
        }
    }
}

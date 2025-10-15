using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TumorHospital.WebAPI.Data;
using TumorHospital.WebAPI.Data.Models;
using TumorHospital.WebAPI.Extensions;
using TumorHospital.WebAPI.ExternalServices.Implementation;
using TumorHospital.WebAPI.ExternalServices.Interfaces;
using TumorHospital.WebAPI.Repositories.Implementations;
using TumorHospital.WebAPI.Repositories.Interfaces;
using TumorHospital.WebAPI.Services.Implementations;
using TumorHospital.WebAPI.Services.Interfaces;
using TumorHospital.WebAPI.Settings;
using TumorHospital.WebAPI.UOW;
using TumorHospital.WebAPI.Validators;

namespace TumorHospital.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGenJwtAuth();

            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
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


            builder.Services.Configure<SMTPSettings>(builder.Configuration.GetSection("SmtpSettings"));
            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.AddScoped<JWTService>();
            var jwtSettings = builder.Configuration.GetSection("JWT");
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

            builder.Services.AddAuthentication(options =>
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

            // Validators Service
            builder.Services.AddFluentValidationClientsideAdapters()
                            .AddValidatorsFromAssemblyContaining<UserDtoValidator>();

            // Repositories and UnitOfWork
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add Services
            builder.Services.AddScoped<IAuthService, AuthService>();

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(Program));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

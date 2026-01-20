using Hangfire;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using TumorHospital.Application;
using TumorHospital.Infrastructure;
using TumorHospital.WebAPI.Extensions;


namespace TumorHospital.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Add Endpoint API Explorer
            builder.Services.AddEndpointsApiExplorer();

            // Register Swagger with JWT Auth
            builder.Services.AddSwaggerGenJwtAuth();

            // Register Memory Cache
            builder.Services.AddMemoryCache();

            // Register Application Layer
            builder.Services.AddApplication();

            // Register Infrastructure Layer
            builder.Services.AddInfrastructure(builder.Configuration);

            // Build The App
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseAuthorization();

            app.UseHangfireDashboard("/hangfire");

            app.MapControllers();

            app.Run();
        }
    }
}

using Hangfire;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.RateLimiting;
using Serilog;
using Serilog.Events;
using System.Threading.RateLimiting;
using TumorHospital.Application;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Infrastructure;
using TumorHospital.Infrastructure.Services;
using TumorHospital.WebAPI.Extensions;


namespace TumorHospital.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

                // Required when behind Docker / reverse proxy / cloud
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            builder.Services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                // Custom rejection response
                options.OnRejected = async (context, token) =>
                {
                    var logger = context.HttpContext.RequestServices
                        .GetRequiredService<ILogger<Program>>();

                    var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString();
                    var endpoint = context.HttpContext.Request.Path;

                    logger.LogWarning(
                        "Rate limit exceeded. IP: {IP}, Endpoint: {Endpoint}",
                        ip,
                        endpoint
                    );

                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    await context.HttpContext.Response.WriteAsync(
                        "Too many requests. Please try again later.",
                        token
                    );
                };

                // Global IP-based limiter
                options.GlobalLimiter =
                    PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    {
                        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                        return RateLimitPartition.GetFixedWindowLimiter(
                            ip,
                            _ => new FixedWindowRateLimiterOptions
                            {
                                PermitLimit = 25,
                                Window = TimeSpan.FromMinutes(1),
                                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                                QueueLimit = 0
                            });
                    });

                // Named policies
                options.AddFixedWindowLimiter("strict", opt =>
                {
                    opt.PermitLimit = 10;
                    opt.Window = TimeSpan.FromMinutes(1);
                    opt.QueueLimit = 0;
                });
            });

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

            // Logging Builder
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                        .WriteTo.File(
                            path: "Logs/log-.txt",
                            rollingInterval: RollingInterval.Day,
                            retainedFileCountLimit: 14,
                            outputTemplate:
                            "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
                        )
                )
                .WriteTo.File(
                    path: "Logs/errors-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    restrictedToMinimumLevel: LogEventLevel.Warning,
                    outputTemplate:
                    "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
                )
                .CreateLogger();

            builder.Host.UseSerilog(Log.Logger);

            // Build The App
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}

            app.UseForwardedHeaders();

            app.UseHttpsRedirection();

            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseRateLimiter();

            app.MapHub<VideoCallHub>("/hubs/video-call");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapHangfireDashboard("/hangfire")
                .DisableRateLimiting();

            app.MapControllers();

            app.AddBackgroundJobs();

            app.Run();
        }
    }
}

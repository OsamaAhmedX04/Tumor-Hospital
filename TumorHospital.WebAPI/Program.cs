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
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGenJwtAuth();

            // Register Application Layer
            builder.Services.AddApplication();

            // Register Infrastructure Layer
            builder.Services.AddInfrastructure(builder.Configuration);

            //// AutoMapper
            ////builder.Services.AddAutoMapper(typeof(Program));

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

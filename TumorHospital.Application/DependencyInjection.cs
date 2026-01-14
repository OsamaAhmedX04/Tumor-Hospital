using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using TumorHospital.Application.Profiles;
using TumorHospital.Application.Validators.Appointment;
using TumorHospital.Application.Validators.Auth;
using TumorHospital.Application.Validators.User;

namespace TumorHospital.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            #region FluentValidation And AutoMapper
            // Validators Service
            services.AddFluentValidationClientsideAdapters()
                            .AddValidatorsFromAssemblyContaining<LoginDtoValidator>();

            // AutoMapper
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);
            #endregion

            return services;
        }
    }
}

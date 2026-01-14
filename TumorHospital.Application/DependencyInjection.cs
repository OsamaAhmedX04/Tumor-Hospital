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

            services.AddFluentValidationClientsideAdapters()
                            .AddValidatorsFromAssemblyContaining<UpdatePatientProfileDtoValidator>();
            services.AddFluentValidationClientsideAdapters()
                            .AddValidatorsFromAssemblyContaining<UpdateDoctorProfileDtoValidator>();
            services.AddFluentValidationClientsideAdapters()
                            .AddValidatorsFromAssemblyContaining<UpdateReceptionistProfileDtoValidator>();
            services.AddFluentValidationClientsideAdapters()
                            .AddValidatorsFromAssemblyContaining<PrescriptionCreateUpdateDtoValidator>();

            // AutoMapper
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);
            services.AddAutoMapper(typeof(ProfileMapping));
            services.AddAutoMapper(typeof(ReceptionistProfile));
            #endregion

            return services;
        }
    }
}

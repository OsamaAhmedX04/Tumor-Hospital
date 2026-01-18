using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using TumorHospital.Application.Validators.Auth;

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

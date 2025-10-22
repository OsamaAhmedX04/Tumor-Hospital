using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Application.Validators.Auth;

namespace TumorHospital.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Validators Service
            services.AddFluentValidationClientsideAdapters()
                            .AddValidatorsFromAssemblyContaining<UserDtoValidator>();
            // Register DbContext

            return services;
        }
    }
}

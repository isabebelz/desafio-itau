using CompraProgramada.Application.Common.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CompraProgramada.Application.Common
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            try
            {
                var thisAssembly = typeof(DependencyInjection).Assembly;

                Log.Information("Registering AutoMapper...");
                services.AddAutoMapper(thisAssembly);

                Log.Information("Registering Validators...");
                services.AddValidatorsFromAssembly(thisAssembly);
                
                Log.Information("Registering MediatR...");
                services.AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssembly(thisAssembly);
                    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
                });
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Fatal error initializing the Application project.");
                throw;
            }

            return services;
        }
    }
}

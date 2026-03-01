using CompraProgramada.Domain.Interfaces;
using CompraProgramada.Infra.Data.Context;
using CompraProgramada.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CompraProgramada.Infra.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                Log.Information("Registering Infrastructure Services...");

                var connectionString = configuration.GetConnectionString("DefaultConnection");

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

                services.AddRepositories();
                services.AddServices();

            }
            catch(Exception ex)
            {
                Log.Fatal(ex, "Fatal error initializing the Infrastructure project.");
                throw;
            }

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAcaoRepository, AcaoRepository>();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {

            return services;
        }
    }
}

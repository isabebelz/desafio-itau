using CompraProgramada.Application.Events;
using CompraProgramada.Domain.Interfaces;
using CompraProgramada.Domain.Interfaces.Repositories;
using CompraProgramada.Domain.Interfaces.Services;
using CompraProgramada.Infra.Data.Context;
using CompraProgramada.Infra.Data.Messaging;
using CompraProgramada.Infra.Data.Parses;
using CompraProgramada.Infra.Data.Repositories;
using Confluent.Kafka;
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

                AddKafkaProducer(services, configuration);

                services.AddScoped<ICotahistParser, CotahistParser>();

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
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<ICestaRecomendacaoRepository, CestaRecomendacaoRepository>();
            services.AddScoped<IOrdemCompraRepository, OrdemCompraRepository>();
            services.AddScoped<IContaMasterRepository, ContaMasterRepository>();
            services.AddScoped<IParametroSistemaRepository, ParametroSistemaRepository>();
           services.AddScoped<ICotacaoRepository, CotacaoRepository>();

            return services;
        }

        {
                BootstrapServers = bootstrapServers
            };

            services.AddSingleton<IProducer<Null, string>>(new ProducerBuilder<Null, string>(producerConfig).Build());

        }
    }
}

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
                    options.UseMySql(
                        connectionString,
                        ServerVersion.Parse("8.0.36-mysql"),
                        mySqlOptions => mySqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 10,                            
                            maxRetryDelay: TimeSpan.FromSeconds(5),       
                            errorNumbersToAdd: null                       
                        )
                    )
                );

                AddKafkaProducer(services, configuration);

                services.AddScoped<ICotahistParser, CotahistParser>();

                services.AddRepositories();

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

        private static void AddKafkaProducer(IServiceCollection services, IConfiguration configuration)
        {
            var kafkaSection = configuration.GetSection("Kafka");
            var bootstrapServers = kafkaSection.GetValue<string>("BootstrapServers");

            var topicDistribuicao = kafkaSection.GetValue<string>("TopicDistribuicaoIrDedoDuro");
            if (string.IsNullOrWhiteSpace(topicDistribuicao))
                throw new InvalidOperationException("O tópico Kafka 'TopicDistribuicaoIrDedoDuro' não foi configurado. Verifique seu appsettings.json.");

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = bootstrapServers
            };

            services.AddSingleton<IProducer<Null, string>>(new ProducerBuilder<Null, string>(producerConfig).Build());

            services.AddSingleton<IKafkaProducer>(provider =>
                new KafkaProducer(
                    provider.GetRequiredService<IProducer<Null, string>>(),
                    topicDistribuicao
                ));
        }
    }
}

using CompraProgramada.Domain.Constants;
using CompraProgramada.Domain.Entities;
using CompraProgramada.Domain.Entities.ContaMasterAggregate;
using CompraProgramada.Domain.Interfaces;
using CompraProgramada.Domain.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CompraProgramada.Infra.Data.Seeds
{
    public static class DatabaseSeed
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var provider = scope.ServiceProvider;

            Log.Information("Iniciando seed do banco de dados...");

            await SeedContaMasterAsync(provider);
            await SeedParametrosAsync(provider);

            Log.Information("Seed do banco de dados finalizado.");
        }

        /// <summary>
        /// Garante que existe exatamente uma ContaMaster no sistema.
        /// A ContaMaster é singleton de negócio: é a conta da corretora
        /// que consolida as compras antes da distribuição para os clientes.
        /// </summary>
        private static async Task SeedContaMasterAsync(IServiceProvider provider)
        {
            var repo = provider.GetRequiredService<IContaMasterRepository>();
            var contaMaster = await repo.ObterAsync();

            if (contaMaster is not null)
            {
                Log.Information("Conta Master já existe (ID: {Id}).", contaMaster.Id);
                return;
            }

            contaMaster = new ContaMaster("Conta Master - Itaú Corretora");
            await repo.AdicionarAsync(contaMaster);

            Log.Information("Conta Master criada com sucesso (ID: {Id}).", contaMaster.Id);
        }

        /// <summary>
        /// Popula os parâmetros configuráveis do sistema com valores padrão.
        /// </summary>
        private static async Task SeedParametrosAsync(IServiceProvider provider)
        {
            var repo = provider.GetRequiredService<IParametroSistemaRepository>();

            var parametrosIniciais = new List<(string chave, string valor, string descricao)>
            {
                (ParametroChaves.VALOR_APORTE_MINIMO, "100.00",
                    "Valor mínimo de aporte mensal do cliente (R$)"),

                (ParametroChaves.PARCELAS_POR_MES, "3",
                    "Número de parcelas mensais (compras nos dias 5, 15 e 25)"),

                (ParametroChaves.LIMIAR_DESVIO_REBALANCEAMENTO, "5.00",
                    "Desvio percentual máximo permitido antes do rebalanceamento (p.p.)"),

                (ParametroChaves.ALIQUOTA_IR_DEDO_DURO, "0.00005",
                    "Alíquota do IR dedo-duro (0,005%)"),

                (ParametroChaves.LIMITE_ISENCAO_IR_VENDAS, "20000.00",
                    "Limite mensal de vendas para isenção de IR (R$)"),

                (ParametroChaves.ALIQUOTA_IR_VENDAS, "0.20",
                    "Alíquota de IR sobre lucro em vendas acima do limite (20%)")
            };

            foreach (var (chave, valor, descricao) in parametrosIniciais)
            {
                var existente = await repo.ObterPorChaveAsync(chave);
                if (existente is null)
                {
                    var parametro = new ParametroSistema(chave, valor, descricao);
                    await repo.AtualizarAsync(parametro);
                }
            }
        }
    }
}
using CompraProgramada.Domain.Constants;
using CompraProgramada.Domain.Entities;
using CompraProgramada.Domain.Entities.ContaMasterAggregate;
using CompraProgramada.Domain.Interfaces;
using CompraProgramada.Domain.Interfaces.Repositories;

namespace CompraProgramada.Api.Extensions
{
    public static class DatabaseSeedExtension
    {
        public static async Task SeedDatabaseAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var provider = scope.ServiceProvider;

            await SeedContaMasterAsync(provider);
            await SeedParametrosAsync(provider);
        }

        private static async Task SeedContaMasterAsync(IServiceProvider provider)
        {
            var repo = provider.GetRequiredService<IContaMasterRepository>();
            var contaMaster = await repo.ObterAsync();

            if (contaMaster is null)
            {
                await repo.AdicionarAsync(new ContaMaster("Conta Master - Itaú Corretora"));
            }
        }

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
using CompraProgramada.Domain.Entities.OrdemCompraAggregate;
using CompraProgramada.Domain.Interfaces.Repositories;
using CompraProgramada.Infra.Data.Context;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;

namespace CompraProgramada.Infra.Data.Repositories
{
    public class OrdemCompraRepository : IOrdemCompraRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public OrdemCompraRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private IDbConnection CreateConnection() => new MySqlConnection(_connectionString);

        public async Task AdicionarAsync(OrdemCompra ordem)
        {
            await _context.Set<OrdemCompra>().AddAsync(ordem);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(OrdemCompra ordem)
        {
            _context.Set<OrdemCompra>().Update(ordem);
            await _context.SaveChangesAsync();
        }

        public async Task<OrdemCompra?> ObterPorIdComItensAsync(int id)
        {
            using var conn = CreateConnection();

            const string sql = @"
                        SELECT 
                            ID As Id, 
                            DATA_EXECUCAO As DataExecucao,
                            VALOR_TOTAL As ValorTotal, STATUS As Status
                        FROM T_ORDEM_COMPRA
                        WHERE ID = @Id;";

            return await conn.QuerySingleOrDefaultAsync<OrdemCompra>(sql, new { Id = id });
        }

        public async Task<IEnumerable<OrdemCompra>> ObterPorDataAsync(DateTime data)
        {
            using var conn = CreateConnection();

            const string sql = @"
                SELECT 
                    ID As Id, 
                    DATA_EXECUCAO As DataExecucao,
                    VALOR_TOTAL As ValorTotal, 
                    STATUS As Status
                FROM T_ORDEM_COMPRA
                WHERE DATE(DATA_EXECUCAO) = @Data;";

            return await conn.QueryAsync<OrdemCompra>(sql, new { Data = data.Date });
        }

        public async Task AdicionarDistribuicaoAsync(Distribuicao distribuicao)
        {
            await _context.Set<Distribuicao>().AddAsync(distribuicao);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Distribuicao>> ObterDistribuicoesPorOrdemAsync(int ordemCompraId)
        {
            using var conn = CreateConnection();

            const string sql = @"
                        SELECT 
                            ID As Id, 
                            ORDEM_COMPRA_ID As OrdemCompraId,
                            CONTA_GRAFICA_ID As ContaGraficaId, 
                            ACAO_ID As AcaoId,
                            QUANTIDADE As Quantidade, 
                            PRECO_UNITARIO As PrecoUnitario,
                            VALOR_IR_DEDO_DURO As ValorIRDedoDuro,
                            DATA_DISTRIBUICAO As DataDistribuicao
                        FROM T_DISTRIBUICAO
                        WHERE ORDEM_COMPRA_ID = @OrdemCompraId;";

            return await conn.QueryAsync<Distribuicao>(sql, new { OrdemCompraId = ordemCompraId });
        }

        public async Task<IEnumerable<Distribuicao>> ObterDistribuicoesPorContaGraficaAsync(int contaGraficaId)
        {
            using var conn = CreateConnection();

            const string sql = @"
                        SELECT 
                            ID As Id, 
                            ORDEM_COMPRA_ID As OrdemCompraId,
                            CONTA_GRAFICA_ID As ContaGraficaId, 
                            ACAO_ID As AcaoId,
                            QUANTIDADE As Quantidade, 
                            PRECO_UNITARIO As PrecoUnitario,
                            VALOR_IR_DEDO_DURO As ValorIRDedoDuro,
                            DATA_DISTRIBUICAO As DataDistribuicao
                        FROM T_DISTRIBUICAO
                        WHERE CONTA_GRAFICA_ID = @ContaGraficaId;";

            return await conn.QueryAsync<Distribuicao>(sql, new { ContaGraficaId = contaGraficaId });
        }
    }
}
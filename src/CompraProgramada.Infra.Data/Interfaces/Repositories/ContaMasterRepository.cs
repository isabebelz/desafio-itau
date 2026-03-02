using CompraProgramada.Domain.Entities.ContaMasterAggregate;
using CompraProgramada.Domain.Interfaces;
using CompraProgramada.Domain.Interfaces.Repositories;
using CompraProgramada.Infra.Data.Context;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace CompraProgramada.Infra.Data.Repositories
{
    public class ContaMasterRepository : IContaMasterRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public ContaMasterRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private IDbConnection CreateConnection() => new MySqlConnection(_connectionString);

        public async Task AdicionarAsync(ContaMaster contaMaster)
        {
            await _context.Set<ContaMaster>().AddAsync(contaMaster);
            await _context.SaveChangesAsync();
        }

        public async Task<ContaMaster?> ObterAsync()
        {
            using var conn = CreateConnection();

            const string sql = @"
                        SELECT 
                            ID As Id, 
                            DESCRICAO As Descricao
                        FROM T_CONTA_MASTER
                        LIMIT 1;";

            return await conn.QuerySingleOrDefaultAsync<ContaMaster>(sql);
        }

        public async Task AdicionarCustodiaAsync(CustodiaMaster custodia)
        {
            await _context.Set<CustodiaMaster>().AddAsync(custodia);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarCustodiaAsync(CustodiaMaster custodia)
        {
            _context.Set<CustodiaMaster>().Update(custodia);
            await _context.SaveChangesAsync();
        }

        public async Task<CustodiaMaster?> ObterCustodiaPorAcaoAsync(int contaMasterId, int acaoId)
        {
            using var conn = CreateConnection();

            const string sql = @"
                SELECT 
                    ID As Id, 
                    CONTA_MASTER_ID As ContaMasterId,
                    ACAO_ID As AcaoId, 
                    QUANTIDADE As Quantidade,
                    PRECO_MEDIO As PrecoMedio
                FROM T_CUSTODIA_MASTER
                WHERE CONTA_MASTER_ID = @ContaMasterId AND ACAO_ID = @AcaoId;";

            return await conn.QuerySingleOrDefaultAsync<CustodiaMaster>(sql,
                new { ContaMasterId = contaMasterId, AcaoId = acaoId });
        }

        public async Task<IEnumerable<CustodiaMaster>> ObterTodasCustodiasAsync(int contaMasterId)
        {
            using var conn = CreateConnection();

            const string sql = @"
                        SELECT 
                            ID As Id, 
                            CONTA_MASTER_ID As ContaMasterId,
                            ACAO_ID As AcaoId, 
                            QUANTIDADE As Quantidade,
                            PRECO_MEDIO As PrecoMedio
                        FROM T_CUSTODIA_MASTER
                        WHERE CONTA_MASTER_ID = @ContaMasterId;";

            return await conn.QueryAsync<CustodiaMaster>(sql, new { ContaMasterId = contaMasterId });
        }
    }
}
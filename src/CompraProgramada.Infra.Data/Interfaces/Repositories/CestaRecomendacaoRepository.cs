using CompraProgramada.Domain.Entities.CestaAggregate;
using CompraProgramada.Domain.Interfaces.Repositories;
using CompraProgramada.Infra.Data.Context;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace CompraProgramada.Infra.Data.Interfaces.Repositories
{
    public class CestaRecomendacaoRepository : ICestaRecomendacaoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public CestaRecomendacaoRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private IDbConnection CreateConnection() => new MySqlConnection(_connectionString);

        public async Task AdicionarAsync(CestaRecomendacao cesta)
        {
            await _context.Set<CestaRecomendacao>().AddAsync(cesta);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(CestaRecomendacao cesta)
        {
            _context.Set<CestaRecomendacao>().Update(cesta);
            await _context.SaveChangesAsync();
        }

        public async Task<CestaRecomendacao?> ObterAtivaComItensAsync()
        {
            using var conn = CreateConnection();

            const string sql = @"
                        SELECT 
                            ID As Id, 
                            ATIVA As Ativa, 
                            DATA_VIGENCIA As DataVigencia
                        FROM T_CESTA_RECOMENDACAO
                        WHERE ATIVA = 1
                        LIMIT 1;";

            const string sqlItens = @"
                        SELECT 
                            i.ID As Id, 
                            i.CESTA_RECOMENDACAO_ID As CestaRecomendacaoId,
                            i.ACAO_ID As AcaoId, 
                            i.PERCENTUAL As Percentual,
                            a.CODIGO As Codigo, 
                            a.NOME_EMPRESA As NomeEmpresa
                        FROM T_CESTA_RECOMENDACAO_ITEM i
                        INNER JOIN T_ACAO a ON a.ID = i.ACAO_ID
                        WHERE i.CESTA_RECOMENDACAO_ID = @CestaId;";

            var cesta = await conn.QuerySingleOrDefaultAsync<CestaRecomendacao>(sql);

            if (cesta != null)
            {
                var itens = await conn.QueryAsync<CestaRecomendacaoItem>(sqlItens,
                    new { CestaId = cesta.Id });
                // Itens serão retornados separadamente no serviço via DTO
            }

            return cesta;
        }

        public async Task<CestaRecomendacao?> ObterPorIdComItensAsync(int id)
        {
            using var conn = CreateConnection();

            const string sql = @"
                        SELECT 
                            ID As Id,   
                            ATIVA As Ativa, 
                            DATA_VIGENCIA As DataVigencia
                        FROM T_CESTA_RECOMENDACAO
                        WHERE ID = @Id;";

            return await conn.QuerySingleOrDefaultAsync<CestaRecomendacao>(sql, new { Id = id });
        }

        public async Task<IEnumerable<CestaRecomendacao>> ObterHistoricoAsync()
        {
            using var conn = CreateConnection();

            const string sql = @"
                        SELECT 
                            ID As Id,   
                            ATIVA As Ativa, 
                            DATA_VIGENCIA As DataVigencia,
                            DATA_CRIACAO As DataCriacao
                        FROM T_CESTA_RECOMENDACAO
                        ORDER BY DATA_CRIACAO DESC;";

            return await conn.QueryAsync<CestaRecomendacao>(sql);
        }
    }
}

using CompraProgramada.Domain.Entities;
using CompraProgramada.Domain.Interfaces.Repositories;
using CompraProgramada.Infra.Data.Context;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;

namespace CompraProgramada.Infra.Data.Repositories
{
    public class CotacaoRepository : ICotacaoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public CotacaoRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private IDbConnection CreateConnection() => new MySqlConnection(_connectionString);

        public async Task SalvarLoteAsync(IEnumerable<Cotacao> cotacoes)
        {
            await _context.Set<Cotacao>().AddRangeAsync(cotacoes);
            await _context.SaveChangesAsync();
        }

        public async Task<Cotacao?> ObterUltimaPorTickerAsync(string codigo)
        {
            using var conn = CreateConnection();

            const string sql = @"
                SELECT 
                    ID As Id,
                    CODIGO As Codigo,
                    PRECO_FECHAMENTO As PrecoFechamento,
                    PRECO_ABERTURA As PrecoAbertura,
                    PRECO_MAXIMO As PrecoMaximo,
                    PRECO_MINIMO As PrecoMinimo,
                    DATA_PREGAO As DataPregao,
                    CODIGO_BDI As CodigoBDI,
                    TIPO_MERCADO As TipoMercado
                FROM T_COTACAO
                WHERE Codigo = @Codigo
                ORDER BY DATA_PREGAO DESC
                LIMIT 1;";

            return await conn.QuerySingleOrDefaultAsync<Cotacao>(sql, new { Codigo = codigo.ToUpper() });
        }

        public async Task<IEnumerable<Cotacao>> ObterPorDataAsync(DateTime data)
        {
            using var conn = CreateConnection();

            const string sql = @"
                SELECT 
                    ID As Id,
                    CODIGO As Codigo,
                    PRECO_FECHAMENTO As PrecoFechamento,
                    DATA_PREGAO As DataPregao,
                    TIPO_MERCADO As TipoMercado
                FROM T_COTACAO
                WHERE DATE(DATA_PREGAO) = DATE(@Data);";

            return await conn.QueryAsync<Cotacao>(sql, new { Data = data });
        }

        public async Task<bool> ExisteCotacaoNaDataAsync(DateTime data)
        {
            using var conn = CreateConnection();

            const string sql = "SELECT EXISTS(SELECT 1 FROM T_COTACAO WHERE DATE(DATA_PREGAO) = DATE(@Data));";

            return await conn.ExecuteScalarAsync<bool>(sql, new { Data = data });
        }
    }
}
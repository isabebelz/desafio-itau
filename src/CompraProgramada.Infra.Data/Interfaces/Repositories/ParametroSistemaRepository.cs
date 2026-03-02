using CompraProgramada.Domain.Entities;
using CompraProgramada.Domain.Interfaces;
using CompraProgramada.Infra.Data.Context;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace CompraProgramada.Infra.Data.Repositories
{
    public class ParametroSistemaRepository : IParametroSistemaRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public ParametroSistemaRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private IDbConnection CreateConnection() => new MySqlConnection(_connectionString);

        public async Task<ParametroSistema?> ObterPorChaveAsync(string chave)
        {
            using var conn = CreateConnection();

            const string sql = @"
                        SELECT 
                            ID As Id, 
                            CHAVE As Chave, 
                            VALOR As Valor,
                            DESCRICAO As Descricao
                        FROM T_PARAMETRO_SISTEMA
                        WHERE CHAVE = @Chave;";

            return await conn.QuerySingleOrDefaultAsync<ParametroSistema>(sql, new { Chave = chave });
        }

        public async Task<IEnumerable<ParametroSistema>> ObterTodosAsync()
        {
            using var conn = CreateConnection();

            const string sql = @"
                        SELECT 
                            ID As Id, 
                            CHAVE As Chave, 
                            VALOR As Valor,
                            DESCRICAO As Descricao
                        FROM T_PARAMETRO_SISTEMA;";

            return await conn.QueryAsync<ParametroSistema>(sql);
        }

        public async Task AtualizarAsync(ParametroSistema parametro)
        {
            _context.Set<ParametroSistema>().Update(parametro);
            await _context.SaveChangesAsync();
        }
    }
}
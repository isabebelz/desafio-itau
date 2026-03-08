using CompraProgramada.Domain.Entities;
using CompraProgramada.Domain.Interfaces.Repositories;
using CompraProgramada.Infra.Data.Context;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace CompraProgramada.Infra.Data.Interfaces.Repositories
{
    public class AcaoRepository : IAcaoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public AcaoRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException(nameof(configuration), "The Connection String 'DefaultConnection' was not found or is empty in appsettings.json."); ;
        }
        private IDbConnection CreateConnection() => new MySqlConnection(_connectionString);

        public async Task AdicionarAsync(Acao acao)
        {
            await _context.Set<Acao>().AddAsync(acao);
            await _context.SaveChangesAsync();
        }

        public Task AtualizarAsync(Acao acao)
        {
            _context.Entry(acao).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public async Task<Acao?> ObterPorIdAsync(int id)
        {
            using var conn = CreateConnection();
            const string sql = @"
                        SELECT 
                            ID As Id,
                            CODIGO As Codigo,
                            NOME_EMPRESA As NomeEmpresa,
                            PRECO As Preco,
                            ATIVO As Ativo
                        FROM T_ACAO
                        WHERE Id = @Id;";
            return await conn.QuerySingleOrDefaultAsync<Acao>(sql, new { Id = id });
        }

        public async Task<Acao?> ObterPorCodigoAsync(string codigo)
        {
            using var conn = CreateConnection();
            const string sql = @"
                         SELECT 
                            ID As Id,
                            CODIGO As Codigo,
                            NOME_EMPRESA As NomeEmpresa,
                            PRECO As Preco,
                            ATIVO As Ativo
                        FROM T_ACAO
                        WHERE Codigo = @Codigo;";
            return await conn.QuerySingleOrDefaultAsync<Acao>(sql, new { Codigo = codigo });
        }

        public async Task<IEnumerable<Acao>> ObterTodosAtivosAsync()
        {
            return await ObterTodasAsync(true);
        }

        public async Task<IEnumerable<Acao>> ObterTodasAsync(bool? ativo)
        {
            using var conn = CreateConnection();

            var sql = @"
                SELECT 
                    ID As Id,
                    CODIGO As Codigo,
                    NOME_EMPRESA As NomeEmpresa,
                    PRECO As Preco,
                    ATIVO As Ativo
                FROM T_ACAO";

            var param = new DynamicParameters();

            if (ativo.HasValue)
            {
                sql += " WHERE ATIVO = @ATIVO";
                param.Add("ATIVO", ativo.Value);
            }

            return await conn.QueryAsync<Acao>(sql, param);
        }
    }
}

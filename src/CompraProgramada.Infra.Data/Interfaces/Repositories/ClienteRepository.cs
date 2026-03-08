using CompraProgramada.Domain.Entities.ClienteAggregate;
using CompraProgramada.Domain.Interfaces.Repositories;
using CompraProgramada.Infra.Data.Context;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace CompraProgramada.Infra.Data.Interfaces.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public ClienteRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private IDbConnection CreateConnection() => new MySqlConnection(_connectionString);

        public async Task AdicionarAsync(Cliente cliente)
        {
            await _context.Set<Cliente>().AddAsync(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Cliente cliente)
        {
            _context.Set<Cliente>().Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task<Cliente?> ObterPorIdAsync(int id)
        {
            using var conn = CreateConnection();

            const string sql = @"
                        SELECT 
                            ID As Id, 
                            NOME As Nome, 
                            CPF,
                            EMAIL As Email,
                            VALOR_APORTE_MENSAL As ValorAporteMensal,
                            ATIVO As Ativo,
                            DATA_ADESAO As DataAdesao, 
                            DATA_SAIDA As DataSaida
                        FROM T_CLIENTE
                        WHERE ID = @Id;";

            return await conn.QuerySingleOrDefaultAsync<Cliente>(sql, new { Id = id });
        }

        public async Task<Cliente?> ObterPorCpfAsync(string cpf)
        {
            using var conn = CreateConnection();

            const string sql = @"
                        SELECT 
                            ID As Id, 
                            NOME As Nome, 
                            CPF, 
                            EMAIL As Email,
                            VALOR_APORTE_MENSAL As ValorAporteMensal, 
                            ATIVO As Ativo,
                            DATA_ADESAO As DataAdesao, 
                            DATA_SAIDA As DataSaida
                        FROM T_CLIENTE
                        WHERE CPF = @CPF;";

            return await conn.QuerySingleOrDefaultAsync<Cliente>(sql, new { CPF = cpf });
        }

        public async Task<IEnumerable<Cliente>> ObterTodosAsync(bool? ativo)
        {
            using var conn = CreateConnection();

            var sql = @"
                SELECT 
                    ID As Id, 
                    NOME As Nome, 
                    CPF, 
                    EMAIL As Email,
                    VALOR_APORTE_MENSAL As ValorAporteMensal, 
                    ATIVO As Ativo,
                    DATA_ADESAO As DataAdesao, 
                    DATA_SAIDA As DataSaida
                FROM T_CLIENTE";

            if (ativo.HasValue)
            {
                sql += " WHERE ATIVO = @Ativo";
                return await conn.QueryAsync<Cliente>(sql, new { Ativo = ativo.Value });
            }

            return await conn.QueryAsync<Cliente>(sql);
        }

        public async Task<IEnumerable<Cliente>> ObterTodosAtivosAsync()
        {
            return await ObterTodosAsync(true);
        }

        public async Task AdicionarContaGraficaAsync(ContaGrafica contaGrafica)
        {
            await _context.Set<ContaGrafica>().AddAsync(contaGrafica);
            await _context.SaveChangesAsync();
        }

        public async Task<ContaGrafica?> ObterContaGraficaPorClienteIdAsync(int clienteId)
        {
            using var conn = CreateConnection();

            const string sql = @"
                        SELECT 
                            ID As Id, 
                            CLIENTE_ID As ClienteId
                        FROM T_CONTA_GRAFICA
                        WHERE CLIENTE_ID = @ClienteId;";

            return await conn.QuerySingleOrDefaultAsync<ContaGrafica>(sql, new { ClienteId = clienteId });
        }

        public async Task AdicionarCustodiaFilhoteAsync(CustodiaFilhote custodia)
        {
            await _context.Set<CustodiaFilhote>().AddAsync(custodia);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarCustodiaFilhoteAsync(CustodiaFilhote custodia)
        {
            _context.Set<CustodiaFilhote>().Update(custodia);
            await _context.SaveChangesAsync();
        }

        public async Task<CustodiaFilhote?> ObterCustodiaPorContaEAcaoAsync(int contaGraficaId, int acaoId)
        {
            using var conn = CreateConnection();

            const string sql = @"
                        SELECT 
                            ID As Id, 
                            CONTA_GRAFICA_ID As ContaGraficaId,
                            ACAO_ID As AcaoId, 
                            QUANTIDADE As Quantidade,
                            PRECO_MEDIO As PrecoMedio
                        FROM T_CUSTODIA_FILHOTE
                        WHERE CONTA_GRAFICA_ID = @ContaGraficaId AND ACAO_ID = @AcaoId;";

            return await conn.QuerySingleOrDefaultAsync<CustodiaFilhote>(sql,
                new { ContaGraficaId = contaGraficaId, AcaoId = acaoId });
        }

        public async Task<IEnumerable<CustodiaFilhote>> ObterCustodiasPorContaGraficaAsync(int contaGraficaId)
        {
            using var conn = CreateConnection();

            const string sql = @"
                SELECT 
                    ID As Id, 
                    CONTA_GRAFICA_ID As ContaGraficaId,
                    ACAO_ID As AcaoId, 
                    QUANTIDADE As Quantidade,
                    PRECO_MEDIO As PrecoMedio
                FROM T_CUSTODIA_FILHOTE
                WHERE CONTA_GRAFICA_ID = @ContaGraficaId;";

            return await conn.QueryAsync<CustodiaFilhote>(sql, new { ContaGraficaId = contaGraficaId });
        }
    }
}
using CompraProgramada.Domain.Entities.ClienteAggregate;

namespace CompraProgramada.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repositório do agregado Cliente.
    /// Gerencia Cliente, ContaGrafica e CustodiaFilhote.
    /// </summary>
    public interface IClienteRepository
    {
        Task AdicionarAsync(Cliente cliente);
        Task AtualizarAsync(Cliente cliente);
        Task<Cliente?> ObterPorIdAsync(int id);
        Task<Cliente?> ObterPorCpfAsync(string cpf);
        Task<IEnumerable<Cliente>> ObterTodosAtivosAsync();
        Task<IEnumerable<Cliente>> ObterTodosAsync(bool? ativo);
        Task<IEnumerable<Cliente>> ObterPorIdsAsync(IEnumerable<int> ids);

        Task AdicionarContaGraficaAsync(ContaGrafica contaGrafica);
        Task<ContaGrafica?> ObterContaGraficaPorClienteIdAsync(int clienteId);

        Task AdicionarCustodiaFilhoteAsync(CustodiaFilhote custodia);
        Task AtualizarCustodiaFilhoteAsync(CustodiaFilhote custodia);
        Task<CustodiaFilhote?> ObterCustodiaPorContaEAcaoAsync(int contaGraficaId, int acaoId);
        Task<IEnumerable<CustodiaFilhote>> ObterCustodiasPorContaGraficaAsync(int contaGraficaId);
    }
}

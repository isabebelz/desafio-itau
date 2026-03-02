using CompraProgramada.Domain.Entities.OrdemCompraAggregate;

namespace CompraProgramada.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repositório do agregado OrdemCompra.
    /// Gerencia a ordem, seus itens e as distribuições.
    /// </summary>
    public interface IOrdemCompraRepository
    {
        Task AdicionarAsync(OrdemCompra ordem);
        Task AtualizarAsync(OrdemCompra ordem);
        Task<OrdemCompra?> ObterPorIdComItensAsync(int id);
        Task<IEnumerable<OrdemCompra>> ObterPorDataAsync(DateTime data);

        Task AdicionarDistribuicaoAsync(Distribuicao distribuicao);
        Task<IEnumerable<Distribuicao>> ObterDistribuicoesPorOrdemAsync(int ordemCompraId);
        Task<IEnumerable<Distribuicao>> ObterDistribuicoesPorContaGraficaAsync(int contaGraficaId);
    }
}

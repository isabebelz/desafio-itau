using CompraProgramada.Domain.Entities;

namespace CompraProgramada.Domain.Interfaces.Repositories
{
    public interface ICotacaoRepository
    {
        Task SalvarLoteAsync(IEnumerable<Cotacao> cotacoes);
        Task<Cotacao?> ObterUltimaPorTickerAsync(string codigo);
        Task<bool> ExisteCotacaoNaDataAsync(DateTime data);
    }
}

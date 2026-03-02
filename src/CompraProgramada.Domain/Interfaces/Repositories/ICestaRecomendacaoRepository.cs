using CompraProgramada.Domain.Entities.CestaAggregate;

namespace CompraProgramada.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repositório do agregado CestaRecomendacao.
    /// Gerencia a cesta e seus itens como unidade.
    /// </summary>
    public interface ICestaRecomendacaoRepository
    {
        Task AdicionarAsync(CestaRecomendacao cesta);
        Task AtualizarAsync(CestaRecomendacao cesta);
        Task<CestaRecomendacao?> ObterAtivaComItensAsync();
        Task<CestaRecomendacao?> ObterPorIdComItensAsync(int id);
        Task<IEnumerable<CestaRecomendacao>> ObterHistoricoAsync();
    }
}

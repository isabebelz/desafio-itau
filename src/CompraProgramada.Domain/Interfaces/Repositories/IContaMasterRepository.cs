using CompraProgramada.Domain.Entities.ContaMasterAggregate;

namespace CompraProgramada.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repositório do agregado ContaMaster.
    /// Gerencia a conta master e sua custódia.
    /// </summary>
    public interface IContaMasterRepository
    {
        Task AdicionarAsync(ContaMaster contaMaster);
        Task<ContaMaster?> ObterAsync();

        Task AdicionarCustodiaAsync(CustodiaMaster custodia);
        Task AtualizarCustodiaAsync(CustodiaMaster custodia);
        Task<CustodiaMaster?> ObterCustodiaPorAcaoAsync(int contaMasterId, int acaoId);
        Task<IEnumerable<CustodiaMaster>> ObterTodasCustodiasAsync(int contaMasterId);
    }
}

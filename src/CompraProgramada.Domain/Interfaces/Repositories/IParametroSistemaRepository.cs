using CompraProgramada.Domain.Entities;

namespace CompraProgramada.Domain.Interfaces
{
    public interface IParametroSistemaRepository
    {
        Task<ParametroSistema?> ObterPorChaveAsync(string chave);
        Task<IEnumerable<ParametroSistema>> ObterTodosAsync();
        Task AtualizarAsync(ParametroSistema parametro);
    }
}
using CompraProgramada.Domain.Entities;

namespace CompraProgramada.Domain.Interfaces
{
    /// <summary>
    /// Contrato que define as operações de persistência para a entidade Acao.
    /// A camada de Aplicação depende desta interface, não da implementação concreta.
    /// </summary>
    public interface IAcaoRepository
    {
        Task AdicionarAsync(Acao acao);
        Task AtualizarAsync(Acao acao);
        Task<Acao?> ObterPorIdAsync(int id);
        Task<Acao?> ObterPorCodigoAsync(string codigo);
        Task<IEnumerable<Acao>> ObterTodasAsync(bool? ativo);
    }
}
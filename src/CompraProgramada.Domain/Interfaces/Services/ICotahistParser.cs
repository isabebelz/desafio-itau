using CompraProgramada.Domain.Entities;

namespace CompraProgramada.Domain.Interfaces.Services
{
    public interface ICotahistParser
    {
        IEnumerable<Cotacao> Parse(string caminhoArquivo);
    }
}

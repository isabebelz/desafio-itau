using CompraProgramada.Domain.Interfaces.Repositories;
using CompraProgramada.Domain.Interfaces.Services;
using MediatR;
namespace CompraProgramada.Application.Features.Cotacoes.Commands
{
    public class ImportarArquivoCotacaoHandler(ICotahistParser _parser,
                                               ICotacaoRepository _cotacaoRepository) : IRequestHandler<ImportarArquivoCotacaoCommand, bool>
    {

        public async Task<bool> Handle(ImportarArquivoCotacaoCommand request, CancellationToken cancellationToken)
        {
            var cotacoes = _parser.Parse(request.CaminhoArquivo);

            var filtradas = cotacoes.Where(c => c.TipoMercado == 10 || c.TipoMercado == 20);

            await _cotacaoRepository.SalvarLoteAsync(filtradas);

            return true;
        }
    }
}

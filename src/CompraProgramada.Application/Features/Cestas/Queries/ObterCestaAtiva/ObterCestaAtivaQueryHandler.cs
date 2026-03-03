using CompraProgramada.Domain.DTOs.Cestas;
using CompraProgramada.Domain.Interfaces.Repositories;
using MediatR;

namespace CompraProgramada.Application.Features.Cestas.Queries.ObterCestaAtiva
{
    public sealed class ObterCestaAtivaQueryHandler(
        ICestaRecomendacaoRepository _cestaRepository,
        IAcaoRepository _acaoRepository) : IRequestHandler<ObterCestaAtivaQuery, CestaResponse?>
    {
        public async Task<CestaResponse?> Handle(ObterCestaAtivaQuery query, CancellationToken cancellationToken)
        {
            var cesta = await _cestaRepository.ObterAtivaComItensAsync();
            if (cesta is null) return null;

            var itensResponse = new List<CestaItemResponse>();
            foreach (var item in cesta.Itens)
            {
                var acao = await _acaoRepository.ObterPorIdAsync(item.AcaoId);
                itensResponse.Add(new CestaItemResponse(
                    item.AcaoId,
                    acao?.Codigo ?? "N/A",
                    acao?.NomeEmpresa ?? "N/A",
                    item.Percentual
                ));
            }

            return new CestaResponse(
                cesta.Id,
                cesta.Ativa,
                cesta.DataVigencia,
                cesta.DataCriacao,
                itensResponse
            );
        }
    }
}
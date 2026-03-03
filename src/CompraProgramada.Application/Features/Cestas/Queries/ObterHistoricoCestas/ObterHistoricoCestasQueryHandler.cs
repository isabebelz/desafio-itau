using CompraProgramada.Domain.DTOs.Cestas;
using CompraProgramada.Domain.Interfaces.Repositories;
using MediatR;

namespace CompraProgramada.Application.Features.Cestas.Queries.ObterHistoricoCestas
{
    public sealed class ObterHistoricoCestasQueryHandler(
        ICestaRecomendacaoRepository _cestaRepository,
        IAcaoRepository _acaoRepository) : IRequestHandler<ObterHistoricoCestasQuery, IEnumerable<CestaResponse>>
    {
        public async Task<IEnumerable<CestaResponse>> Handle(ObterHistoricoCestasQuery query, CancellationToken cancellationToken)
        {
            var cestas = await _cestaRepository.ObterHistoricoAsync();
            var resultado = new List<CestaResponse>();

            foreach (var cesta in cestas)
            {
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

                resultado.Add(new CestaResponse(
                    cesta.Id,
                    cesta.Ativa,
                    cesta.DataVigencia,
                    cesta.DataCriacao,
                    itensResponse
                ));
            }

            return resultado;
        }
    }
}
using CompraProgramada.Domain.DTOs.Cestas;
using MediatR;

namespace CompraProgramada.Application.Features.Cestas.Queries.ObterHistoricoCestas
{
    public sealed record ObterHistoricoCestasQuery() : IRequest<IEnumerable<CestaResponse>>;
}
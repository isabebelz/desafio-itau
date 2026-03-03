using CompraProgramada.Domain.DTOs.Cestas;
using MediatR;

namespace CompraProgramada.Application.Features.Cestas.Queries.ObterCestaAtiva
{
    public sealed record ObterCestaAtivaQuery() : IRequest<CestaResponse?>;
}
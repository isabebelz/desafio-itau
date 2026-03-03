using CompraProgramada.Domain.DTOs.Cestas;
using MediatR;

namespace CompraProgramada.Application.Features.Cestas.Commands.CadastrarCesta
{
    public sealed record CadastrarCestaCommand(CadastrarCestaDTO Cesta) : IRequest<CestaResponse>;
}
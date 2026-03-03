using CompraProgramada.Domain.DTOs.Clientes;
using MediatR;

namespace CompraProgramada.Application.Features.Clientes.Queries.ConsultarCarteira
{
    public sealed record ConsultarCarteiraQuery(int ClienteId) : IRequest<CarteiraResponse>;
}
using CompraProgramada.Domain.DTOs.Clientes;
using MediatR;

namespace CompraProgramada.Application.Features.Clientes.Commands.SairCliente
{
    public sealed record SairClienteCommand(int ClienteId) : IRequest<ClienteResponse>;
}
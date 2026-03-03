using CompraProgramada.Domain.DTOs.Clientes;
using MediatR;

namespace CompraProgramada.Application.Features.Clientes.Commands.ReativarCliente
{
    public sealed record ReativarClienteCommand(int ClienteId) : IRequest<ClienteResponse>;
}
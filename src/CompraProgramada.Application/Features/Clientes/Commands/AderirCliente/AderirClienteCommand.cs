using CompraProgramada.Domain.DTOs.Clientes;
using MediatR;

namespace CompraProgramada.Application.Features.Clientes.Commands.AderirCliente
{
    public sealed record AderirClienteCommand(AderirClienteDTO Cliente) : IRequest<ClienteResponse>;
}
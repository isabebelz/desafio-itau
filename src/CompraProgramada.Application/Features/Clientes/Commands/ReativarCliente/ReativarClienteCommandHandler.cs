using AutoMapper;
using CompraProgramada.Domain.DTOs.Clientes;
using CompraProgramada.Domain.Exceptions;
using CompraProgramada.Domain.Interfaces.Repositories;
using MediatR;

namespace CompraProgramada.Application.Features.Clientes.Commands.ReativarCliente
{
    public sealed class ReativarClienteCommandHandler(
        IClienteRepository _clienteRepository,
        IMapper _mapper) : IRequestHandler<ReativarClienteCommand, ClienteResponse>
    {
        public async Task<ClienteResponse> Handle(ReativarClienteCommand command, CancellationToken cancellationToken)
        {
            var cliente = await _clienteRepository.ObterPorIdAsync(command.ClienteId)
                ?? throw new DomainException("Cliente não encontrado.");

            cliente.Reativar();

            await _clienteRepository.AtualizarAsync(cliente);

            return _mapper.Map<ClienteResponse>(cliente);
        }
    }
}
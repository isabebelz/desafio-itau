using AutoMapper;
using CompraProgramada.Domain.DTOs.Clientes;
using CompraProgramada.Domain.Exceptions;
using CompraProgramada.Domain.Interfaces.Repositories;
using MediatR;

namespace CompraProgramada.Application.Features.Clientes.Commands.SairCliente
{
    public sealed class SairClienteCommandHandler(
        IClienteRepository _clienteRepository,
        IMapper _mapper) : IRequestHandler<SairClienteCommand, ClienteResponse>
    {
        public async Task<ClienteResponse> Handle(SairClienteCommand command, CancellationToken cancellationToken)
        {
            var cliente = await _clienteRepository.ObterPorIdAsync(command.ClienteId)
                ?? throw new DomainException("Cliente não encontrado.");

            cliente.Sair();

            await _clienteRepository.AtualizarAsync(cliente);

            return _mapper.Map<ClienteResponse>(cliente);
        }
    }
}
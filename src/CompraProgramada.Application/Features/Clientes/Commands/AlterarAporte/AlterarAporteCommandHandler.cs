using AutoMapper;
using CompraProgramada.Domain.Constants;
using CompraProgramada.Domain.DTOs.Clientes;
using CompraProgramada.Domain.Exceptions;
using CompraProgramada.Domain.Interfaces;
using CompraProgramada.Domain.Interfaces.Repositories;
using MediatR;

namespace CompraProgramada.Application.Features.Clientes.Commands.AlterarAporte
{
    public sealed class AlterarAporteCommandHandler(
        IClienteRepository _clienteRepository,
        IParametroSistemaRepository _parametroRepository,
        IMapper _mapper) : IRequestHandler<AlterarAporteCommand, ClienteResponse>
    {
        public async Task<ClienteResponse> Handle(AlterarAporteCommand command, CancellationToken cancellationToken)
        {
            var cliente = await _clienteRepository.ObterPorIdAsync(command.ClienteId)
                ?? throw new DomainException("Cliente não encontrado.");

            var parametro = await _parametroRepository.ObterPorChaveAsync(ParametroChaves.VALOR_APORTE_MINIMO);
            var valorMinimo = parametro?.ObterComoDecimal() ?? 100.00m;

            cliente.AlterarValorAporte(command.Aporte.NovoValor, valorMinimo);

            await _clienteRepository.AtualizarAsync(cliente);

            return _mapper.Map<ClienteResponse>(cliente);
        }
    }
}
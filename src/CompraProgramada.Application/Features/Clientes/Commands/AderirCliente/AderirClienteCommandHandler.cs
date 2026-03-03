using AutoMapper;
using CompraProgramada.Domain.Constants;
using CompraProgramada.Domain.DTOs.Clientes;
using CompraProgramada.Domain.Entities.ClienteAggregate;
using CompraProgramada.Domain.Interfaces;
using CompraProgramada.Domain.Interfaces.Repositories;
using MediatR;
using ValidationException = CompraProgramada.Application.Common.Exceptions.ValidationException;

namespace CompraProgramada.Application.Features.Clientes.Commands.AderirCliente
{
    public sealed class AderirClienteCommandHandler(
        IClienteRepository _clienteRepository,
        IParametroSistemaRepository _parametroRepository,
        IMapper _mapper) : IRequestHandler<AderirClienteCommand, ClienteResponse>
    {
        public async Task<ClienteResponse> Handle(AderirClienteCommand command, CancellationToken cancellationToken)
        {
            var clienteExistente = await _clienteRepository.ObterPorCpfAsync(command.Cliente.CPF);

            if (clienteExistente is not null)
                throw new ValidationException(
                [
                    new("CPF", "Já existe um cliente cadastrado com este CPF.")
                ]);

            var parametro = await _parametroRepository.ObterPorChaveAsync(ParametroChaves.VALOR_APORTE_MINIMO);
            var valorMinimo = parametro?.ObterComoDecimal() ?? 100.00m;

            var cliente = new Cliente(
                command.Cliente.Nome,
                command.Cliente.CPF,
                command.Cliente.Email,
                command.Cliente.ValorAporteMensal,
                valorMinimo
            );

            await _clienteRepository.AdicionarAsync(cliente);

            cliente.CriarContaGrafica();
            await _clienteRepository.AtualizarAsync(cliente);

            return _mapper.Map<ClienteResponse>(cliente);
        }
    }
}
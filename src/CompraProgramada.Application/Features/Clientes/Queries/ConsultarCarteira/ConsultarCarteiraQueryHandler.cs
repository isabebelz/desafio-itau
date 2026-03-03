using CompraProgramada.Domain.DTOs.Clientes;
using CompraProgramada.Domain.Exceptions;
using CompraProgramada.Domain.Interfaces.Repositories;
using MediatR;

namespace CompraProgramada.Application.Features.Clientes.Queries.ConsultarCarteira
{
    public sealed class ConsultarCarteiraQueryHandler(
        IClienteRepository _clienteRepository,
        IAcaoRepository _acaoRepository) : IRequestHandler<ConsultarCarteiraQuery, CarteiraResponse>
    {
        public async Task<CarteiraResponse> Handle(ConsultarCarteiraQuery query, CancellationToken cancellationToken)
        {
            var cliente = await _clienteRepository.ObterPorIdAsync(query.ClienteId)
                ?? throw new DomainException("Cliente não encontrado.");

            var contaGrafica = await _clienteRepository.ObterContaGraficaPorClienteIdAsync(cliente.Id);
            if (contaGrafica is null)
            {
                return new CarteiraResponse(
                    cliente.Id, cliente.Nome, 0, 0, 0, 0,
                    []
                );
            }

            var custodias = await _clienteRepository.ObterCustodiasPorContaGraficaAsync(contaGrafica.Id);
            if (!custodias.Any())
            {
                return new CarteiraResponse(
                    cliente.Id, cliente.Nome, 0, 0, 0, 0,
                    []
                );
            }

            var ativos = new List<AtivoCarteiraDTO>();
            decimal valorInvestidoTotal = 0;
            decimal valorAtualTotal = 0;

            foreach (var custodia in custodias.Where(c => c.Quantidade > 0))
            {
                var acao = await _acaoRepository.ObterPorIdAsync(custodia.AcaoId);
                if (acao is null) continue;

                var cotacaoAtual = acao.Preco;
                var valorAtual = custodia.CalcularValorAtual(cotacaoAtual);
                var valorInvestido = custodia.CalcularValorInvestido();
                var pl = custodia.CalcularPL(cotacaoAtual);
                var rentabilidade = custodia.CalcularRentabilidade(cotacaoAtual);

                valorInvestidoTotal += valorInvestido;
                valorAtualTotal += valorAtual;

                ativos.Add(new AtivoCarteiraDTO(
                    acao.Codigo,
                    acao.NomeEmpresa,
                    custodia.Quantidade,
                    custodia.PrecoMedio,
                    cotacaoAtual,
                    valorAtual,
                    pl,
                    rentabilidade,
                    0
                ));
            }

            if (valorAtualTotal > 0)
            {
                ativos = ativos.Select(a => a with
                {
                    PercentualCarteira = Math.Round((a.ValorAtual / valorAtualTotal) * 100, 2)
                }).ToList();
            }

            var plTotal = valorAtualTotal - valorInvestidoTotal;
            var rentabilidadeTotal = valorInvestidoTotal > 0
                ? Math.Round(((valorAtualTotal - valorInvestidoTotal) / valorInvestidoTotal) * 100, 2)
                : 0;

            return new CarteiraResponse(
                cliente.Id,
                cliente.Nome,
                Math.Round(valorInvestidoTotal, 2),
                Math.Round(valorAtualTotal, 2),
                Math.Round(plTotal, 2),
                rentabilidadeTotal,
                ativos
            );
        }
    }
}
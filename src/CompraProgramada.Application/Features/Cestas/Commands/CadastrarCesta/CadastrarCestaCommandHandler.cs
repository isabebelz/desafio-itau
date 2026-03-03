using CompraProgramada.Domain.DTOs.Cestas;
using CompraProgramada.Domain.Entities.CestaAggregate;
using CompraProgramada.Domain.Exceptions;
using CompraProgramada.Domain.Interfaces.Repositories;
using MediatR;

namespace CompraProgramada.Application.Features.Cestas.Commands.CadastrarCesta
{
    public sealed class CadastrarCestaCommandHandler(
        ICestaRecomendacaoRepository _cestaRepository,
        IAcaoRepository _acaoRepository) : IRequestHandler<CadastrarCestaCommand, CestaResponse>
    {
        public async Task<CestaResponse> Handle(CadastrarCestaCommand command, CancellationToken cancellationToken)
        {
            foreach (var item in command.Cesta.Itens)
            {
                var acao = await _acaoRepository.ObterPorIdAsync(item.AcaoId)
                    ?? throw new DomainException($"Ação com ID {item.AcaoId} não encontrada.");

                if (!acao.Ativo)
                    throw new DomainException($"Ação {acao.Codigo} está desativada.");
            }

            var cestaAtiva = await _cestaRepository.ObterAtivaComItensAsync();
            cestaAtiva?.Desativar();

            if (cestaAtiva is not null)
                await _cestaRepository.AtualizarAsync(cestaAtiva);

            var novaCesta = new CestaRecomendacao(DateTime.UtcNow);

            foreach (var item in command.Cesta.Itens)
            {
                novaCesta.AdicionarItem(item.AcaoId, item.Percentual);
            }

            novaCesta.Validar();

            await _cestaRepository.AdicionarAsync(novaCesta);

            var itensResponse = new List<CestaItemResponse>();
            foreach (var item in novaCesta.Itens)
            {
                var acao = await _acaoRepository.ObterPorIdAsync(item.AcaoId);
                itensResponse.Add(new CestaItemResponse(
                    item.AcaoId,
                    acao!.Codigo,
                    acao.NomeEmpresa,
                    item.Percentual
                ));
            }

            return new CestaResponse(
                novaCesta.Id,
                novaCesta.Ativa,
                novaCesta.DataVigencia,
                novaCesta.DataCriacao,
                itensResponse
            );
        }
    }
}
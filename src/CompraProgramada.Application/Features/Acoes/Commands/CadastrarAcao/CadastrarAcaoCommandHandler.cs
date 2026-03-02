using CompraProgramada.Domain.Entities;
using CompraProgramada.Domain.Interfaces.Repositories;
using MediatR;

namespace CompraProgramada.Application.Features.Acoes.Commands.CadastrarAcao
{
    public sealed class CadastrarAcaoCommandHandler(IAcaoRepository _acaoRepository) : IRequestHandler<CadastrarAcaoCommand, int>
    {
        public async Task<int> Handle(CadastrarAcaoCommand command, CancellationToken cancellationToken)
        {
            var acao = new Acao(command.acao.Codigo, command.acao.NomeEmpresa, command.acao.Preco);

            await _acaoRepository.AdicionarAsync(acao);

            return acao.Id;
        }
    }
}

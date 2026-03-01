using CompraProgramada.Domain.DTOs.Acoes;
using MediatR;

namespace CompraProgramada.Application.Features.Acoes.Commands.CadastrarAcao
{
    public sealed record CadastrarAcaoCommand(CadastrarAcaoDTO acao) : IRequest<int>;
}

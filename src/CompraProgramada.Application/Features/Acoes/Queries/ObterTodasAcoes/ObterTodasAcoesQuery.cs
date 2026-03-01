using CompraProgramada.Domain.DTOs.Acoes;
using MediatR;
namespace CompraProgramada.Application.Features.Acoes.Queries.ObterTodasAcoes
{
    public record ObterTodasAcoesQuery(bool? ativo) : IRequest<IEnumerable<ObterAcaoDTO>>;
}

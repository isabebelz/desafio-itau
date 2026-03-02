using AutoMapper;
using CompraProgramada.Domain.DTOs.Acoes;
using CompraProgramada.Domain.Interfaces.Repositories;
using MediatR;

namespace CompraProgramada.Application.Features.Acoes.Queries.ObterTodasAcoes
{
    public sealed class ObterTodasAcoesQueryHandler(IAcaoRepository _acaoRepository,
                                                    IMapper _mapper) : IRequestHandler<ObterTodasAcoesQuery, IEnumerable<ObterAcaoDTO>>
    {

        public async Task<IEnumerable<ObterAcaoDTO>> Handle(ObterTodasAcoesQuery query, CancellationToken cancellationToken)
        {
            var acoes = await _acaoRepository.ObterTodasAsync(query.ativo);
            return _mapper.Map<IEnumerable<ObterAcaoDTO>>(acoes);
        }
    }
}

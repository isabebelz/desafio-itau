using AutoMapper;
using CompraProgramada.Domain.DTOs.Acoes;
using CompraProgramada.Domain.Entities;

namespace CompraProgramada.Application.Common.Mappings
{
    public class AcaoMapping : Profile
    {
        public AcaoMapping()
        {
            CreateMap<Acao, ObterAcaoDTO>();
        }
    }
}

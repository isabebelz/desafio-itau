using AutoMapper;
using CompraProgramada.Domain.DTOs.Clientes;
using CompraProgramada.Domain.Entities.ClienteAggregate;

namespace CompraProgramada.Application.Common.Mappings
{
    public class ClienteMapping : Profile
    {
        public ClienteMapping()
        {
            CreateMap<Cliente, ClienteResponse>();
        }
    }
}
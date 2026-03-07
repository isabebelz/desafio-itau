using CompraProgramada.Domain.DTOs.Events;

namespace CompraProgramada.Application.Events
{
    public interface IKafkaProducer
    {
        Task PublicarDistribuicaoIrDedoDuroAsync(DistribuicaoIrDedoDuroKafkaEvent evento);
    }
}

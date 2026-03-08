using System.Text.Json;
using CompraProgramada.Application.Events;
using CompraProgramada.Domain.DTOs.Events;
using Confluent.Kafka;

namespace CompraProgramada.Infra.Data.Messaging
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;

        public KafkaProducer(IProducer<Null, string> producer, string topic)
        {
            _producer = producer;
            _topic = topic;
        }

        public async Task PublicarDistribuicaoIrDedoDuroAsync(DistribuicaoIrDedoDuroKafkaEvent evento)
        {
            var json = JsonSerializer.Serialize(evento);
            var message = new Message<Null, string> { Value = json };
            await _producer.ProduceAsync(_topic, message);
        }
    }
}
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Tutorium.AuthService.Core.Models.Kafka;

namespace Tutorium.AuthService.Core.Services
{
    public class KafkaProducerService
    {
        private readonly IProducer<string, string> _producer;
        private readonly KafkaOptions _kafka;

        public KafkaProducerService(IOptions<KafkaOptions> kafka)
        {
            _kafka = kafka.Value;

            var config = new ProducerConfig() { BootstrapServers = _kafka.Bootstrap };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }
            
        public async Task SendMessageAsync(string topic, string key, string message)
        {
            var msg = new Message<string, string> { Key = key, Value = message };
            var deliveryResult = await _producer.ProduceAsync(topic, msg);
            Console.WriteLine($"Message delivered to {deliveryResult.TopicPartitionOffset}");
        }
    }
}

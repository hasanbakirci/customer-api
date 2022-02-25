using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace CustomerService.Clients.MessageQueueClients
{
    public class KafkaClient
    {
        private readonly IProducer<string, string> _producer;

        public KafkaClient()
        {
            var config = new ProducerConfig() {BootstrapServers = "localhost:9092", Acks = Acks.Leader};
            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task Publish<T>(string topicName,T message)
        {
            var dr = await _producer.ProduceAsync(topicName, new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = JsonConvert.SerializeObject(message)
            });
            _producer.Flush(TimeSpan.FromMilliseconds(5));
        }
    }
    
    
}
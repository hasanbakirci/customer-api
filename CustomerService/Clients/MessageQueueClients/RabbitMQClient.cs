using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace CustomerService.Clients.MessageQueueClients
{
    public class RabbitMQClient : IMessageQueueClient
    {
        private readonly IConfiguration _configuration;
        private readonly RabbitMQSettings _rabbitMQSettings;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        
        public RabbitMQClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _rabbitMQSettings = _configuration.GetSection("RabbitMQSettings").Get<RabbitMQSettings>();
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMQSettings.Hostname,
                UserName = _rabbitMQSettings.Username,
                Password = _rabbitMQSettings.Password
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(RabbitMQHelper.CustomerExchange,ExchangeType.Direct,true);
            _channel.QueueDeclare(RabbitMQHelper.CreatedQueue, false, false, false, null);
            _channel.QueueDeclare(RabbitMQHelper.UpdatedQueue, false, false, false, null);
            _channel.QueueBind(RabbitMQHelper.CreatedQueue,RabbitMQHelper.CustomerExchange,RabbitMQHelper.CreatedQueue);
            _channel.QueueBind(RabbitMQHelper.UpdatedQueue,RabbitMQHelper.CustomerExchange,RabbitMQHelper.UpdatedQueue);
        }
        
        public void Publish<T>(string queueName, T message)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            _channel.BasicPublish(RabbitMQHelper.CustomerExchange,queueName,null,body);
        }
    }
}
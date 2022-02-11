namespace CustomerService.Clients.MessageQueueClients
{
    public interface IMessageQueueClient
    {
        void Publish<T>(string queueName, T message);
    }
}
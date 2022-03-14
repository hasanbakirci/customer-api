namespace CustomerService.Clients.MessageQueueClients
{
    public static class RabbitMQHelper
    {
        public static string CreatedQueue => "Created-Customer-Queue";
        public static string UpdatedQueue => "Updated-Customer-Queue";
        public static string DeletedQueue => "Deleted-Customer-Queue";
        public static string CustomerExchange => "Customer-Exchange";
    }
}
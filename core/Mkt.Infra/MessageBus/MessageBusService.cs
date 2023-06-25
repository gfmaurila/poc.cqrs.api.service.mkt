using Mkt.Core.Services;
using RabbitMQ.Client;

namespace Mkt.Infra.MessageBus;
public class MessageBusService : IMessageBusService
{
    private readonly ConnectionFactory _connectionFactory;

    public MessageBusService()
    {
        _connectionFactory = new ConnectionFactory
        {
            HostName = "localhost",
        };
    }
    public void Publish(string queue, byte[] message)
    {
        using (var connection = _connectionFactory.CreateConnection())
        {
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: queue,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                channel.BasicPublish(
                    exchange: "",
                    routingKey: queue,
                    basicProperties: null,
                    body: message);
            }
        }
    }
}

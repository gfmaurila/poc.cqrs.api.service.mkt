using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mkt.Core.DTOs;
using Mkt.Core.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Mkt.App.Consumers.User;

public class SendGridGenerateCodeResetConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IServiceProvider _serviceProvider;
    private const string QUEUE_NAME = "SendGrid_GenerateCodeReset_User";

    public SendGridGenerateCodeResetConsumer(IServiceProvider servicesProvider)
    {
        _serviceProvider = servicesProvider;

        var factory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(
            queue: QUEUE_NAME,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (sender, eventArgs) =>
        {
            var infoSendGridBytes = eventArgs.Body.ToArray();
            var infoSendGridJson = Encoding.UTF8.GetString(infoSendGridBytes);
            var infoSendGrid = JsonSerializer.Deserialize<SendGridDto>(infoSendGridJson);
            await SendGrid(infoSendGrid);
            _channel.BasicAck(eventArgs.DeliveryTag, false);
        };
        _channel.BasicConsume(QUEUE_NAME, false, consumer);
        return Task.CompletedTask;
    }

    public async Task SendGrid(SendGridDto dto)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var sendGridService = scope.ServiceProvider.GetRequiredService<ISendGridEmailService>();
            await sendGridService.SendEmailAsync(dto);
        }
    }
}

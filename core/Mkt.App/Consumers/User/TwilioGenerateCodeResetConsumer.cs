using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mkt.Core.DTOs;
using Mkt.Core.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Mkt.App.Consumers.User;

public class TwilioGenerateCodeResetConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IServiceProvider _serviceProvider;
    private const string QUEUE_NAME = "Twilio_GenerateCodeReset_User";

    public TwilioGenerateCodeResetConsumer(IServiceProvider servicesProvider)
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
            var infoTwilioBytes = eventArgs.Body.ToArray();
            var infoTwilioJson = Encoding.UTF8.GetString(infoTwilioBytes);
            var infoTwilio = JsonSerializer.Deserialize<TwilioDto>(infoTwilioJson);
            await SendGrid(infoTwilio);
            _channel.BasicAck(eventArgs.DeliveryTag, false);
        };
        _channel.BasicConsume(QUEUE_NAME, false, consumer);
        return Task.CompletedTask;
    }

    public async Task SendGrid(TwilioDto dto)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var send = scope.ServiceProvider.GetRequiredService<ITwilioWhatsAppService>();
            await send.SendTwilioWhatsAppAsync(dto);
        }
    }
}

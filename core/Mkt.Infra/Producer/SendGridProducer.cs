using Microsoft.Extensions.Configuration;
using Mkt.Core.DTOs;
using Mkt.Core.Producer;
using Mkt.Core.Services;
using System.Text;
using System.Text.Json;

namespace Mkt.Infra.Producer;

public class SendGridProducer : ISendGridProducer
{
    private readonly IConfiguration _configuration;
    private readonly IMessageBusService _messageBusService;
    private const string QUEUE_NAME = "SendGrid_GenerateCodeReset_User";
    public SendGridProducer(IConfiguration configuration, IMessageBusService messageBusService)
    {
        _configuration = configuration;
        _messageBusService = messageBusService;
    }

    public void Publish(SendGridDto dto)
    {
        var infoSendGrid = new SendGridDto(dto.PlainTextContent, dto.Subject, dto.HtmlContent, dto.To, dto.Name);
        var infoSendGridJson = JsonSerializer.Serialize(infoSendGrid);
        var infoSendGridBytes = Encoding.UTF8.GetBytes(infoSendGridJson);
        _messageBusService.Publish(QUEUE_NAME, infoSendGridBytes);
    }
}

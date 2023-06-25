using Microsoft.Extensions.Configuration;
using Mkt.Core.DTOs;
using Mkt.Core.Producer;
using Mkt.Core.Services;
using System.Text;
using System.Text.Json;

namespace Mkt.Infra.Producer;

public class TwilioWhatsAppProducer : ITwilioWhatsAppProducer
{
    private readonly IConfiguration _configuration;
    private readonly IMessageBusService _messageBusService;
    private const string QUEUE_NAME = "Twilio_GenerateCodeReset_User";
    public TwilioWhatsAppProducer(IConfiguration configuration, IMessageBusService messageBusService)
    {
        _configuration = configuration;
        _messageBusService = messageBusService;
    }

    public void Publish(TwilioDto dto)
    {
        var info = new TwilioDto(dto.Body, dto.To);
        var infoJson = JsonSerializer.Serialize(info);
        var infoBytes = Encoding.UTF8.GetBytes(infoJson);
        _messageBusService.Publish(QUEUE_NAME, infoBytes);
    }
}

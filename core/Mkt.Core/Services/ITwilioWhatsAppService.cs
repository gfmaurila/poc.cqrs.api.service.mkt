using Mkt.Core.DTOs;

namespace Mkt.Core.Services;
public interface ITwilioWhatsAppService
{
    Task SendTwilioWhatsAppAsync(TwilioDto dto);
}

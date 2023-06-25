using Mkt.Core.DTOs;

namespace Mkt.Core.Producer;

public interface ITwilioWhatsAppProducer
{
    void Publish(TwilioDto dto);
}

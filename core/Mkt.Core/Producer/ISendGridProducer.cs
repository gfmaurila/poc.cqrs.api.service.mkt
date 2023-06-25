using Mkt.Core.DTOs;

namespace Mkt.Core.Producer;

public interface ISendGridProducer
{
    void Publish(SendGridDto dto);
}

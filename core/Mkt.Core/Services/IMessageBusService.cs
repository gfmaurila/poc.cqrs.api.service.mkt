namespace Mkt.Core.Services;
public interface IMessageBusService
{
    void Publish(string queue, byte[] message);
}

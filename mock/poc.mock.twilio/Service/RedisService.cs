using Newtonsoft.Json;
using poc.mock.twilio.Model;
using StackExchange.Redis;

namespace poc.mock.twilio.Service;
public interface IRedisService
{
    Task SetAsync(TwilioWhatsApp data);
}

public class RedisService : IRedisService
{
    private readonly IDatabase _database;
    private readonly IConnectionMultiplexer _multiplexer;

    public RedisService(IConnectionMultiplexer multiplexer)
    {
        _multiplexer = multiplexer;
        _database = multiplexer.GetDatabase();
    }

    public async Task SetAsync(TwilioWhatsApp data)
    {
        var jsonData = JsonConvert.SerializeObject(data);
        await _database.StringSetAsync(data.Id, jsonData);
    }
}


using Microsoft.AspNetCore.Mvc;
using poc.mock.twilio.Model;
using poc.mock.twilio.Service;

namespace poc.mock.twilio.Controllers;
[ApiController]
[Route("[controller]")]
public class TwilioWhatsAppController : ControllerBase
{
    private readonly IRedisService _redisService;

    public TwilioWhatsAppController(IRedisService redisService)
    {
        _redisService = redisService;
    }

    [HttpPost]
    public async Task<IActionResult> Post(TwilioWhatsApp data)
    {
        await _redisService.SetAsync(data);
        return Ok();
    }
}


namespace Mkt.Core.DTOs;
public class TwilioDto
{
    public TwilioDto(string body, string to)
    {
        To = to;
        Body = body;
    }

    public string Body { get; set; }
    public string To { get; set; }
}
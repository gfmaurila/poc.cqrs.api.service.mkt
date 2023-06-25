namespace Mkt.Core.DTOs;
public class TwilioWhatsAppDto
{
    public string AccountSid { get; set; }
    public string AuthToken { get; set; }
    public string From { get; set; }
    public string Body { get; set; }
    public string To { get; set; }
}
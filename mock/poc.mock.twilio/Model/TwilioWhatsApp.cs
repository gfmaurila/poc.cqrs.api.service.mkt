namespace poc.mock.twilio.Model;
public class TwilioWhatsApp
{
    public TwilioWhatsApp()
    {
        Id = Guid.NewGuid().ToString();
    }

    public string Id { get; set; }
    public string AccountSid { get; set; }
    public string AuthToken { get; set; }
    public string From { get; set; }
    public string Body { get; set; }
    public string To { get; set; }
}

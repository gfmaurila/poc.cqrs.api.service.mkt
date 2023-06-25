namespace Mkt.Core.DTOs;
public class SendGridEmailDto
{
    public string Key { get; set; }
    public string From { get; set; }
    public string User { get; set; }
    public string PlainTextContent { get; set; }
    public string Subject { get; set; }
    public string HtmlContent { get; set; }
    public string To { get; set; }
    public string Name { get; set; }
}
namespace Mkt.Core.DTOs;
public class SendGridDto
{
    public SendGridDto(string plainTextContent, string subject, string htmlContent, string to, string name)
    {
        PlainTextContent = plainTextContent;
        Subject = subject;
        HtmlContent = htmlContent;
        To = to;
        Name = name;
    }

    public string PlainTextContent { get; set; }
    public string Subject { get; set; }
    public string HtmlContent { get; set; }
    public string To { get; set; }
    public string Name { get; set; }
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace poc.mock.sendgrid.Model;
public class SendGridEmail
{
    [BsonId]
    public string Id { get; set; }
    // Restante das propriedades...

    public SendGridEmail()
    {
        Id = ObjectId.GenerateNewId().ToString();
    }

    public string Key { get; set; }
    public string From { get; set; }
    public string User { get; set; }

    public string PlainTextContent { get; set; }
    public string Subject { get; set; }
    public string HtmlContent { get; set; }
    public string To { get; set; }
    public string Name { get; set; }
}


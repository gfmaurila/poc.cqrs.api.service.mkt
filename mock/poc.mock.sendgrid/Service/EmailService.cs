using MongoDB.Driver;
using poc.mock.sendgrid.Model;

namespace poc.mock.sendgrid.Service;

public interface IEmailService
{
    Task Create(SendGridEmail email);
}

public class EmailService : IEmailService
{
    private readonly IMongoCollection<SendGridEmail> _emails;

    public EmailService(MongoDatabaseFactory dbFactory)
    {
        _emails = dbFactory.GetDatabase().GetCollection<SendGridEmail>("SendGridEmails");
    }

    public async Task Create(SendGridEmail email)
    {
        await _emails.InsertOneAsync(email);
    }
}

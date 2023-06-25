using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mkt.Core.DTOs;
using Mkt.Core.Services;
using Polly;
using System.Net;
using System.Net.Http.Json;

namespace Mkt.Infra.Services;

public class SendGridEmailService : ISendGridEmailService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SendGridEmailService> _logger;
    private readonly IConfiguration _configuration;
    private readonly AsyncPolicy<HttpResponseMessage> _retryPolicy;

    public SendGridEmailService(HttpClient httpClient, ILogger<SendGridEmailService> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;

        // Configuração da política de tentativas de retry
        _retryPolicy = Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (ex, retryCount, context) =>
                {
                    // Lógica a ser executada a cada tentativa de retry
                    _logger.LogWarning($"Tentativa {retryCount} de envio de e-mail...");
                }
            );
    }

    public async Task SendEmailAsync(SendGridDto dto)
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            var response = await _httpClient.PostAsJsonAsync(
                _configuration["SendGrid:URL"],
                new SendGridEmailDto
                {
                    From = _configuration["SendGrid:From"],
                    User = _configuration["SendGrid:User"],
                    Key = _configuration["SendGrid:Key"],
                    To = dto.To,
                    PlainTextContent = dto.PlainTextContent,
                    HtmlContent = dto.HtmlContent,
                    Name = dto.Name,
                    Subject = dto.Subject,
                }
            );
            response.EnsureSuccessStatusCode();

            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Accepted)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Falha ao enviar e-mail: {error}");
            }

            return response;
        });
    }
}


using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mkt.Core.DTOs;
using Mkt.Core.Services;
using Polly;
using System.Net;
using System.Net.Http.Json;

namespace Mkt.Infra.Services;

public class TwilioWhatsAppService : ITwilioWhatsAppService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TwilioWhatsAppService> _logger;
    private readonly IConfiguration _configuration;
    private readonly AsyncPolicy<HttpResponseMessage> _resiliencePolicy;

    public TwilioWhatsAppService(HttpClient httpClient, ILogger<TwilioWhatsAppService> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;

        // Configuração das políticas de resiliência
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (ex, retryCount, context) =>
                {
                    // Lógica a ser executada a cada tentativa de retry
                    _logger.LogWarning($"Tentativa {retryCount} de envio de mensagem WhatsApp...");
                }
            );

        var circuitBreakerPolicy = Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(10),
                onBreak: (ex, breakDelay) =>
                {
                    // Lógica a ser executada quando o circuit breaker é acionado
                    _logger.LogWarning($"Circuit breaker aberto. Aguardando {breakDelay.TotalSeconds} segundos...");
                },
                onReset: () =>
                {
                    // Lógica a ser executada quando o circuit breaker é rearmado
                    _logger.LogWarning("Circuit breaker rearmado.");
                }
            );

        _resiliencePolicy = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
    }

    public async Task SendTwilioWhatsAppAsync(TwilioDto dto)
    {
        await _resiliencePolicy.ExecuteAsync(async () =>
        {
            var response = await _httpClient.PostAsJsonAsync(
                _configuration["Twilio:URL"],
                new TwilioWhatsAppDto
                {
                    From = _configuration["Twilio:From"],
                    AccountSid = _configuration["Twilio:AccountSid"],
                    AuthToken = _configuration["Twilio:AuthToken"],
                    Body = dto.Body,
                    To = dto.To,
                }
            );
            response.EnsureSuccessStatusCode();

            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Accepted)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Falha ao enviar WhatsApp: {error}");
            }

            return response; // Retornar a resposta da chamada HTTP
        });
    }
}


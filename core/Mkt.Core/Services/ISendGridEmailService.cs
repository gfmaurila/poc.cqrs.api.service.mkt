using Mkt.Core.DTOs;

namespace Mkt.Core.Services;
public interface ISendGridEmailService
{
    Task SendEmailAsync(SendGridDto dto);
}

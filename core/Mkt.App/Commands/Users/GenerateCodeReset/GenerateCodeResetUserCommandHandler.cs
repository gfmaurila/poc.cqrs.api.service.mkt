using MediatR;
using Mkt.Core.DTOs;
using Mkt.Core.Entities;
using Mkt.Core.Enums;
using Mkt.Core.Exceptions;
using Mkt.Core.Producer;
using Mkt.Core.Repositories;
using System.Text;

namespace Mkt.App.Commands.Users.GenerateCodeReset;
public class GenerateCodeResetUserCommandHandler : IRequestHandler<GenerateCodeResetUserCommand, int>
{
    private readonly IUserRepository _userRepository;
    private readonly ISendGridProducer _producerSendGrid;
    private readonly ITwilioWhatsAppProducer _producerTwilio;
    public GenerateCodeResetUserCommandHandler(ISendGridProducer producerSendGrid, ITwilioWhatsAppProducer producerTwilio, IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _producerSendGrid = producerSendGrid;
        _producerTwilio = producerTwilio;
    }

    public async Task<int> Handle(GenerateCodeResetUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
            throw new UserNotFoundException("Usuário não encontrado.");

        user.UpdateGenerateCodeReset(GenerateRandomNumber(20, 100).ToString());
        await _userRepository.UpdateUserAsync(user);

        switch (request.ETypeSend)
        {
            case ETypeSend.WhatsApp:
                _producerTwilio.Publish(new TwilioDto(CreateWhatsAPpBody(user), user.Phone));
                break;

            case ETypeSend.Email:
                _producerSendGrid.Publish(new SendGridDto(CreateEmailBody(user), "Solicitação de reset de senha - MKT", CreateEmailBody(user), user.Email, user.FullName));
                break;

            default:
                throw new NotImplementedException("Tipo de envio não suportado.");
        }
        return user.Id;
    }

    #region Private
    public int GenerateRandomNumber(int minValue, int maxValue)
    {
        Random random = new Random();
        return random.Next(minValue, maxValue + 1);
    }

    private string CreateEmailBody(User user)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<html>");
        sb.AppendLine("<body>");
        sb.AppendLine($"<h1>Olá, {user.FullName} dados para alteração </h1>");
        sb.AppendLine($"<div>Link: .... </div>");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");
        return sb.ToString();
    }

    private string CreateWhatsAPpBody(User user)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Olá, {user.FullName} dados para alteração");
        return sb.ToString();
    }
    #endregion Private
}
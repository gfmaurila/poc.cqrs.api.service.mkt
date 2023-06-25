using MediatR;
using Mkt.Core.Repositories;
using Mkt.Core.Services;

namespace Mkt.App.Commands.Users.UpdateUser;
public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, int>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;
    private readonly IMessageBusService _messageBusService;
    public UpdateUserCommandHandler(IMessageBusService messageBusService, IUserRepository userRepository, IAuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
        _messageBusService = messageBusService;
    }

    public async Task<int> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        user.Update(request.FullName, request.Email, request.Phone, request.BirthDate, request.Role);
        await _userRepository.UpdateUserAsync(user);
        //await Publish(request);
        return user.Id;
    }


    #region Private
    //private async Task Publish(UpdateUserCommand request)
    //{
    //    var infoSendGrid = new SendGridDto(CreateEmailBody(request), "Alteração de dados - MKT", CreateEmailBody(request), request.Email, request.FullName);
    //    var infoSendGridJson = JsonSerializer.Serialize(infoSendGrid);
    //    var infoSendGridBytes = Encoding.UTF8.GetBytes(infoSendGridJson);
    //    _messageBusService.Publish("SendGrid_Update_User", infoSendGridBytes);
    //}

    //private string CreateEmailBody(UpdateUserCommand request)
    //{
    //    StringBuilder sb = new StringBuilder();
    //    sb.AppendLine("<html>");
    //    sb.AppendLine("<body>");
    //    sb.AppendLine($"<h1>Olá, {request.FullName} Dados alterados </h1>");
    //    sb.AppendLine($"<div>Usuário: {request.Email} </div>");
    //    sb.AppendLine("</body>");
    //    sb.AppendLine("</html>");
    //    return sb.ToString();
    //}
    #endregion Private
}
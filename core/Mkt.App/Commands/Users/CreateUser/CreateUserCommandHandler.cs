using MediatR;
using Mkt.Core.Entities;
using Mkt.Core.Repositories;
using Mkt.Core.Services;

namespace Mkt.App.Commands.Users.CreateUser;
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;
    private readonly IMessageBusService _messageBusService;
    public CreateUserCommandHandler(IMessageBusService messageBusService, IUserRepository userRepository, IAuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
        _messageBusService = messageBusService;
    }

    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var passwordHash = _authService.ComputeSha256Hash(request.Password);
        var user = new User(request.FullName, request.Email, request.Phone, request.BirthDate, passwordHash, request.Role, "-");
        await _userRepository.AddUserAsync(user);
        //await Publish(request);
        return user.Id;
    }

    //#region Private
    //private async Task Publish(CreateUserCommand request)
    //{
    //    var infoSendGrid = new SendGridDto(CreateEmailBody(request), "Cadastro - MKT", CreateEmailBody(request), request.Email, request.FullName);
    //    var infoSendGridJson = JsonSerializer.Serialize(infoSendGrid);
    //    var infoSendGridBytes = Encoding.UTF8.GetBytes(infoSendGridJson);
    //    _messageBusService.Publish("SendGrid_Create_User", infoSendGridBytes);
    //}

    //private string CreateEmailBody(CreateUserCommand request)
    //{
    //    StringBuilder sb = new StringBuilder();
    //    sb.AppendLine("<html>");
    //    sb.AppendLine("<body>");
    //    sb.AppendLine($"<h1>Olá, {request.FullName} seja bem vindo </h1>");
    //    sb.AppendLine($"<div>Usuário: {request.Email} </div>");
    //    sb.AppendLine($"<div>Senha: {request.Password} </div>");
    //    sb.AppendLine("</body>");
    //    sb.AppendLine("</html>");
    //    return sb.ToString();
    //}
    //#endregion Private

}

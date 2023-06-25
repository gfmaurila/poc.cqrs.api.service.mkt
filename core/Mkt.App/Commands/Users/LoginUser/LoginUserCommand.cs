using MediatR;
using Mkt.App.ViewModels.Users;

namespace Mkt.App.Commands.Users.LoginUser;

public class LoginUserCommand : IRequest<LoginUserViewModel>
{
    public string Email { get; set; }
    public string Password { get; set; }
}


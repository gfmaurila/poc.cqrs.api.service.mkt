using MediatR;
using Mkt.Core.Enums;

namespace Mkt.App.Commands.Users.GenerateCodeReset;

public class GenerateCodeResetUserCommand : IRequest<int>
{
    public string Email { get; set; }
    public ETypeSend ETypeSend { get; set; }
}


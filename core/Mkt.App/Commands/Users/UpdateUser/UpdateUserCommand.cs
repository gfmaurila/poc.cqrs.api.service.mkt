using MediatR;

namespace Mkt.App.Commands.Users.UpdateUser;

public class UpdateUserCommand : IRequest<int>
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime BirthDate { get; set; }
    public string Role { get; set; }
}


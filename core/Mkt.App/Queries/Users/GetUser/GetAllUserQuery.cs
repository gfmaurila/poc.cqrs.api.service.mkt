using MediatR;
using Mkt.App.ViewModels.Users;

namespace Mkt.App.Queries.Users.GetUser;
public class GetAllUserQuery : IRequest<List<UserViewModel>>
{
    public GetAllUserQuery()
    {
    }

    public GetAllUserQuery(int id, string userName, string email, string phone)
    {
        Id = id;
        FullName = userName;
        Email = email;
        Phone = phone;
    }

    public int Id { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}
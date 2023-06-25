using MediatR;
using Mkt.App.ViewModels.Users;

namespace Mkt.App.Queries.Users.GetUser;
public class GetUserQuery : IRequest<UserViewModel>
{
    public GetUserQuery()
    {
    }

    public GetUserQuery(int id)
    {
        Id = id;
    }
    public int Id { get; set; }
}
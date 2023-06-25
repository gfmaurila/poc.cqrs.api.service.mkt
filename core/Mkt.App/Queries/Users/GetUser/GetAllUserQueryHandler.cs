using MediatR;
using Mkt.App.ViewModels.Users;
using Mkt.Core.Repositories;

namespace Mkt.App.Queries.Users.GetUser;

public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, List<UserViewModel>>
{
    private readonly IUserRepository _repo;
    public GetAllUserQueryHandler(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<UserViewModel>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        var all = await _repo.GetAllAsync();

        var viewModel = all.Select(p => new UserViewModel(p.Id, p.FullName, p.Email, p.Phone)).ToList();

        return viewModel;
    }
}
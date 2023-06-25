using MediatR;
using Mkt.App.ViewModels.Users;
using Mkt.Core.Repositories;

namespace Mkt.App.Queries.Users.GetUser;
public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserViewModel>
{
    private readonly IUserRepository _userRepository;
    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserViewModel> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);

        if (user == null)
        {
            return null;
        }

        return new UserViewModel(user.FullName, user.Email);
    }
}
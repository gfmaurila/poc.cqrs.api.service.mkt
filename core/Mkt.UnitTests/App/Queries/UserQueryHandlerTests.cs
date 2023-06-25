using Mkt.App.Queries.Users.GetUser;
using Mkt.App.ViewModels.Users;
using Mkt.Core.Entities;
using Mkt.Core.Repositories;
using Moq;
using Xunit;

namespace Mkt.UnitTests.App.Queries;
public class UserQueryHandlerTests
{
    [Fact]
    public async Task GetByIdAsync()
    {
        // Arrange
        int userId = 1;
        var request = new GetUserQuery { Id = userId };

        var user = new User
        {
            Id = userId
        };

        var userRepositoryMock = new Mock<IUserRepository>();
        userRepositoryMock.Setup(repo => repo.GetByIdAsync(request.Id)).ReturnsAsync(user);

        var handler = new GetUserQueryHandler(userRepositoryMock.Object);

        // Act
        UserViewModel result = await handler.Handle(request, CancellationToken.None);

        // Assert
        userRepositoryMock.Verify(repo => repo.GetByIdAsync(request.Id), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(user.FullName, result.FullName);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.Phone, result.Phone);
    }


    [Fact]
    public async Task Handle_GetAllUserQuery_ReturnsListOfUserViewModels()
    {
        // Arrange
        var users = new List<User>
        {
            new User ("fulano de tal 1", "fulanodetal1@teste.com.br", "5551985623312", new DateTime(1986, 03, 18), "@D19e03m1963", "admin", "-"),
            new User ("fulano de tal 2", "fulanodetal2@teste.com.br", "5551985623312", new DateTime(1986, 03, 18), "@D19e03m1963", "admin", "-"),
        };

        var userRepositoryMock = new Mock<IUserRepository>();
        userRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);

        var handler = new GetAllUserQueryHandler(userRepositoryMock.Object);

        // Act
        List<UserViewModel> result = await handler.Handle(new GetAllUserQuery(), CancellationToken.None);

        // Assert
        userRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(users.Count, result.Count);

        for (int i = 0; i < users.Count; i++)
        {
            Assert.Equal(users[i].Id, result[i].Id);
            Assert.Equal(users[i].FullName, result[i].FullName);
            Assert.Equal(users[i].Email, result[i].Email);
            Assert.Equal(users[i].Phone, result[i].Phone);
        }
    }

}

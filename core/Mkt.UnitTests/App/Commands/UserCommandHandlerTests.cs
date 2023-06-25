using Mkt.App.Commands.Users.CreateUser;
using Mkt.App.Commands.Users.UpdateUser;
using Mkt.Core.Entities;
using Mkt.Core.Repositories;
using Mkt.Core.Services;
using Moq;
using Xunit;

namespace Mkt.UnitTests.App.Commands;
public class UserCommandHandlerTests
{
    [Fact]
    public async Task CreateUser()
    {
        var repoMock = new Mock<IUserRepository>();
        var busMock = new Mock<IMessageBusService>();
        var serviceMock = new Mock<IAuthService>();

        var command = new CreateUserCommand
        {
            FullName = "fulano de tal",
            Email = "fulanodetal@teste.com.br",
            Phone = "5551985623312",
            Password = "@D19e03m1963",
            BirthDate = new DateTime(1986, 03, 18),
            Role = "admin"
        };

        var commandHandler = new CreateUserCommandHandler(busMock.Object, repoMock.Object, serviceMock.Object);

        // Act
        var id = await commandHandler.Handle(command, new CancellationToken());

        // Assert
        Assert.True(id >= 0);

    }

    [Fact]
    public async Task UpdateUser()
    {
        // Arrange
        int expectedUserId = 1;
        var request = new UpdateUserCommand
        {
            Id = expectedUserId,
            FullName = "John Doe",
            Email = "john.doe@example.com",
            Phone = "5551985623312",
            BirthDate = new DateTime(1990, 1, 1),
            Role = "Admin"
        };

        var userRepositoryMock = new Mock<IUserRepository>();

        userRepositoryMock.Setup(repo => repo.GetByIdAsync(request.Id)).ReturnsAsync(new User { Id = request.Id });
        userRepositoryMock.Setup(repo => repo.UpdateUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

        var authServiceMock = new Mock<IAuthService>();

        var messageBusServiceMock = new Mock<IMessageBusService>();

        var handler = new UpdateUserCommandHandler(messageBusServiceMock.Object, userRepositoryMock.Object, authServiceMock.Object);

        // Act
        int result = await handler.Handle(request, CancellationToken.None);

        // Assert
        userRepositoryMock.Verify(repo => repo.GetByIdAsync(request.Id), Times.Once);
        userRepositoryMock.Verify(repo => repo.UpdateUserAsync(It.IsAny<User>()), Times.Once);

        Assert.Equal(expectedUserId, result);
    }
}

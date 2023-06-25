using FluentAssertions;
using Mkt.App.Commands.Users.GenerateCodeReset;
using Mkt.Core.DTOs;
using Mkt.Core.Entities;
using Mkt.Core.Enums;
using Mkt.Core.Exceptions;
using Mkt.Core.Producer;
using Mkt.Core.Repositories;
using Moq;
using Xunit;

namespace Mkt.UnitTests.App.Commands;

public class GenerateCodeResetUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ISendGridProducer> _sendGridProducerMock;
    private readonly Mock<ITwilioWhatsAppProducer> _twilioWhatsAppProducerMock;

    private readonly GenerateCodeResetUserCommandHandler _commandHandler;

    public GenerateCodeResetUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _sendGridProducerMock = new Mock<ISendGridProducer>();
        _twilioWhatsAppProducerMock = new Mock<ITwilioWhatsAppProducer>();

        _commandHandler = new GenerateCodeResetUserCommandHandler(
            _sendGridProducerMock.Object,
            _twilioWhatsAppProducerMock.Object,
            _userRepositoryMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidRequest_ReturnsUserId()
    {
        // Arrange
        var email = "test@example.com";
        var user = new User("fulano de tal 1", "fulanodetal1@teste.com.br", "5551985623312", new DateTime(1986, 03, 18), "@D19e03m1963", "admin", "code123");

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(email)).ReturnsAsync(user);
        _userRepositoryMock.Setup(x => x.UpdateUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

        var command = new GenerateCodeResetUserCommand { Email = email, ETypeSend = ETypeSend.Email };

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(user.Id);
        _userRepositoryMock.Verify(x => x.GetByEmailAsync(email), Times.Once);
        _userRepositoryMock.Verify(x => x.UpdateUserAsync(It.Is<User>(u => u.Id == user.Id && u.GenerateCodeReset != null)), Times.Once);
        _sendGridProducerMock.Verify(x => x.Publish(It.IsAny<SendGridDto>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithUnknownUser_ThrowsUserNotFoundException()
    {
        // Arrange
        var email = "test@example.com";

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(email)).ReturnsAsync((User)null);

        var command = new GenerateCodeResetUserCommand { Email = email, ETypeSend = ETypeSend.Email };

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() => _commandHandler.Handle(command, CancellationToken.None));
        _userRepositoryMock.Verify(x => x.GetByEmailAsync(email), Times.Once);
        _userRepositoryMock.Verify(x => x.UpdateUserAsync(It.IsAny<User>()), Times.Never);
        _sendGridProducerMock.Verify(x => x.Publish(It.IsAny<SendGridDto>()), Times.Never);
    }

}



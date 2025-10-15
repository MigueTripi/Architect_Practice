using Moq;
using NServiceBus.Testing;
using SelfResearch.Financial.API.Contracts;
using SelfResearch.UserManagement.API.Contracts;
using SelfResearch.UserManagement.API.Features.UserManagement;
using SelfResearch.UserManagement.API.Features.UserManagement.CreateUser;

namespace SelfResearch.UserManagement.API.Test.Core.EventHandlers;

public class CreateUserHandlerTests
{
    private Mock<IUpdateUserService> _updateUserServiceMock = new();

    [Fact]
    public async Task UserCreationSucceedMessage_Handle_PublishesMessage()
    {
        // Arrange
        var handler = GetValidHandler();
        var message = new UserCreationSucceedMessage
        {
            UserId = 10,
            State = 1
        };
        var messageHandleContext = new TestableMessageHandlerContext();

        // Act
        await handler.Handle(message, messageHandleContext);

        // Assert
        Assert.Single(messageHandleContext.PublishedMessages);
        Assert.IsType<UserCreationSucceedMessage>(messageHandleContext.PublishedMessages[0].Message);
    }

    [Fact]
    public async Task WalletCreationSucceedMessage_Handle_UpdatesTheUser()
    {
        // Arrange
        var handler = GetValidHandler();
        var message = new WalletCreationSucceedMessage
        {
            UserId = 10,
            WalletId = 1
        };
        var messageHandleContext = new TestableMessageHandlerContext();

        // Act
        await handler.Handle(message, messageHandleContext);

        // Assert
        _updateUserServiceMock.Verify(x => x.UpdateUserStateAsync(message.UserId, UserStateEnumDto.Active), Times.Once);
    }
    
    private CreateUserHandler GetValidHandler()
    {
        return new CreateUserHandler(_updateUserServiceMock.Object);
    }
}

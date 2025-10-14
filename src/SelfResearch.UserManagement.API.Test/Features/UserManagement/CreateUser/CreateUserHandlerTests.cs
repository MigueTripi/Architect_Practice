using System;
using Moq;
using NServiceBus.Testing;
using SelfResearch.UserManagement.API.Contracts;
using SelfResearch.UserManagement.API.Features.UserManagement.CreateUser;

namespace SelfResearch.UserManagement.API.Test.Features.UserManagement.CreateUser;

public class CreateUserHandlerTests
{
    private Mock<IUpdateUserService> _updateUserServiceMock = new();

    [Fact]
    public async Task Handle_UserCreationSucceedMessage_PublishesMessage()
    {
        // Arrange
        var handler = new CreateUserHandler(_updateUserServiceMock.Object);
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
}

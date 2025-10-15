using System;
using NServiceBus.Testing;
using SelfResearch.Financial.API.Contracts;
using SelfResearch.Financial.API.Core.EventHandlers;

namespace SelfResearch.Financial.API.Test.Core.EventHandlers;

public class WalletCreationEventHandlerTests
{

    [Fact]
    public async Task WalletCreationSucceedMessage_Handle_PublishesMessage()
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
        Assert.Single(messageHandleContext.PublishedMessages);
        Assert.IsType<WalletCreationSucceedMessage>(messageHandleContext.PublishedMessages[0].Message);
    }

    private WalletCreationEventHandler GetValidHandler()
    {
        return new WalletCreationEventHandler();
    }
}

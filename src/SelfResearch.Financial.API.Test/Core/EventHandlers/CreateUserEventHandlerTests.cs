using System;
using Moq;
using NServiceBus.Testing;
using SelfResearch.Financial.API.Core.EventHandlers;
using SelfResearch.Financial.API.Feature.Propagate;
using SelfResearch.Financial.API.Feature.Wallet;
using SelfResearch.Financial.API.Feature.Wallet.CreateWalet;
using SelfResearch.UserManagement.API.Contracts;

namespace SelfResearch.Financial.API.Test.Core.EventHandlers;

public class CreateUserEventHandlerTests
{
    private Mock<IPropagatedEntityService<PropagatedUser>> _propagateUserServiceMock = new();
    private Mock<ICreateWalletService> _createWalletServiceMock = new();

    [Fact]
    public async Task Handle_NewUser_CreatesUser()
    {
        // Arrange
        var handler = GetValidHandler();
        var message = new UserCreationSucceedMessage
        {
            UserId = 10,
            State = 1
        };
        _propagateUserServiceMock.Setup(x => x.GetByIdAsync(message.UserId))
            .ReturnsAsync((PropagatedUser?)null);
        _propagateUserServiceMock.Setup(x => x.CreateAsync(It.IsAny<PropagatedUser>()))
            .ReturnsAsync(new PropagatedUser
            {
                Id = message.UserId,
                State = message.State
            });
        _createWalletServiceMock.Setup(x => x.CreateDefaultWalletForUserAsync(It.IsAny<int>()))
            .ReturnsAsync(new WalletDto());

        // Act
        await handler.Handle(message, new TestableMessageHandlerContext());

        // Assert
        _propagateUserServiceMock.Verify(x => x.GetByIdAsync(message.UserId), Times.Once);
        _propagateUserServiceMock.Verify(x => x.CreateAsync(It.IsAny<PropagatedUser>()), Times.Once);
        _createWalletServiceMock.Verify(x => x.CreateDefaultWalletForUserAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ExistingUser_DoesNotCallCreate()
    {
        // Arrange
        var handler = GetValidHandler();
        var message = new UserCreationSucceedMessage
        {
            UserId = 10,
            State = 1
        };
        
        _propagateUserServiceMock.Setup(x => x.GetByIdAsync(message.UserId))
            .ReturnsAsync(new PropagatedUser());
        _propagateUserServiceMock.Setup(x => x.CreateAsync(It.IsAny<PropagatedUser>()))
            .ReturnsAsync(new PropagatedUser
            {
                Id = message.UserId,
                State = message.State
            });

        // Act
        await handler.Handle(message, new TestableMessageHandlerContext());

        // Assert
        _propagateUserServiceMock.Verify(x => x.GetByIdAsync(message.UserId), Times.Once);
        _propagateUserServiceMock.Verify(x => x.CreateAsync(It.IsAny<PropagatedUser>()), Times.Never);
    }

    private CreateUserEventHandler GetValidHandler()
    {
        return new CreateUserEventHandler(_propagateUserServiceMock.Object, _createWalletServiceMock.Object);
    }
}
using SelfResearch.UserManagement.API.Contracts;

namespace SelfResearch.UserManagement.API.Features.UserManagement.CreateUser;

public class CreateUserHandler :
    IHandleMessages<UserCreationSucceedMessage>
// IHandleMessages<WalletCreationFailureMessage>
{

    private readonly IUpdateUserService _updateUserService;

    public CreateUserHandler(IUpdateUserService updateUserService)
    {
        _updateUserService = updateUserService;
    }

    /// <inheritdoc/>
    public async Task Handle(UserCreationSucceedMessage message, IMessageHandlerContext context)
    {
        await context.Publish(message);

        return;
    }

    //TODO: Handle wallet creation once the microservice is ready

    // public async Task Handle(WalletCreationSucceedMessage message, IMessageHandlerContext context)
    // {
    //     await this._updateUserService.UpdateUserStateAsync(message.userId, UserStateEnumDto.Active);
    // }

    //     public Task Handle(WalletCreationFailureMessage message, IMessageHandlerContext context)
    //     {
    //         throw new NotImplementedException();
    //     }
}

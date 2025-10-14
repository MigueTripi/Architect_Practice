using System;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SelfResearch.Financial.API.Feature.Propagate;
using SelfResearch.Financial.API.Feature.Wallet.CreateWalet;
using SelfResearch.UserManagement.API.Contracts;

namespace SelfResearch.Financial.API.Core.EventHandlers;

public class CreateUserEventHandler : IHandleMessages<UserCreationSucceedMessage>
{
    private readonly IPropagatedEntityService<PropagatedUser> _propagateUserService;

    private readonly ICreateWalletService _createWalletService;

    public CreateUserEventHandler(
        IPropagatedEntityService<PropagatedUser> propagateUserService,
        ICreateWalletService createWalletService)
    {
        _propagateUserService = propagateUserService;
        _createWalletService = createWalletService;
    }

    public async Task Handle(UserCreationSucceedMessage message, IMessageHandlerContext context)
    {
        var user = await _propagateUserService.GetByIdAsync(message.UserId);
        if (user != null)
        {
            //TODO: Log that the user already exists
            return;
        }

        await _propagateUserService.CreateAsync(new PropagatedUser()
        {
            Id = message.UserId,
            State = message.State
        });
        
        await _createWalletService.CreateDefaultWalletForUserAsync(message.UserId);
    }
}

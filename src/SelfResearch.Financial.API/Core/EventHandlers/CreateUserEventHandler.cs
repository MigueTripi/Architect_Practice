using System;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SelfResearch.Financial.API.Feature.Propagate;
using SelfResearch.UserManagement.API.Contracts;

namespace SelfResearch.Financial.API.Core.EventHandlers;

public class CreateUserEventHandler : IHandleMessages<UserCreationSucceedMessage>
{
    private readonly IPropagatedEntityService<PropagatedUser> _propagateUserService;

    public CreateUserEventHandler(IPropagatedEntityService<PropagatedUser> propagateUserService)
    {
        _propagateUserService = propagateUserService;
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
    }
}

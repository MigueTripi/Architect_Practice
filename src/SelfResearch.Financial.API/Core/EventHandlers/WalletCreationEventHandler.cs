using SelfResearch.Financial.API.Contracts;

namespace SelfResearch.Financial.API.Core.EventHandlers;

public class WalletCreationEventHandler : IHandleMessages<WalletCreationSucceedMessage>
{
    public async Task Handle(WalletCreationSucceedMessage message, IMessageHandlerContext context)
    {
        await context.Publish(message);
 
        return;
    }
}
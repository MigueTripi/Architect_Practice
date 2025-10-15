namespace SelfResearch.Financial.API.Contracts;

public class WalletCreationSucceedMessage : IEvent
{
    public int WalletId { get; set; }
    public int UserId { get; set; }
}
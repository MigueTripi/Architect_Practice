namespace SelfResearch.Financial.API.Feature.Wallet;

public class WalletDto
{
    /// <summary>
    /// The wallet identifier
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// The user identifier associated with the wallet
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// The current balance of the wallet
    /// </summary>
    public double Balance { get; set; }
    /// <summary>
    /// The state of the wallet
    /// </summary>
    public WalletStateEnumDto State { get; set; }
    /// <summary>
    /// The currency of the wallet
    /// </summary>
    public string Currency { get; set; } = null!;
    /// <summary>
    /// The date and time when the wallet was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// The date and time when the wallet was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}
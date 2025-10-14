namespace SelfResearch.Financial.API.Feature.Wallet;

public interface IRetrieveWalletService
{
    /// <summary>
    /// Retrieves the wallets for a specific user
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>The wallets</returns>
    Task<List<WalletDto>> GetWalletsByUserAsync(int userId);
}

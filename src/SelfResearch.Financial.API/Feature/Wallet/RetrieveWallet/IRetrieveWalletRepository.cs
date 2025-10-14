namespace SelfResearch.Financial.API.Feature.Wallet.RetrieveWallet;

public interface IRetrieveWalletRepository
{
    /// <summary>
    /// Retrieves the wallets for a specific user
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>The wallets</returns>
    Task<List<Wallet>> GetWalletsByUserIdAsync(int userId);
}

namespace SelfResearch.Financial.API.Feature.Wallet.RetrieveWallet;

public interface IRetrieveWalletRepository
{
    /// <summary>
    /// Retrieves the wallet information for a specific user
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>The wallet</returns>
    Task<Wallet?> GetWalletByUserIdAsync(int userId);
}

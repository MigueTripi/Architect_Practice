namespace SelfResearch.Financial.API.Feature.Wallet;

public interface IRetrieveWalletService
{
    /// <summary>
    /// Retrieves the wallet information for a specific user
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>The wallet</returns>
    Task<WalletDto?> GetWalletByUserAsync(int userId);
}

using System;

namespace SelfResearch.Financial.API.Feature.Wallet.CreateWalet;

public interface ICreateWalletService
{
    /// <summary>
    /// Creates a new wallet
    /// </summary>
    /// <param name="wallet">The wallet</param>
    /// <returns>The created wallet</returns>
    Task<WalletDto?> CreateAsync(WalletDto wallet);

    /// <summary>
    /// Creates a default wallet for a user if the user does not have any
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>The created wallet</returns>
    Task<WalletDto?> CreateDefaultWalletForUserAsync(int userId);
}

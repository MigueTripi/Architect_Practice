using System;

namespace SelfResearch.Financial.API.Feature.Wallet.CreateWalet;

public interface ICreateWalletRepository
{
    /// <summary>
    /// Creates a new wallet
    /// </summary>
    /// <param name="wallet">The wallet</param>
    /// <returns>The created wallet</returns>
    Task<Wallet> CreateAsync(Wallet wallet);
}

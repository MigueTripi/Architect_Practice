using System;
using Microsoft.EntityFrameworkCore;
using SelfResearch.Financial.API.Core.Data;

namespace SelfResearch.Financial.API.Feature.Wallet.RetrieveWallet;

public class RetrieveWalletRepository : IRetrieveWalletRepository
{

    private readonly AppDbContext _dbContext;

    /// <summary>
    /// The constructor
    /// </summary>
    /// <param name="dbContext">The database context</param>
    public RetrieveWalletRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // <inheritdoc />
    public async Task<Wallet?> GetWalletByUserIdAsync(int userId)
    {
        return await _dbContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
    }
}

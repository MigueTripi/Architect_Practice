using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SelfResearch.Financial.API.Core.Data;

namespace SelfResearch.Financial.API.Feature.Wallet.RetrieveWallet;

[ExcludeFromCodeCoverage]
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
    public async Task<List<Wallet>> GetWalletsByUserIdAsync(int userId)
    {
        return await _dbContext.Wallets.Where(w => w.UserId == userId).ToListAsync();
    }
}

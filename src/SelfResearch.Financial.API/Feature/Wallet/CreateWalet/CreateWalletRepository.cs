using System.Diagnostics.CodeAnalysis;
using SelfResearch.Financial.API.Core.Data;

namespace SelfResearch.Financial.API.Feature.Wallet.CreateWalet;

[ExcludeFromCodeCoverage]
public class CreateWalletRepository : ICreateWalletRepository
{
    private readonly AppDbContext _context;

    public CreateWalletRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Wallet?> CreateAsync(Wallet wallet)
    {
        await _context.Wallets.AddAsync(wallet);
        await _context.SaveChangesAsync();
        return wallet;
    }
}

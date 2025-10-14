using System;
using AutoMapper;

namespace SelfResearch.Financial.API.Feature.Wallet.CreateWalet;

public class CreateWalletService : ICreateWalletService
{
    private readonly ICreateWalletRepository _repository;
    private readonly IRetrieveWalletService _retrieveWalletService;
    private readonly IMapper _mapper;

    public CreateWalletService(
        ICreateWalletRepository repository,
        IRetrieveWalletService retrieveWalletService,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
        _retrieveWalletService = retrieveWalletService;
    }

    // <inheritdoc />
    public Task<WalletDto?> CreateAsync(WalletDto wallet)
    {
        throw new NotImplementedException();
    }

    // <inheritdoc />
    public async Task<WalletDto?> CreateDefaultWalletForUserAsync(int userId)
    {
        var existingWallets = await _retrieveWalletService.GetWalletsByUserAsync(userId);
        if (existingWallets.Any())
        {
            return existingWallets.FirstOrDefault();
        }

        var wallet = await _repository.CreateAsync(GetDefaultWallet(userId));

       return _mapper.Map<WalletDto>(wallet);    
    }

    private Wallet GetDefaultWallet(int userId)
    {
        return new Wallet
        {
            UserId = userId,
            Balance = 0,
            Currency = "EUR", //TODO: Make configurable. In the future, it could be based on user's country or preferences
            CreatedAt = DateTime.UtcNow,
            State = WalletStateEnum.Active
        };
    }
}

using AutoMapper;
using SelfResearch.Financial.API.Feature.Wallet.RetrieveWallet;

namespace SelfResearch.Financial.API.Feature.Wallet;

public class RetrieveWalletService : IRetrieveWalletService
{
    public IRetrieveWalletRepository _retrieveWalletRepository { get; set; }
    public IMapper _mapper { get; set; }

    /// <summary>
    /// The constructor
    /// </summary>
    /// <param name="retrieveWalletRepository">The retrieve wallet repository</param>
    /// <param name="mapper">The mapper</param>
    public RetrieveWalletService(IRetrieveWalletRepository retrieveWalletRepository, IMapper mapper)
    {
        _retrieveWalletRepository = retrieveWalletRepository;
        _mapper = mapper;
    }

    // <inheritdoc />
    public async Task<WalletDto?> GetWalletByUserAsync(int userId)
    {
        var wallet = await _retrieveWalletRepository.GetWalletByUserIdAsync(userId);
        return _mapper.Map<WalletDto?>(wallet);
    }    
}
using System.Threading.Tasks;
using Moq;
using SelfResearch.Financial.API.Feature.Wallet;
using SelfResearch.Financial.API.Feature.Wallet.RetrieveWallet;

namespace SelfResearch.Financial.API.Test.Features.Wallet.RetrieveWallet;

public class RetrieveWalletServiceTests : BaseTests
{
    private Mock<IRetrieveWalletRepository> _retrieveWalletRepositoryMock = new(MockBehavior.Strict);

    [Fact]
    public async Task GetWalletByUserAsync_DontFindWallet_ReturnsNull()
    {
        //Arrange
        _retrieveWalletRepositoryMock.Setup(x => x.GetWalletsByUserIdAsync(It.IsAny<int>()))
            .ReturnsAsync([]);
        var service = GetNewValidService();

        //Act
        var result = await service.GetWalletsByUserAsync(1);

        //Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetWalletByUserAsync_FindsWallet_ReturnsObject()
    {
        //Arrange
        var expectedWallet = GetDummyWallet();
        _retrieveWalletRepositoryMock.Setup(x => x.GetWalletsByUserIdAsync(It.IsAny<int>()))
            .ReturnsAsync([expectedWallet]);
        var service = GetNewValidService();

        //Act
        var result = await service.GetWalletsByUserAsync(1);

        //Assert
        Assert.NotNull(result);
        Assert.Single(result);
        AssertWalletEntity(expectedWallet, result.First());
    }

    private void AssertWalletEntity(Feature.Wallet.Wallet expected, WalletDto result)
    {
        Assert.Equal(expected.Id, result.Id);
        Assert.Equal(expected.UserId, result.UserId);
        Assert.Equal(expected.Balance, result.Balance);
        Assert.Equal(expected.Currency, result.Currency);
        Assert.Equal(expected.CreatedAt, result.CreatedAt);
        Assert.Equal(expected.UpdatedAt, result.UpdatedAt);
        Assert.Equal((int)expected.State, (int)result.State);
        
    }

    private RetrieveWalletService GetNewValidService()
    {
        return new(this._retrieveWalletRepositoryMock.Object, _mapper);
    }

    private Feature.Wallet.Wallet GetDummyWallet()
    {
        return new()
        {
            Id = 1,
            Balance = 10,
            CreatedAt = DateTime.Today,
            Currency = "EUR",
            State = WalletStateEnum.Active,
            UpdatedAt = DateTime.UtcNow,
            UserId = 2
        };
    }
}
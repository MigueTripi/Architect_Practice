using System;
using AutoMapper;
using Moq;
using SelfResearch.Financial.API.Contracts;
using SelfResearch.Financial.API.Feature.Wallet;
using SelfResearch.Financial.API.Feature.Wallet.CreateWalet;

namespace SelfResearch.Financial.API.Test.Features.Wallet.CreateWallet;

public class CreateWalletServiceTests
{
    private Mock<ICreateWalletRepository> _repositoryMock = new();
    private Mock<IRetrieveWalletService> _retrieveWalletServiceMock = new();
    private Mock<IMapper> _mapperMock = new();
    private Mock<IMessageSession> _messageSessionMock = new();

    [Fact]
    public async Task CreateDefaultWalletForUserAsync_NoExistingWallets_CreatesWallet()
    {
        // Arrange
        int userId = 1;
        var expectedWallet = new WalletDto
        {
            Id = 1,
            UserId = userId,
            Balance = 0,
            Currency = "EUR",
            CreatedAt = DateTime.UtcNow
        };

        _retrieveWalletServiceMock.Setup(r => r.GetWalletsByUserAsync(userId))
            .ReturnsAsync(new List<WalletDto>());
        _repositoryMock.Setup(r => r.CreateAsync(It.IsAny<Feature.Wallet.Wallet>()))
            .ReturnsAsync((Feature.Wallet.Wallet w) => { w.Id = 1; return w; });
        _mapperMock.Setup(m => m.Map<WalletDto>(It.IsAny<Feature.Wallet.Wallet>()))
            .Returns(expectedWallet);


        var service = GetValidService();

        // Act
        var result = await service.CreateDefaultWalletForUserAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedWallet.UserId, result.UserId);
        Assert.Equal(expectedWallet.Balance, result.Balance);
        Assert.Equal(expectedWallet.Id, result.Id);
        Assert.Equal(expectedWallet.Currency, result.Currency);
    }

    [Fact]
    public async Task CreateDefaultWalletForUserAsync_WithExistingWallets_DoesNotCreateWallet()
    {
        // Arrange
        int userId = 1;
        var existingWallet = new WalletDto
        {
            Id = 1,
            UserId = userId,
            Balance = 0,
            Currency = "EUR",
            CreatedAt = DateTime.UtcNow
        };

        _retrieveWalletServiceMock.Setup(r => r.GetWalletsByUserAsync(userId))
            .ReturnsAsync(new List<WalletDto> { existingWallet});

        var service = GetValidService();

        // Act
        var result = await service.CreateDefaultWalletForUserAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(existingWallet.UserId, result.UserId);
        Assert.Equal(existingWallet.Balance, result.Balance);
        Assert.Equal(existingWallet.Id, result.Id);
        Assert.Equal(existingWallet.Currency, result.Currency);

        _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<Feature.Wallet.Wallet>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ThrowsNotImplementedException()
    {
        // Arrange
        var service = GetValidService();

        // Act & Assert
        await Assert.ThrowsAsync<NotImplementedException>(() => service.CreateAsync(new()));
    }

    private CreateWalletService GetValidService()
    {
        return new(
            _repositoryMock.Object,
            _retrieveWalletServiceMock.Object,
            _mapperMock.Object,
            _messageSessionMock.Object
        );
    }
}

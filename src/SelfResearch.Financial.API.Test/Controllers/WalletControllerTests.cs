using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SelfResearch.Financial.API.Controllers;
using SelfResearch.Financial.API.Feature.Wallet;

namespace SelfResearch.Financial.API.Test.Controllers;

public class WalletControllerTests
{

    private Mock<IRetrieveWalletService> _retrieveWalletServiceMock = new(MockBehavior.Strict);
    private Mock<ILogger<WalletController>> _loggerMock  = new();

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetWalletByUserId_WithInvalidId_Returns400BadRequest(int userId)
    {
        //Arrange and Act
        var controller = GetNewValidController();
        var result = await controller.GetWalletByUserId(userId);

        //Arrange
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetWalletByUserId_ForNonExistingWallet_Returns404NotFound()
    {
        //Arrange
        _retrieveWalletServiceMock.Setup(x => x.GetWalletByUserAsync(It.IsAny<int>())).ReturnsAsync((WalletDto?)null);
        var controller = GetNewValidController();

        //Act
        var result = await controller.GetWalletByUserId(1);

        //Arrange
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetWalletByUserId_ForExistingWallet_ReturnsOk()
    {
        //Arrange
        _retrieveWalletServiceMock.Setup(x => x.GetWalletByUserAsync(It.IsAny<int>())).ReturnsAsync(new WalletDto());
        var controller = GetNewValidController();

        //Act
        var result = await controller.GetWalletByUserId(1);

        //Arrange
        Assert.IsType<OkObjectResult>(result.Result);
    }

    private WalletController GetNewValidController()
    {
        return new(_loggerMock.Object, _retrieveWalletServiceMock.Object);

    }
}

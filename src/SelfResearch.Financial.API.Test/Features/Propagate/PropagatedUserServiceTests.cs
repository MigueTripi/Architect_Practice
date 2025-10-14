using System;
using Moq;
using SelfResearch.Financial.API.Feature.Propagate;

namespace SelfResearch.Financial.API.Test.Features.Propagate;

public class PropagatedUserServiceTests
{
    private Mock<IPropagatedEntityRepository<PropagatedUser>> _repositoryMock = new();

    [Fact]
    public async Task CreateAsync_NewUser_CreatesUser()
    {
        // Arrange
        var service = GetValidService();
        var newUser = new PropagatedUser
        {
            Id = 10,
            State = 1
        };

        _repositoryMock.Setup(x => x.GetByIdAsync(newUser.Id))
            .ReturnsAsync((PropagatedUser?)null);
        _repositoryMock.Setup(x => x.CreateAsync(newUser))
            .ReturnsAsync(newUser);

        // Act
        var result = await service.CreateAsync(newUser);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newUser.Id, result?.Id);
        _repositoryMock.Verify(x => x.GetByIdAsync(newUser.Id), Times.Once);
        _repositoryMock.Verify(x => x.CreateAsync(newUser), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ExisitingUser_RetrievesExistingUser()
    {
        // Arrange
        var service = GetValidService();
        var newUser = new PropagatedUser
        {
            Id = 10,
            State = 1
        };

        _repositoryMock.Setup(x => x.GetByIdAsync(newUser.Id))
            .ReturnsAsync(newUser);
        _repositoryMock.Setup(x => x.CreateAsync(newUser))
            .ReturnsAsync(new PropagatedUser());

        // Act
        var result = await service.CreateAsync(newUser);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newUser.Id, result?.Id);
        _repositoryMock.Verify(x => x.GetByIdAsync(newUser.Id), Times.Once);
        _repositoryMock.Verify(x => x.CreateAsync(newUser), Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingUser_ReturnsNull()
    {
        // Arrange
        var service = GetValidService();
        _repositoryMock.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((PropagatedUser?)null);

        // Act
        var result = await service.GetByIdAsync(1);

        // Assert
        Assert.Null(result);
        _repositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingUser_ReturnsUser()
    {
        // Arrange
        var service = GetValidService();
        var existingUser = new PropagatedUser
        {
            Id = 10,
            State = 1
        };
        _repositoryMock.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(existingUser);

        // Act
        var result = await service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(existingUser.Id, result!.Id);
        _repositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);
    }

    private PropagatedUserService GetValidService()
    {
        return new PropagatedUserService(_repositoryMock.Object);
    }
}

using AutoMapper;
using SelfResearch.UserManagement.API.Features.UserManagement;
using Moq;
using SelfResearch.UserManagement.API.Features.UserManagement.CreateUser;
using SelfResearch.Core.Infraestructure.ErrorHandling;

namespace SelfResearch.UserManagement.API.Test.Features.UserManagement.UpdateUser;

public class UpdateUserServiceTest
{
    private Mock<IUserManagementRepository> _userManagementRepositoryMock = new();
    private Mock<IMapper> _mapperMock = new();

    [Fact]
    public async Task UpdateStateAsync_WithNonExistentUser_ReturnsNull()
    {
        // Arrange
        _userManagementRepositoryMock.Setup(x => x.GetUserAsync(It.IsAny<int>()))
            .ReturnsAsync((User?)null);

        var service = GetNewValidService();

        // Act
        var result = await service.UpdateUserStateAsync(1, UserStateEnumDto.Active);

        // Assert
        Assert.Single(result.Errors);
        Assert.NotNull(result.Errors.FirstOrDefault(e => e is NotFoundError));
    }

    [Fact]
    public async Task UpdateStateAsync_WithExistentUser_ReturnsUser()
    {
        // Arrange
        var user = new User { Id = 1, Name = "Test User", Email = "m@m.com", State = UserStateEnum.Active };
        _userManagementRepositoryMock.Setup(x => x.GetUserAsync(It.IsAny<int>()))
            .ReturnsAsync(user);
        _userManagementRepositoryMock.Setup(x => x.UpdateUserAsync())
            .Returns(Task.CompletedTask);
        _mapperMock.Setup(x => x.Map<UserDto>(It.IsAny<User>()))
            .Returns((User source) => new UserDto
            {
                Id = source.Id,
                Name = source.Name,
                Email = source.Email,
                State = (UserStateEnumDto)source.State
            });

        var service = GetNewValidService();

        // Act
        var result = await service.UpdateUserStateAsync(1, UserStateEnumDto.Inactive);

        // Assert
        Assert.NotNull(result);
        AssertUserData(result.Value, user);
        _userManagementRepositoryMock.Verify(x => x.UpdateUserAsync(), Times.Once);
    }

    private UpdateUserService GetNewValidService()
    {
        return new UpdateUserService(
            _userManagementRepositoryMock.Object,
            _mapperMock.Object);
    }
    
    private void AssertUserData(UserDto dto, User user)
    {
        Assert.Equal(dto.Id, user.Id);
        Assert.Equal(dto.Name, user.Name);
        Assert.Equal(dto.Email, user.Email);
        Assert.Equal((int)dto.State, (int)user.State);
    }
}
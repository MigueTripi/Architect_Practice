using AutoMapper;
using SelfResearch.UserManagement.API.Features.UserManagement;
using Moq;
using SelfResearch.UserManagement.API.Features.UserManagement.CreateUser;

namespace SelfResearch.UserManagement.API.Test.Features.UserManagement.CreateUser;

public class CreateUserServiceTest
{
    private Mock<IUserManagementRepository> _userManagementRepositoryMock = new();
    private Mock<IMapper> _mapperMock = new();

    [Fact]
    public async Task CreateUserAsync_ReturnsCreatedUser()
    {
        // Arrange
        var userDto = new UserDto { Name = "New User", Email = "test@test.com", State = UserStateEnumDto.Active };
        var user = new User { Id = 1, Name = userDto.Name, Email = userDto.Email, State = UserStateEnum.Active };

        _mapperMock.Setup(x => x.Map<User>(It.IsAny<UserDto>()))
            .Returns(user);
        _mapperMock.Setup(x => x.Map<UserDto>(It.IsAny<User>()))
            .Returns(userDto);
        _userManagementRepositoryMock.Setup(x => x.CreateUserAsync(It.IsAny<User>()))
            .ReturnsAsync(user);

        var service = GetNewValidService();

        // Act
        var result = await service.CreateUserAsync(userDto);

        // Assert
        Assert.Equal(userDto.Name, result.Name);
        Assert.Equal(userDto.Email, result.Email);
        Assert.Equal((int)userDto.State, (int)result.State);
    }

    private CreateUserService GetNewValidService()
    {
        return new CreateUserService(_userManagementRepositoryMock.Object, this._mapperMock.Object);
    }
    
    private void AssertUserData(UserDto dto, User user)
    {
        Assert.Equal(dto.Id, user.Id);
        Assert.Equal(dto.Name, user.Name);
        Assert.Equal(dto.Email, user.Email);
        Assert.Equal((int)dto.State, (int)user.State);
    }
}
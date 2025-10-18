using AutoMapper;
using SelfResearch.UserManagement.API.Features.UserManagement;
using Moq;
using SelfResearch.UserManagement.API.Features.UserManagement.CreateUser;
using SelfResearch.Core.Infraestructure.ErrorHandling;

namespace SelfResearch.UserManagement.API.Test.Features.UserManagement.CreateUser;

public class CreateUserServiceTest
{
    private Mock<IUserManagementRepository> _userManagementRepositoryMock = new();
    private Mock<IMapper> _mapperMock = new();
    private Mock<IMessageSession> _messageSesionMock = new();

    [Fact]
    public async Task CreateUserAsync_WithCorrectDto_ReturnsCreatedUserWithStateInitial()
    {
        // Arrange
        UserDto userDto = new UserDto()
        {
            Id = 0,
            Name = "Test User dto",
            Email = "mail@dto.com",
            State = UserStateEnumDto.Active
        };

        _mapperMock.Setup(x => x.Map<User>(It.IsAny<UserDto>()))
            .Returns((UserDto userDto) =>
            {
                return new User
                {
                    Id = userDto.Id,
                    Name = userDto.Name,
                    Email = userDto.Email,
                    State = (UserStateEnum)userDto.State
                };
            });

        _mapperMock.Setup(x => x.Map<UserDto>(It.IsAny<User>()))
            .Returns((User anUser) =>
            {
                return new UserDto
                {
                    Id = anUser.Id,
                    Name = anUser.Name,
                    Email = anUser.Email,
                    State = (UserStateEnumDto)anUser.State
                };
            });

        _userManagementRepositoryMock.Setup(x => x.CreateUserAsync(It.IsAny<User>()))
            .ReturnsAsync((User anUser) => { return anUser; });

        var service = GetNewValidService();

        // Act
        var result = await service.CreateUserAsync(userDto);

        // Assert
        Assert.Equal(userDto.Name, result.Value.Name);
        Assert.Equal(userDto.Email, result.Value.Email);
        Assert.Equal((int)UserStateEnumDto.Initial, (int)result.Value.State);
    }

    [Fact]
    public async Task CreateUserAsync_WithIncorrectDto_ReturnsFailedResult()
    {
        // Arrange
        UserDto userDto = new UserDto()
        {
            Id = 1,
            Name = "invalid id",
            Email = "mail@dto.com",
            State = UserStateEnumDto.Active
        };

        var service = GetNewValidService();

        // Act
        var result = await service.CreateUserAsync(userDto);

        // Assert
        Assert.Single(result.Errors);
        Assert.NotNull(result.Errors.FirstOrDefault(e => e is ArgumentError));
    }

    private CreateUserService GetNewValidService()
    {
        return new CreateUserService(
            _userManagementRepositoryMock.Object,
            _mapperMock.Object,
            _messageSesionMock.Object);
    }
}
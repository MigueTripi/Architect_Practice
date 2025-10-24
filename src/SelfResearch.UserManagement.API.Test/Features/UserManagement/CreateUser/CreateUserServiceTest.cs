using AutoMapper;
using SelfResearch.UserManagement.API.Features.UserManagement;
using Moq;
using SelfResearch.UserManagement.API.Features.UserManagement.CreateUser;
using SelfResearch.Core.Infraestructure.ErrorHandling;
using System.Linq.Expressions;
using SelfResearch.UserManagement.API.Contracts;

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

        _messageSesionMock.Setup(x => x.Publish(
            It.IsAny<UserCreationSucceedMessage>(), It.IsAny<PublishOptions>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var service = GetNewValidService();

        // Act
        var result = await service.CreateUserAsync(userDto);

        // Assert
        Assert.Equal(userDto.Name, result.Value.Name);
        Assert.Equal(userDto.Email, result.Value.Email);
        Assert.Equal((int)UserStateEnumDto.Initial, (int)result.Value.State);
        _messageSesionMock.Verify(x => x.Publish(
            It.IsAny<UserCreationSucceedMessage>(), It.IsAny<PublishOptions>(), It.IsAny<CancellationToken>()
            ), Times.Once);
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

    [Theory]
    [InlineData("duplicated name", "duplicated name", "db@email.com", "valid@email.com", "A user with the same name already exists.")]
    [InlineData("DUPLICATED name", "duplicated name", "db@email.com", "valid@email.com", "A user with the same name already exists.")]
    [InlineData("DUPLICATED name ", " duplicated name", "db@email.com", "valid@email.com", "A user with the same name already exists.")]
    [InlineData("valid name", "db name", "duplicated@email.com", "duplicated@email.com", "A user with the same email already exists.")]
    [InlineData("VALID name", "DB name", "duplicated@email.com", "duplicated@EMAIL.com", "A user with the same email already exists.")]
    public async Task CreateUserAsync_WithExistingUser_ReturnsFailedResult(string dtoName, string dbName, string dtoEmail, string dbEmail, string expectedMessage)
    {
        // Arrange
        UserDto userDto = new UserDto()
        {
            Name = dtoName,
            Email = dtoEmail,
            State = UserStateEnumDto.Active
        };

        _userManagementRepositoryMock.Setup(x => x.FindUserByPredicateAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(new User
            {
                Id = 99,
                Name = dbName,
                Email = dbEmail,
                State = UserStateEnum.Active
            });

        var service = GetNewValidService();

        // Act
        var result = await service.CreateUserAsync(userDto);

        // Assert
        Assert.Single(result.Errors);
        var error = result.Errors.First(e => e is ArgumentError);
        Assert.NotNull(error);
        Assert.Contains(expectedMessage, error.Message);
    }

    private CreateUserService GetNewValidService()
    {
        return new CreateUserService(
            _userManagementRepositoryMock.Object,
            _mapperMock.Object,
            _messageSesionMock.Object);
    }
}
using AutoMapper;
using SelfResearch.UserManagement.API.Features.UserManagement;
using Moq;
using SelfResearch.Core.Infraestructure.ErrorHandling;
using Microsoft.AspNetCore.JsonPatch;

namespace SelfResearch.UserManagement.API.Test.Features.UserManagement;

public class UserManagementServiceTest
{
    private Mock<IUserManagementRepository> _userManagementRepositoryMock = new();
    private Mock<IMapper> _mapperMock = new();

    [Fact]
    public async Task GetUserAsync_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        _userManagementRepositoryMock.Setup(x => x.GetUserAsync(It.IsAny<int>()))
            .ReturnsAsync((User)null!);

        var service = GetNewValidService();

        // Act
        var result = await service.GetUserAsync(1);

        // Assert
        Assert.Single(result.Errors);
        Assert.NotNull(result.Errors.FirstOrDefault(e => e is NotFoundError));
    }

    [Fact]
    public async Task GetUserAsync_WithValidId_ReturnsUser()
    {
        // Arrange
        var testUser = new User { Id = 1, Name = "Test User", Email = "m@mail.net", State = UserStateEnum.Active };
        _userManagementRepositoryMock.Setup(x => x.GetUserAsync(1))
            .ReturnsAsync(testUser);

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
        var result = await service.GetUserAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Value);
        AssertUserData(result.Value, testUser);
    }

    [Fact]
    public async Task GetPagedUsersAsync_ReturnsPagedUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, Name = "User1", Email = "m1@mail.net", State = UserStateEnum.Active },
            new User { Id = 1, Name = "User2", Email = "m2@mail.net", State = UserStateEnum.Active },
        };

        _userManagementRepositoryMock.Setup(x => x.GetPagedUsersAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(users);

        _mapperMock.Setup(x => x.Map<List<UserDto>>(It.IsAny<List<User>>()))
            .Returns((List<User> source) => source.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                State = (UserStateEnumDto)u.State
            }).ToList());

        var service = GetNewValidService();

        // Act
        var result = await service.GetPagedUsersAsync(0, 10);

        // Assert
        Assert.Equal(2, result.Count);
        AssertUserData(result[0], users[0]);
    }

    [Fact]
    public async Task PatchUserAsync_WithStatePropertyOnOperations_ReturnsArgumentError()
    {
        // Arrange
        _userManagementRepositoryMock.Setup(x => x.GetUserAsync(It.IsAny<int>()))
            .ReturnsAsync((User?)null);

        var service = GetNewValidService();

        JsonPatchDocument<UserDto> doc = CreateDummyPatchDocument();
        doc.Replace(u => u.State, UserStateEnumDto.Active);

        // Act
        var result = await service.PatchUserAsync(1, doc);

        // Assert
        Assert.Single(result.Errors);
        Assert.NotNull(result.Errors.FirstOrDefault(e => e is ArgumentError));
    }

    [Fact]
    public async Task PatchUserAsync_WithNonExistentUser_ReturnsNull()
    {
        // Arrange
        _userManagementRepositoryMock.Setup(x => x.GetUserAsync(It.IsAny<int>()))
            .ReturnsAsync((User?)null);

        var service = GetNewValidService();

        // Act
        var result = await service.PatchUserAsync(1, CreateDummyPatchDocument());

        // Assert
        Assert.Single(result.Errors);
        Assert.NotNull(result.Errors.FirstOrDefault(e => e is NotFoundError));
    }

    [Fact]
    public async Task PatchUserAsync_WithExistingUser_ReturnsUpdatedUserName()
    {
        // Arrange
        var userDto = new UserDto { Id = 1, Name = "Updated User", Email = "updated@test.com", State = UserStateEnumDto.Inactive };
        var existingUser = new User { Id = 1, Name = "Original User", Email = "original@test.com", State = UserStateEnum.Inactive };
        var patchDoc = CreateDummyPatchDocument();
        patchDoc.Replace(u => u.Name, userDto.Name);

        _userManagementRepositoryMock.Setup(x => x.GetUserAsync(userDto.Id))
            .ReturnsAsync(existingUser);
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
        var result = await service.PatchUserAsync(1, patchDoc);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userDto.Name, result.Value.Name);
        Assert.Equal(existingUser.Email, result.Value.Email);
        Assert.Equal((int)userDto.State, (int)result.Value.State);
    }

    [Fact]
    public async Task PatchUserAsync_WithExistingUser_ReturnsUpdatedUserEmail()
    {
        // Arrange
        var userDto = new UserDto { Id = 1, Name = "Original User", Email = "updated@test.com", State = UserStateEnumDto.Inactive };
        var existingUser = new User { Id = 1, Name = "Original User", Email = "original@test.com", State = UserStateEnum.Inactive };
        var patchDoc = CreateDummyPatchDocument(userDto.Name);
        patchDoc.Replace(u => u.Email, userDto.Email);

        _userManagementRepositoryMock.Setup(x => x.GetUserAsync(userDto.Id))
            .ReturnsAsync(existingUser);
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
        var result = await service.PatchUserAsync(1, patchDoc);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(existingUser.Name, result.Value.Name);
        Assert.Equal(userDto.Email, result.Value.Email);
        Assert.Equal((int)userDto.State, (int)result.Value.State);
    }

    [Fact]
    public async Task DeleteUserAsync_WithNonExistentUser_ReturnsFalse()
    {
        // Arrange
        _userManagementRepositoryMock.Setup(x => x.GetUserAsync(It.IsAny<int>()))
            .ReturnsAsync((User?)null);

        var service = GetNewValidService();

        // Act
        var result = await service.DeleteUserAsync(1);

        // Assert
        Assert.Single(result.Errors);
        Assert.NotNull(result.Errors.FirstOrDefault(e => e is NotFoundError));
    }

    [Fact]
    public async Task DeleteUserAsync_WithExistingUser_ReturnsTrue()
    {
        // Arrange
        var user = new User { Id = 1, Name = "Test User" };
        _userManagementRepositoryMock.Setup(x => x.GetUserAsync(It.IsAny<int>()))
            .ReturnsAsync(user);
        _userManagementRepositoryMock.Setup(x => x.DeleteUserAsync(It.IsAny<int>()))
            .Returns(Task.CompletedTask);

        var service = GetNewValidService();

        // Act
        var result = await service.DeleteUserAsync(1);

        // Assert
        Assert.True(result.IsSuccess);
        _userManagementRepositoryMock.Verify(x => x.DeleteUserAsync(1), Times.Once);
    }

    private UserManagementService GetNewValidService()
    {
        return new UserManagementService(_userManagementRepositoryMock.Object, this._mapperMock.Object);
    }

    private void AssertUserData(UserDto dto, User user)
    {
        Assert.Equal(dto.Id, user.Id);
        Assert.Equal(dto.Name, user.Name);
        Assert.Equal(dto.Email, user.Email);
        Assert.Equal((int)dto.State, (int)user.State);
    }
    
    private JsonPatchDocument<UserDto> CreateDummyPatchDocument(string? value = null)
    {
        var patchDoc = new JsonPatchDocument<UserDto>();
        patchDoc.Replace(u=> u.Name, value);
        return patchDoc;
    }
}
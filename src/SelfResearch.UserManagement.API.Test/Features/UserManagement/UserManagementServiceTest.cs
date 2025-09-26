using AutoMapper;
using SelfResearch.UserManagement.API.Features.UserManagement;
using Moq;

namespace SelfResearch.UserManagement.API.Test.Features.UserManagement;

public class UserManagementServiceTest
{
    private Mock<IUserManagementRepository> _userManagementRepositoryMock = new();
    private Mock<IMapper> _mapperMock = new(); 

    [Fact]
    public async Task GetUserAsync_WithNonExistentId_ReturnsNull()
    {
        // Arrange
        _userManagementRepositoryMock.Setup(x => x.GetUserAsync(It.IsAny<int>()))
            .ReturnsAsync((User)null);

        var service = GetNewValidService();

        // Act
        var result = await service.GetUserAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserAsync_WithValidId_ReturnsUser()
    {
        // Arrange
        var testUser = new User { Id = 1, Name = "Test User" };
        _userManagementRepositoryMock.Setup(x => x.GetUserAsync(1))
            .ReturnsAsync(testUser);

        _mapperMock.Setup(x => x.Map<UserDto>(It.IsAny<User>()))
            .Returns((User source) => new UserDto
            {
                Id = source.Id,
                Name = source.Name,
                Email = source.Email
            });
        var service = GetNewValidService();
        
        // Act
        var result = await service.GetUserAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testUser.Id, result.Id);
        Assert.Equal(testUser.Name, result.Name);
    }

    [Fact]
    public async Task GetPagedUsersAsync_ReturnsPagedUsers()
    {
        // Arrange
        var users = new List<User> 
        { 
            new User { Id = 1, Name = "User1" },
            new User { Id = 2, Name = "User2" }
        };
        
        _userManagementRepositoryMock.Setup(x => x.GetPagedUsersAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(users);

        _mapperMock.Setup(x => x.Map<List<UserDto>>(It.IsAny<List<User>>()))
            .Returns((List<User> source) => source.Select(u => new UserDto 
            { 
                Id = u.Id, 
                Name = u.Name,
                Email = u.Email 
            }).ToList());

        var service = GetNewValidService();

        // Act
        var result = await service.GetPagedUsersAsync(0, 10);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(users[0].Name, result[0].Name);
    }

    [Fact]
    public async Task CreateUserAsync_ReturnsCreatedUser()
    {
        // Arrange
        var userDto = new UserDto { Name = "New User", Email = "test@test.com" };
        var user = new User { Id = 1, Name = userDto.Name, Email = userDto.Email };

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
    }

    [Fact]
    public async Task PatchUserAsync_WithNonExistentUser_ReturnsNull()
    {
        // Arrange
        _userManagementRepositoryMock.Setup(x => x.GetUserAsync(It.IsAny<int>()))
            .ReturnsAsync((User?)null);

        var service = GetNewValidService();

        // Act
        var result = await service.PatchUserAsync(1, new());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task PatchUserAsync_WithExistingUser_ReturnsUpdatedUserName()
    {
        // Arrange
        var userDto = new UserDto { Id = 1, Name = "Updated User", Email = "updated@test.com" };
        var existingUser = new User { Id = 1, Name = "Original User", Email = "original@test.com" };
        var patchDoc = new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<UserDto>();
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
                Email = source.Email 
            });

        var service = GetNewValidService();

        // Act
        var result = await service.PatchUserAsync(1, patchDoc);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userDto.Name, result.Name);
        Assert.Equal(existingUser.Email, result.Email);
    }

    [Fact]
    public async Task PatchUserAsync_WithExistingUser_ReturnsUpdatedUserEmail()
    {
        // Arrange
        var userDto = new UserDto { Id = 1, Name = "Updated User", Email = "updated@test.com" };
        var existingUser = new User { Id = 1, Name = "Original User", Email = "original@test.com" };
        var patchDoc = new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<UserDto>();
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
                Email = source.Email 
            });

        var service = GetNewValidService();

        // Act
        var result = await service.PatchUserAsync(1, patchDoc);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(existingUser.Name, result.Name);
        Assert.Equal(userDto.Email, result.Email);
    }

    [Fact]
    public async Task DeleteUserAsync_WithNonExistentUser_ReturnsFalse()
    {
        // Arrange
        _userManagementRepositoryMock.Setup(x => x.GetUserAsync(It.IsAny<int>()))
            .ReturnsAsync((User)null);

        var service = GetNewValidService();

        // Act
        var result = await service.DeleteUserAsync(1);

        // Assert
        Assert.False(result);
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
        Assert.True(result);
    }

    private UserManagementService GetNewValidService()
    {
        return new UserManagementService(_userManagementRepositoryMock.Object, this._mapperMock.Object);
    }
}
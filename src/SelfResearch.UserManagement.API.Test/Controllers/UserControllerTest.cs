using SelfResearch.UserManagement.API.Controllers;
using SelfResearch.UserManagement.API.Features.UserManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SelfResearch.UserManagement.API.Features.UserManagement.CreateUser;

namespace SelfResearch.UserManagement.API.Test.Controllers;

public class UserControllerTest
{
    private Mock<ILogger<UserController>> _loggerMock = new();
    private Mock<IUserManagementService> _userManagementService = new();
    private Mock<ICreateUserService> _createUserService = new();
    private UserDto _existingTestUser = new UserDto { Id = 1, Name = "Existing User" };
    private UserDto _newTestUser = new UserDto { Id = 0, Name = "New User" };

    [Fact]
    public async Task GetUserById_WithInvalidId_ReturnsBadRequest()
    {
        // Arrange
        var controller = GetNewValidController();

        // Act
        var result = await controller.GetUserById(0);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetUserById_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        _userManagementService.Setup(x => x.GetUserAsync(It.IsAny<int>()))
            .ReturnsAsync((UserDto)null);
        var controller = GetNewValidController();

        // Act
        var result = await controller.GetUserById(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetUserById_WithValidId_ReturnsUser()
    {
        // Arrange
        _userManagementService.Setup(x => x.GetUserAsync(1))
            .ReturnsAsync(_existingTestUser);
        var controller = GetNewValidController();

        // Act
        var result = await controller.GetUserById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(_existingTestUser, okResult.Value);
    }

    [Fact]
    public async Task GetPagedUsers_WithValidUser_ReturnsUsers()
    {
        // Arrange
        var users = new List<UserDto> { _existingTestUser };
        _userManagementService.Setup(x => x.GetPagedUsersAsync(1, 10))
            .ReturnsAsync(users);
        var controller = GetNewValidController();

        // Act
        var result = await controller.GetPagedUsers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(users, okResult.Value);
    }

    [Fact]
    public async Task CreateUser_WithNullUser_ReturnsBadRequest()
    {
        // Arrange
        var controller = GetNewValidController();

        // Act
        var result = await controller.CreateUser(null);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateUser_WithExistingUser_ReturnsBadRequest()
    {
        // Arrange
        var controller = GetNewValidController();

        // Act
        var result = await controller.CreateUser(new() { Id = 1, Name = "Existing User" });

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateUser_WithValidRequest_ReturnsCreatedUser()
    {
        // Arrange
        _createUserService.Setup(x => x.CreateUserAsync(_newTestUser))
            .ReturnsAsync(_newTestUser);
        var controller = GetNewValidController();

        // Act
        var result = await controller.CreateUser(_newTestUser);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(_newTestUser, createdResult.Value);
    }

    [Fact]
    public async Task PatchUser_WithNullDocoument_ReturnsBadRequest()
    {
        // Arrange
        var controller = GetNewValidController();

        // Act
        var result = await controller.PatchUser(1, null);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }


    [Fact]
    public async Task PatchUser_WithInvalidId_ReturnsBadRequest()
    {
        // Arrange
        var controller = GetNewValidController();

        // Act
        var result = await controller.PatchUser(0, new());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task PatchUser_WithNonExistentUser_ReturnsNotFound()
    {
        // Arrange
        _userManagementService.Setup(x => x.PatchUserAsync(It.IsAny<int>(), It.IsAny<Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<UserDto>>()))
            .ReturnsAsync((UserDto?)null);
        var controller = GetNewValidController();

        // Act
        var result = await controller.PatchUser(1, new());

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task PatchUser_WithValidUser_ReturnsUpdatedUser()
    {
        // Arrange
        _userManagementService.Setup(x => x.PatchUserAsync(It.IsAny<int>(), It.IsAny<Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<UserDto>>()))
            .ReturnsAsync(_existingTestUser);
        var controller = GetNewValidController();

        // Act
        var result = await controller.PatchUser(1, new());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(_existingTestUser, okResult.Value);
    }

    [Fact]
    public async Task DeleteUser_ReturnsResult()
    {
        // Arrange
        _userManagementService.Setup(x => x.DeleteUserAsync(1))
            .ReturnsAsync(true);
        var controller = GetNewValidController();

        // Act
        var result = await controller.DeleteUser(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.True((bool)okResult.Value);
    }

    [Theory]
    [InlineData(0, UserStateEnumDto.Active)]
    [InlineData(-1, UserStateEnumDto.Active)]
    [InlineData(1, null)]
    public async Task UpdateStateAsync_WithInvalidRequest_ReturnsBadRequest(int userId, UserStateEnumDto? state)
    {
        // Arrange
        var controller = GetNewValidController();

        // Act
        var result = await controller.UpdateUserState(userId, state == null ? null : new() { UserState = state.Value });

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateStateAsync_WithInexistingUser_ReturnsNotFound()
    {
        // Arrange
        _userManagementService.Setup(x => x.UpdateUserStateAsync(It.IsAny<int>(), It.IsAny<UserStateEnumDto>()))
            .ReturnsAsync((UserDto?)null);
        var controller = GetNewValidController();

        // Act
        var result = await controller.UpdateUserState(1, new() { UserState = UserStateEnumDto.Active });

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateStateAsync_WithExistingUser_ReturnsOk()
    {
        // Arrange
        _userManagementService.Setup(x => x.UpdateUserStateAsync(It.IsAny<int>(), It.IsAny<UserStateEnumDto>()))
            .ReturnsAsync(new UserDto());
        var controller = GetNewValidController();

        // Act
        var result = await controller.UpdateUserState(1, new() { UserState = UserStateEnumDto.Active });

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    private UserController GetNewValidController()
    {
        return new UserController(
            _loggerMock.Object,
            _userManagementService.Object,
            _createUserService.Object);
    }

}
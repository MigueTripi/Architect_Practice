using AutoMapper;
using SelfResearch.UserManagement.API.Features.UserManagement;
using Moq;
using SelfResearch.UserManagement.API.Features.UserManagement.CreateUser;
using SelfResearch.Core.Infraestructure.ErrorHandling;
using Microsoft.AspNetCore.JsonPatch;

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
    
    private JsonPatchDocument<UserDto> CreateDummyPatchDocument(string? value = null)
    {
        var patchDoc = new JsonPatchDocument<UserDto>();
        patchDoc.Replace(u=> u.Name, value);
        return patchDoc;
    }
}
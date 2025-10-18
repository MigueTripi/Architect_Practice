using FluentResults;

namespace SelfResearch.UserManagement.API.Features.UserManagement.CreateUser;

public interface ICreateUserService
{
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="userDto">The user to create</param>
    /// <returns>The created user</returns>
    Task<Result<UserDto>> CreateUserAsync(UserDto userDto);
}

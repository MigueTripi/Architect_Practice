using FluentResults;

namespace SelfResearch.UserManagement.API.Features.UserManagement.CreateUser;

public interface IUpdateUserService
{
    /// <summary>
    /// Update user's state.
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <param name="state">The user state</param>
    /// <returns>The result for the updated user</returns>
    Task<Result<UserDto>> UpdateUserStateAsync(int userId, UserStateEnumDto state);
}

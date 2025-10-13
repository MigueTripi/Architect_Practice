using System;

namespace SelfResearch.UserManagement.API.Features.UserManagement.CreateUser;

public interface IUpdateUserService
{
    /// <summary>
    /// Update user's state.
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <param name="state">The user state</param>
    /// <returns>The updated user</returns>
    Task<UserDto?> UpdateUserStateAsync(int userId, UserStateEnumDto state);
}

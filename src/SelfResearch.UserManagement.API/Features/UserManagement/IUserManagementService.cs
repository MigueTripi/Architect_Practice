using FluentResults;
using Microsoft.AspNetCore.JsonPatch;

namespace SelfResearch.UserManagement.API.Features.UserManagement;

public interface IUserManagementService
{
    /// <summary>
    /// Gets a user by its identifier.
    /// </summary>
    /// <param name="id">The user identifier</param>
    /// <returns>The user's result</returns>
    Task<Result<UserDto?>> GetUserAsync(int id);

    /// <summary>
    /// Gets a paged list of users.
    /// </summary>
    /// <param name="skip">The quantity to bypass</param>
    /// <param name="take">The quantity to return</param>
    /// <returns>The paged users</returns>
    Task<List<UserDto>> GetPagedUsersAsync(int skip, int take);

    /// <summary>
    /// Deletes a user by its identifier.
    /// </summary>
    /// <param name="id">The identifier</param>
    /// <returns>Result indicating if operation finished sucessfully</returns>
    Task<Result<bool>> DeleteUserAsync(int id);
}

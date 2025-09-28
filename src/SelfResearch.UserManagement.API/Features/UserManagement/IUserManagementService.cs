using System;
using Microsoft.AspNetCore.JsonPatch;

namespace SelfResearch.UserManagement.API.Features.UserManagement;

public interface IUserManagementService
{
    /// <summary>
    /// Gets a user by its identifier.
    /// </summary>
    /// <param name="id">The user identifier</param>
    /// <returns>The user</returns>
    Task<UserDto?> GetUserAsync(int id);

    /// <summary>
    /// Gets a paged list of users.
    /// </summary>
    /// <param name="skip">The quantity to bypass</param>
    /// <param name="take">The quantity to return</param>
    /// <returns>The paged users</returns>
    Task<List<UserDto>> GetPagedUsersAsync(int skip, int take);


    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="userDto">The user to create</param>
    /// <returns>The created user</returns>
    Task<UserDto> CreateUserAsync(UserDto userDto);

    /// <summary>
    /// Deletes a user by its identifier.
    /// </summary>
    /// <param name="id">The identifier</param>
    /// <returns>If operation finished sucessfully</returns>
    Task<bool> DeleteUserAsync(int id);

    /// <summary>
    /// Patches the user.
    /// </summary>
    /// <param name="id">The user identifier</param>
    /// <param name="patchingDocument">The patching document</param>
    /// <returns>The updated user</returns>
    Task<UserDto?> PatchUserAsync(int id, JsonPatchDocument<UserDto> patchingDocument);

    /// <summary>
    /// Updates the state of a user.
    /// </summary>
    /// <param name="id">The user identifier</param>
    /// <param name="newState">The newState</param>
    /// <returns>The updated user</returns>
    Task<UserDto?> UpdateUserStateAsync(int id, UserStateEnumDto newState);
}

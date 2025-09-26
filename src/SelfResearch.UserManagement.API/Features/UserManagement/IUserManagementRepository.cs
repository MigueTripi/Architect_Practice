using System;

namespace SelfResearch.UserManagement.API.Features.UserManagement;

public interface IUserManagementRepository
{
    /// <summary>
    /// Gets a user by its identifier.
    /// </summary>
    /// <param name="id">The user identifier</param>
    /// <returns>The user</returns>
    Task<User?> GetUserAsync(int id);


    /// <summary>
    /// Gets a paged list of users.
    /// </summary>
    /// <param name="skip">The quantity to bypass</param>
    /// <param name="take">The quantity to return</param>
    /// <returns>The paged users</returns>
    Task<List<User>> GetPagedUsersAsync(int skip, int take);

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="user">The user to create</param>
    /// <returns>The created user</returns>
    Task<User> CreateUserAsync(User user);

    /// <summary>
    /// Updates the user.
    /// </summary>
    /// <returns>The Task</returns>
    Task UpdateUserAsync();

    /// <summary>
    /// Deletes a user by its identifier.
    /// </summary>
    /// <param name="id">The identifier</param>
    /// <returns>The task</returns>
    Task DeleteUserAsync(int id);
}

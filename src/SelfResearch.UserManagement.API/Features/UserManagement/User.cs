using System;

namespace SelfResearch.UserManagement.API.Features.UserManagement;

public class User
{
    /// <summary>
    /// The user identifier
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// The user name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>   
    /// The user email
    /// </summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// The user state
    /// </summary>
    public UserStateEnum State { get; set; } = UserStateEnum.Active;
}

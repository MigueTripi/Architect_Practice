namespace SelfResearch.UserManagement.API.Features.UserManagement;

/// <summary>
/// DTO for updating the state of a user.
/// </summary>
public class UserStateUpdateRequestDto
{
    /// <summary>
    /// The new state of the user.
    /// </summary>
    public UserStateEnumDto UserState { get; set; }
}

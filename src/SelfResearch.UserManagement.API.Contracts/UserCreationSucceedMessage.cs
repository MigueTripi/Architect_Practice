namespace SelfResearch.UserManagement.API.Contracts;

public class UserCreationSucceedMessage: IEvent
{
    /// <summary>
    /// The user identifier
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// The user state
    /// </summary>
    public int State { get; set; }
}
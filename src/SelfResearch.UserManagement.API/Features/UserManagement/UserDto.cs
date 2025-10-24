using System.Text.Json.Serialization;

namespace SelfResearch.UserManagement.API.Features.UserManagement;

/// <summary>
/// The user dto
/// </summary>
public class UserDto
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
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserStateEnumDto State { get; set; }
}

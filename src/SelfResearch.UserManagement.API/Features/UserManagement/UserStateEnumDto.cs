using System.Text.Json.Serialization;

namespace SelfResearch.UserManagement.API.Features.UserManagement;

[JsonConverter(typeof(JsonStringEnumConverter<UserStateEnumDto>))]
public enum UserStateEnumDto
{
    Initial = 1,
    Inactive = 2,
    Blocked = 3,
    Active = 10,
}

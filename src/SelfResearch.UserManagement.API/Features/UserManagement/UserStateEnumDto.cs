using System.Text.Json.Serialization;

namespace SelfResearch.UserManagement.API.Features.UserManagement;

[JsonConverter(typeof(JsonStringEnumConverter<UserStateEnumDto>))]
public enum UserStateEnumDto
{
    Active = 1,
    Inactive = 2,
    Suspended = 3
}

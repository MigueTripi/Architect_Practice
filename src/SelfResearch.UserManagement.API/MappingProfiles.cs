
using AutoMapper;
using SelfResearch.UserManagement.API.Features.UserManagement;
namespace SelfResearch.UserManagement.API.Mapping;

/// <summary>
/// Mapping profiles
/// </summary>
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<UserStateEnum, UserStateEnumDto>().ReverseMap();
    }
}
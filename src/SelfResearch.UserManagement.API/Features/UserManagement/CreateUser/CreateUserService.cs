using AutoMapper;

namespace SelfResearch.UserManagement.API.Features.UserManagement.CreateUser;

public class CreateUserService : ICreateUserService
{
    private readonly IUserManagementRepository _userManagementRepository;
    private readonly IMapper _mapper;

    public CreateUserService(
        IUserManagementRepository userManagementRepository,
        IMapper mapper)
    {
        _userManagementRepository = userManagementRepository;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<UserDto> CreateUserAsync(UserDto userDto)
    {
        var user = await this._userManagementRepository.CreateUserAsync(this._mapper.Map<User>(userDto));
        return this._mapper.Map<UserDto>(user);
    }

}

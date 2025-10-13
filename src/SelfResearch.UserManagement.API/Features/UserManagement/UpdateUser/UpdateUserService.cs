using AutoMapper;

namespace SelfResearch.UserManagement.API.Features.UserManagement.CreateUser;

public class UpdateUserService : IUpdateUserService
{
    private readonly IUserManagementRepository _userManagementRepository;
    private readonly IMapper _mapper;

    public UpdateUserService(
        IUserManagementRepository userManagementRepository,
        IMapper mapper)
    {
        _userManagementRepository = userManagementRepository;
        _mapper = mapper;
    }

    /// <inheritdoc/>
        /// <inheritdoc/>
    public async Task<UserDto?> UpdateUserStateAsync(int id, UserStateEnumDto newState)
    {
        var dbUser = await this._userManagementRepository.GetUserAsync(id);
        if (dbUser == null)
        {
            return null;
        }

        //TODO: Add validation of state transitions

        dbUser.State = (UserStateEnum)newState;
        await this._userManagementRepository.UpdateUserAsync();

        //TODO: Publish service bus event

        return this._mapper.Map<UserDto>(dbUser);
    }

}

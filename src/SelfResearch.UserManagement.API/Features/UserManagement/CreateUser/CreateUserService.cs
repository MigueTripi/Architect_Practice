using AutoMapper;
using FluentResults;
using SelfResearch.Core.Infraestructure.ErrorHandling;
using SelfResearch.UserManagement.API.Contracts;

namespace SelfResearch.UserManagement.API.Features.UserManagement.CreateUser;

public class CreateUserService : ICreateUserService
{
    private readonly IUserManagementRepository _userManagementRepository;
    private readonly IMapper _mapper;
    private readonly IMessageSession _messageSession;

    public CreateUserService(
        IUserManagementRepository userManagementRepository,
        IMapper mapper,
        IMessageSession messageSession)
    {
        _userManagementRepository = userManagementRepository;
        _mapper = mapper;
        _messageSession = messageSession;
    }

    /// <inheritdoc/>
    public async Task<Result<UserDto>> CreateUserAsync(UserDto userDto)
    {
        if (userDto.Id != 0)
        {
            return Result.Fail(new ArgumentError(nameof(userDto.Id), "User ID must be zero for creation."));
        }

        userDto.State = UserStateEnumDto.Initial;
        var user = await this._userManagementRepository.CreateUserAsync(this._mapper.Map<User>(userDto));
        var result = this._mapper.Map<UserDto>(user);

        await _messageSession.Publish(new UserCreationSucceedMessage
        {
            UserId = user.Id,
            State = (int)user.State
        });

        return Result.Ok(result);
    }

}

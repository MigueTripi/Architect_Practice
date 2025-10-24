using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.JsonPatch;
using SelfResearch.Core.Infraestructure.ErrorHandling;

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
    public async Task<Result<UserDto>> UpdateUserStateAsync(int id, UserStateEnumDto newState)
    {
        var dbUser = await this._userManagementRepository.GetUserAsync(id);
        if (dbUser == null)
        {
            return Result.Fail(new NotFoundError(id.ToString(), nameof(UserDto)));
        }

        //TODO: Add validation of state transitions

        dbUser.State = (UserStateEnum)newState;
        await this._userManagementRepository.UpdateUserAsync();

        //TODO: Publish service bus event

        return this._mapper.Map<UserDto>(dbUser);
    }

    /// <inheritdoc/>
    public async Task<Result<UserDto>> PatchUserAsync(int id, JsonPatchDocument<UserDto> patchingDocument)
    {
        if (patchingDocument.Operations.Any(x=> x.path.Contains("State")))
        {
            return Result.Fail(new ArgumentError(nameof(patchingDocument), "State field cannot be modified via patching."));
        }
        var dbUser = await this._userManagementRepository.GetUserAsync(id);
        if (dbUser == null)
        {
            return Result.Fail(new NotFoundError(id.ToString(), nameof(UserDto)));
        }

        var userDto = this._mapper.Map<UserDto>(dbUser);
        patchingDocument.ApplyTo(userDto);

        this._mapper.Map(userDto, dbUser);

        await this._userManagementRepository.UpdateUserAsync();
        return Result.Ok(userDto);
    }

}

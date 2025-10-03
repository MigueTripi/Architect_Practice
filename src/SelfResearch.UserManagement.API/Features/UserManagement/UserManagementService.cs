using System;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace SelfResearch.UserManagement.API.Features.UserManagement;

public class UserManagementService : IUserManagementService
{
    private readonly IUserManagementRepository _userManagementRepository;
    private readonly IMapper _mapper;

    public UserManagementService(
        IUserManagementRepository userManagementRepository,
        IMapper mapper)
    {
        _userManagementRepository = userManagementRepository;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<UserDto?> GetUserAsync(int id)
    {
        var dbUser = await _userManagementRepository.GetUserAsync(id);

        return this._mapper.Map<UserDto>(dbUser);
    }

    /// <inheritdoc/>
    public async Task<List<UserDto>> GetPagedUsersAsync(int skip, int take)
    {
        var users = await this._userManagementRepository.GetPagedUsersAsync(skip, take);
        return this._mapper.Map<List<UserDto>>(users);
    }

    /// <inheritdoc/>
    public async Task<UserDto?> PatchUserAsync(int id, JsonPatchDocument<UserDto> patchingDocument)
    {
        var dbUser = await this._userManagementRepository.GetUserAsync(id);
        if (dbUser == null)
        {
            return null;
        }

        var userDto = this._mapper.Map<UserDto>(dbUser);
        patchingDocument.ApplyTo(userDto);

        this._mapper.Map(userDto, dbUser);

        await this._userManagementRepository.UpdateUserAsync();
        return userDto;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteUserAsync(int id)
    {
        var dbUser = await this._userManagementRepository.GetUserAsync(id);
        if (dbUser == null)
        {
            return false;
        }

        await this._userManagementRepository.DeleteUserAsync(id);
        return true;
    }

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

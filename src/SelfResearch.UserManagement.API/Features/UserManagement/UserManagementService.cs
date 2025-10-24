using System;
using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.JsonPatch;
using SelfResearch.Core.Infraestructure.ErrorHandling;

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
    public async Task<Result<UserDto?>> GetUserAsync(int id)
    {
        var dbUser = await _userManagementRepository.GetUserAsync(id);
        if (dbUser == null)
        {
            return Result.Fail(new NotFoundError(id.ToString(), nameof(UserDto)));
        }

        return this._mapper.Map<UserDto>(dbUser);
    }

    /// <inheritdoc/>
    public async Task<List<UserDto>> GetPagedUsersAsync(int skip, int take)
    {
        var users = await this._userManagementRepository.GetPagedUsersAsync(skip, take);
        return this._mapper.Map<List<UserDto>>(users);
    }

    /// <inheritdoc/>
    public async Task<Result<bool>> DeleteUserAsync(int id)
    {
        var dbUser = await this._userManagementRepository.GetUserAsync(id);
        if (dbUser == null)
        {
            return Result.Fail(new NotFoundError(id.ToString(), nameof(UserDto)));
        }

        await this._userManagementRepository.DeleteUserAsync(id);
        return Result.Ok(true);
    }
}

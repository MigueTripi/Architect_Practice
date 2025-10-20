using SelfResearch.UserManagement.API.Features.UserManagement;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SelfResearch.UserManagement.API.Features.UserManagement.CreateUser;
using FluentResults;
using SelfResearch.Core.Infraestructure.ErrorHandling;
using FluentResults.Extensions.AspNetCore;

namespace SelfResearch.UserManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserManagementService _userManagementService;
        private readonly ICreateUserService _createUserService;
        private readonly IUpdateUserService _updateUserService;

        public UserController(
            ILogger<UserController> logger,
            IUserManagementService userManagementService,
            ICreateUserService createUserService,
            IUpdateUserService updateUserService)
        {
            _logger = logger;
            _userManagementService = userManagementService;
            _createUserService = createUserService;
            _updateUserService = updateUserService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            if (id <= 0)
            {
                return Result.Fail(new ArgumentError(nameof(id), "Invalid user ID.")).ToActionResult();
            }

            var result = await _userManagementService.GetUserAsync(id);

            return result.ToCustomActionResult()!;
            }

        [HttpGet("GetPaged")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<UserDto>>> GetPagedUsers(int skip = 1, int take = 10)
        {
            if (skip < 0)
            {
                return Result.Fail(new ArgumentError(nameof(skip), "Skip must be non-negative.")).ToActionResult();
            }

            if (take <= 0)
            {
                return Result.Fail(new ArgumentError(nameof(take), "Take must be a positive integer.")).ToActionResult();
            }

            var result = await this._userManagementService.GetPagedUsersAsync(skip, take);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] UserDto user)
        {
            if (user == null)
            {
                return Result.Fail(new ArgumentError(nameof(user), "User data is required.")).ToActionResult();
            }
            if (user.Id != 0)
            {
                return Result.Fail(new ArgumentError(nameof(user.Id), "User ID must be zero for creation.")).ToActionResult();
            }

            var newUserResult = await this._createUserService.CreateUserAsync(user);

            return newUserResult!.ToCustomActionResult(
                CreatedAtAction(nameof(GetUserById), new { id = newUserResult.Value.Id }, newUserResult.Value)
            )!;
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> PatchUser(int id, [FromBody] JsonPatchDocument<UserDto> patchingDocument)
        {
            if (id <= 0)
            {
                return Result.Fail(new ArgumentError(nameof(id), "Invalid user ID.")).ToActionResult();
            }
            if (patchingDocument == null)
            {
                return Result.Fail(new ArgumentError(nameof(patchingDocument), "Patching document is required.")).ToActionResult();
            }

            var updatedUserResult = await this._userManagementService.PatchUserAsync(id, patchingDocument);
            return updatedUserResult.ToActionResult();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> DeleteUser(int id)
        {
            if (id <= 0)
            {
                return Result.Fail(new ArgumentError(nameof(id), "Invalid user ID.")).ToActionResult();
            }
            var result = await this._userManagementService.DeleteUserAsync(id);
            return result.ToActionResult();
        }

        [HttpPut("{id}/state")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> UpdateUserState(int id, [FromBody] UserStateUpdateRequestDto stateUpdate)
        {
            if (id <= 0)
            {
                return Result.Fail(new ArgumentError(nameof(id), "Invalid user ID.")).ToActionResult();
            }
            if (stateUpdate == null)
            {
                return Result.Fail(new ArgumentError(nameof(stateUpdate), "State update data is required.")).ToActionResult();
            }

            var updatedUserResult = await this._updateUserService.UpdateUserStateAsync(id, stateUpdate.UserState);
            return updatedUserResult.ToActionResult();
        }
    }
}

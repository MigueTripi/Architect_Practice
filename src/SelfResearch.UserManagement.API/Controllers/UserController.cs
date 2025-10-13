using System.Threading.Tasks;
using SelfResearch.UserManagement.API.Features.UserManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SelfResearch.UserManagement.API.Features.UserManagement.CreateUser;

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
                return BadRequest("Invalid user ID.");
            }

            var user = await _userManagementService.GetUserAsync(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }

        [HttpGet("GetPaged")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<UserDto>>> GetPagedUsers(int skip = 1, int take = 10)
        {
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
                return BadRequest("User data is required.");
            }
            if (user.Id != 0)
            {
                return BadRequest("User ID must be zero for creation.");
            }

            var newUser = await this._createUserService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> PatchUser(int id, [FromBody] JsonPatchDocument<UserDto> patchingDocument)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid user ID.");
            }
            if (patchingDocument == null)
            {
                return BadRequest("Patching document is required.");
            }

            var updatedUser = await this._userManagementService.PatchUserAsync(id, patchingDocument);
            if (updatedUser == null)
            {
                return NotFound("User not found.");
            }

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteUser(int id)
        {
            return Ok(await this._userManagementService.DeleteUserAsync(id));
        }

        [HttpPut("{id}/state")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> UpdateUserState(int id, [FromBody] UserStateUpdateRequestDto stateUpdate)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid user ID.");
            }
            if (stateUpdate == null)
            {
                return BadRequest("State update data is required.");
            }

            var updatedUser = await this._updateUserService.UpdateUserStateAsync(id, stateUpdate.UserState);
            if (updatedUser == null)
            {
                return NotFound("User not found.");
            }

            return Ok(updatedUser);
        }
    }
}

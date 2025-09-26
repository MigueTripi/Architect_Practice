using System.Threading.Tasks;
using SelfResearch.UserManagement.API.Features.UserManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace SelfResearch.UserManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserManagementService _userManagementService;

        public UserController(
            ILogger<UserController> logger,
            IUserManagementService userManagementService)
        {
            _logger = logger;
            _userManagementService = userManagementService;
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

            var newUser = await this._userManagementService.CreateUserAsync(user);
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
            return await this._userManagementService.DeleteUserAsync(id);
        }
    }
}

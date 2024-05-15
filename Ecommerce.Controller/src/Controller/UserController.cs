using System.Security.Claims;
using Ecommerce.Core.src.Common;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private IAuthorizationService _authorizationService;

        public UserController(IUserService userService, IAuthorizationService authorizationService)
        {
            _userService = userService;
            _authorizationService = authorizationService;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet] // endpoint: /users
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAllUsersAsync([FromQuery] UserQueryOptions userQueryOptions)
        {
            var users = await _userService.GetAllUsersAsync(userQueryOptions);
            if (users == null || !users.Any())
            {
                return NotFound("No users found.");
            }
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{userId}")] // endpoint: /users/:user_id
        public async Task<ActionResult<UserReadDto>> GetUserByIdAsync([FromRoute] Guid userId)
        {
            var foundUser = await _userService.GetUserByIdAsync(userId);
            if (foundUser == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }
            return Ok(foundUser);
        }

        [AllowAnonymous]
        [HttpPost()] // endpoint: /users
                     // public async Task<UserReadDto> CreateUserAsync([FromBody] UserCreateDto userCreateDto)
        public async Task<ActionResult<UserReadDto>> CreateUserAsync([FromBody] UserCreateDto userCreateDto)
        {
            var createdUser = await _userService.CreateUserAsync(userCreateDto);
            if (createdUser == null)
            {
                return BadRequest("Failed to create user.");
            }
            return Ok(createdUser);
        }

        // only user itself can update the user info
        // resource-based authorization : data need to retrived from data resource to be verified
        [Authorize(/* Policy = "ResourceOwner" */)]
        [HttpPut("{userId}")] // endpoint: /users/:user_id
        public async Task<ActionResult<UserReadDto>> UpdateUserByIdAsync([FromRoute] Guid userId, [FromBody] UserUpdateDto userUpdateDto)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }
            var authResult = await _authorizationService.AuthorizeAsync(HttpContext.User, user, "ResourceOwner");
            if (!authResult.Succeeded)
            {
                throw AppException.Unauthorized("No permission.");
            }
            var updatedUser = await _userService.UpdateUserByIdAsync(userId, userUpdateDto);
            if (updatedUser == null)
            {
                return BadRequest("Failed to update user.");
            }
            return Ok(updatedUser);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{userId}")] // endpoint: /users/:user_id
        public async Task<ActionResult<bool>> DeleteUserByIdAsync([FromRoute] Guid userId)
        {
            var deleted = await _userService.DeleteUserByIdAsync(userId);
            if (!deleted)
            {
                return NotFound($"User with ID {userId} not found.");
            }
            return Ok(true);
        }


        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<UserReadDto>> GetUserProfileAsync()
        {
            var authenticatedClaims = HttpContext.User;
            var userId = authenticatedClaims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            if (userId == null)
            {
                return BadRequest("User ID not found in claims.");
            }
            var foundUserProfile = await _userService.GetUserByIdAsync(Guid.Parse(userId));
            if (foundUserProfile == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }
            return Ok(foundUserProfile);
        }

    }
}

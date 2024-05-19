using System.Security.Claims;
using Ecommerce.Core.src.Common;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IAuthorizationService authorizationService, IUserService userService)
        {
            _authService = authService;
            _authorizationService = authorizationService;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginAsync([FromBody] UserCredential userCredential)
        {
            var token = await _authService.LoginAsync(userCredential); 
            if (token == null)
            {
                return Unauthorized("Invalid username or password");
            }
            return Ok(token);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> LogoutAsync()
        {
            // Retrieve token from the Authorization header
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            // Console.WriteLine(token);

            // Check if the token exists
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is missing");
            }

            // Call the logout method
            var result = await _authService.LogoutAsync();
            if (result == "removed")
            {
                return Ok("Logged out successfully");
            }
            else
            {
                return Ok("User already logout");
            }

        }

        // logged in user or Admin
        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<UserReadDto>> GetCurrnentProfileAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await _userService.GetUserByIdAsync(Guid.Parse(userId));
            var authResult = await _authorizationService.AuthorizeAsync(HttpContext.User, user, "AdminOrOwnerAccount");

            if (!authResult.Succeeded)
            {
                return Forbid();
            }

            var profile = await _authService.GetCurrentProfileAsync(user.Id);
            if (profile == null)
            {
                return NotFound("Profile not found");
            }
            return Ok(profile);
        }

    }
}
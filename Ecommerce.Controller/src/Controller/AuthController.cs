using Ecommerce.Core.src.Common;
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

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<string> LoginAsync([FromBody] UserCredential userCredential)
        {
            return await _authService.LoginAsync(userCredential);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
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

    }
}
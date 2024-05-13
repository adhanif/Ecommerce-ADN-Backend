using System.Security.Claims;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Service.src.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.WebAPI.src.AuthorizationPolicy
{
    #region Requirements
    public class AdminOrOwnerAccountRequirement : IAuthorizationRequirement
    {
        public AdminOrOwnerAccountRequirement()
        {

        }
    }
    #endregion

    #region Rules Handler
    public class AdminOrOwnerAccountHandler : AuthorizationHandler<AdminOrOwnerAccountRequirement, UserReadDto>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminOrOwnerAccountRequirement requirement, UserReadDto user)
        {
            var claims = context.User.Claims;
            var userRole = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == user.Id.ToString() || userRole == UserRole.Admin.ToString())
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
    #endregion
}
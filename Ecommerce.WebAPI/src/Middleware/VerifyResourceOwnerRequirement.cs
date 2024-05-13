
using System.Security.Claims;
using Ecommerce.Core.src.Entity;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.WebAPI.src.Middleware
{
    public class VerifyResourceOwnerRequirement : IAuthorizationRequirement
    {
        public VerifyResourceOwnerRequirement()
        {
        }
    }

    public class VerifyResourceOwnerHandler : AuthorizationHandler<VerifyResourceOwnerRequirement, User>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, VerifyResourceOwnerRequirement requirement, User resource)
        {
            var claims = context.User.Claims;
            var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value; // id of authenticated user
            if (userId == resource.Id.ToString())
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}

// Delete this class soon! -> Move to folder AuthorizationPolicy
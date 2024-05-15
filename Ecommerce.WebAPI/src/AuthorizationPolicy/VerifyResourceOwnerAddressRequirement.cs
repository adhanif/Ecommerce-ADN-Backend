using System.Security.Claims;
using Ecommerce.Core.src.Entity;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.WebAPI.src.AuthorizationPolicy
{
    public class VerifyResourceOwnerAddressRequirement : IAuthorizationRequirement
    {
        public VerifyResourceOwnerAddressRequirement()
        {

        }
    }

    class VerifyResourceOwnerAddressHandler : AuthorizationHandler<VerifyResourceOwnerAddressRequirement, Address>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, VerifyResourceOwnerAddressRequirement requirement, Address resource)
        {
            var claims = context.User.Claims;
            var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value; // id of authenticated user
            if (userId == resource.UserId.ToString())
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
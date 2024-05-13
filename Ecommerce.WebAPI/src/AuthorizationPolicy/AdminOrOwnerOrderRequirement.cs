using System.Security.Claims;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Service.src.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.WebAPI.src.AuthorizationPolicy
{
    public class AdminOrOwnerOrderRequirement : IAuthorizationRequirement
    {
        public AdminOrOwnerOrderRequirement()
        {
        }
    }

    public class AdminOrOwnerOrderHandler : AuthorizationHandler<AdminOrOwnerAccountRequirement, OrderReadDto>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminOrOwnerAccountRequirement requirement, OrderReadDto order)
        {
            var claims = context.User.Claims;
            var userRole = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == order.User.Id.ToString() || userRole == UserRole.Admin.ToString())
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
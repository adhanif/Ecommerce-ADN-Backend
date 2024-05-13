using System.Security.Claims;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Service.src.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.WebAPI.src.AuthorizationPolicy
{
    public class AdminOrOwnerReviewRequirement : IAuthorizationRequirement { }

    public class AdminOrOwnerReviewHandler : AuthorizationHandler<AdminOrOwnerReviewRequirement, ReviewReadDto>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminOrOwnerReviewRequirement requirement, ReviewReadDto review)
        {
            var claims = context.User.Claims;
            var userRole = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == review.User.Id.ToString() || userRole == UserRole.Admin.ToString())
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
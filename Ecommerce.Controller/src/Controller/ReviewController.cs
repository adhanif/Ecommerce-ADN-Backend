using System.Security.Claims;
using Ecommerce.Core.src.Common;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/reviews")]
    public class ReviewController : ControllerBase
    {
        private IReviewService _service;
        private IAuthorizationService _authorizationService;
        public ReviewController(IReviewService service, IAuthorizationService authorizationService)
        {
            _service = service;
            _authorizationService = authorizationService;
        }

        [AllowAnonymous]
        [HttpGet()]
        public async Task<IEnumerable<ReviewReadDto>> GetAllReviewsAsync([FromQuery] BaseQueryOptions options)
        {
            return await _service.GetAllReviewsAsync(options); // Will be modified later
        }

        [HttpGet("product/{productId}")]
        public async Task<IEnumerable<ReviewReadDto>> GetAllReviewsOfProductAsync([FromQuery] Guid productId)
        {
            return await _service.GetAllReviewsOfProductAsync(productId); // Will be modified later
        }

        // Customer auth = CreateAReview's Customer auth or Admin
        [Authorize]
        [HttpGet("{reviewId}")]
        public async Task<ActionResult<ReviewReadDto>> GetReviewByIdAsync([FromRoute] Guid reviewId)
        {

            var review = await _service.GetReviewByIdAsync(reviewId);
            var authResult = await _authorizationService.AuthorizeAsync(HttpContext.User, review, "AdminOrOwnerReview");

            if (!authResult.Succeeded)
            {
                return Forbid();
            }

            if (review == null)
            {
                return NotFound();
            }

            return Ok(review);
        }

        [Authorize]
        [HttpPost()]
        public async Task<ReviewReadDto> CreateReviewAsync([FromBody] ReviewCreateDto reviewCreateDto)
        {
            var userId = GetUserIdClaim();
            return await _service.CreateReviewAsync(userId, reviewCreateDto); // Will be modified later
        }

        // Customer auth = CreateAReview's Customer auth or Admin
        // Implement for admin first
        [Authorize(Roles = "Admin")]
        [HttpPatch("{reviewId}")]
        public async Task<ReviewReadDto> UpdateReviewByIdAsync([FromRoute] Guid reviewId, [FromBody] ReviewUpdateDto reviewUpdateDto)
        {
            reviewUpdateDto.ReviewId = reviewId; // If review is found...
            return await _service.UpdateReviewByIdAsync(reviewId, reviewUpdateDto); // Will be modified later
        }

        // Customer auth = CreateAReview's Customer auth or Admin
        // Implement for admin first
        [Authorize(Roles = "Admin")]
        [HttpDelete("{reviewId}")]
        public async Task<bool> DeleteReviewByIdAsync([FromRoute] Guid reviewId)
        {
            return await _service.DeleteReviewByIdAsync(reviewId); // Will be modified later
        }

        private Guid GetUserIdClaim()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new Exception("User ID claim not found");
            }
            if (!Guid.TryParse(userIdClaim.Value, out var userId))
            {
                throw new Exception("Invalid user ID format");
            }
            return userId;
        }
    }
}
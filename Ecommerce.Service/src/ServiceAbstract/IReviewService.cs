using Ecommerce.Core.src.Common;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstract
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewReadDto>> GetAllReviewsAsync(BaseQueryOptions options); // Admin auth
        Task<IEnumerable<ReviewReadDto>> GetAllReviewsOfProductAsync(Guid productId); // Customer auth
        Task<ReviewReadDto> GetReviewByIdAsync(Guid reviewId); // Customer auth
        Task<ReviewReadDto> CreateReviewAsync(Guid userId, ReviewCreateDto reviewCreateDto); // Customer auth
        Task<ReviewReadDto> UpdateReviewByIdAsync(Guid reviewId, ReviewUpdateDto reviewUpdateDto); // Customer auth = CreateAReview's Customer auth
        Task<bool> DeleteReviewByIdAsync(Guid reviewId); // Customer auth = CreateAReview's Customer auth
    }
}
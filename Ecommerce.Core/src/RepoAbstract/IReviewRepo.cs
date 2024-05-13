using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;

namespace Ecommerce.Core.src.RepoAbstract
{
    public interface IReviewRepo
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync(BaseQueryOptions options); // Admin auth
        Task<IEnumerable<Review>> GetAllReviewsOfProductAsync(Guid productId); // Customer auth
        Task<Review> GetReviewByIdAsync(Guid reviewId); // Customer auth
        Task<Review> CreateReviewAsync(Review newReview); // Customer auth
        Task<Review> UpdateReviewByIdAsync(Review updatedReview); // Customer auth = CreateAReview's Customer auth
        Task<bool> DeleteReviewByIdAsync(Guid reviewId); // Customer auth = CreateAReview's Customer auth
    }
}
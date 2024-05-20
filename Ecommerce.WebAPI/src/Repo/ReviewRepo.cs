using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstract;
using Ecommerce.WebAPI.src.Database;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebAPI.src.Repo
{
    public class ReviewRepo : IReviewRepo
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Review> _reviews;

        public ReviewRepo(AppDbContext context)
        {
            _context = context;
            _reviews = _context.Reviews;
        }

        public async Task<Review> CreateReviewAsync(Review newReview)
        {
            var review = await _reviews.AddAsync(newReview);
            await _context.SaveChangesAsync();
            return review.Entity;
        }

        public async Task<bool> DeleteReviewByIdAsync(Guid reviewId)
        {
            var foundReview = await _reviews.FindAsync(reviewId);
            if (foundReview is null)
            {
                return false;
            }

            _reviews.Remove(foundReview);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync(BaseQueryOptions options)
        {
            var query = _reviews.AsQueryable();
            // Pagination
            if (options is not null)
            {
                query = query.OrderBy(r => r.CreatedDate)
                             .Include(r => r.Product)
                             .Include(r => r.User);
                            //  .Skip(options.Offset)
                            //  .Take(options.Limit);
            }

            var reviews = await query.ToListAsync();
            return reviews;
        }

        public async Task<IEnumerable<Review>> GetAllReviewsOfProductAsync(Guid productId)
        {
            var query = _reviews.AsQueryable();
            query = query
                .Include(r => r.Product)
                .Include(r => r.User)
                .Where(r => r.Product.Id == productId);

            var reviews = await query.ToListAsync();
            return reviews;
        }

        public async Task<Review> GetReviewByIdAsync(Guid reviewId)
        {
            var review = await _reviews
                .Include(r => r.Product)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == reviewId);

            return review;
        }

        public async Task<Review> UpdateReviewByIdAsync(Review updatedReview)
        {
            _reviews.Update(updatedReview);
            await _context.SaveChangesAsync();
            return updatedReview;
        }
    }
}
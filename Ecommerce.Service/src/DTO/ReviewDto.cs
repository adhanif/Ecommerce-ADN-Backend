using System.ComponentModel.DataAnnotations;
using Ecommerce.Core.src.Entity;

namespace Ecommerce.Service.src.DTO
{
    public class ReviewReadDto : BaseEntity
    {
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public float ReviewRating { get; set; }
        public string? ReviewContent { get; set; }
        public Guid UserId { get; set; }
        public UserReadDto User { get; set; }
        public Guid ProductId { get; set; }
        public ProductReviewReadDto Product { get; set; }
        // This line is for user information - public UserReadDTO User { get; set; }
        public void Transform(Review review)
        {
            ReviewRating = review.Rating;
            ReviewContent = review.Content;
            ProductId = review.ProductId;
            UserId = review.UserId;
        }
    }
    public class ReviewCreateDto : BaseEntity
    {
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public float ReviewRating { get; set; }
        public string? ReviewContent { get; set; }
        public Guid ProductId { get; set; }
    }
    public class ReviewUpdateDto
    {
        public Guid ReviewId { get; set; }
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public float? ReviewRating { get; set; }
        public string? ReviewContent { get; set; }
    }
}
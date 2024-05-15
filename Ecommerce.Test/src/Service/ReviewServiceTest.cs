using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstract;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.Service;
using Ecommerce.Service.src.Shared;
using Moq;
using Xunit;

namespace Ecommerce.Test.src.Service
{
    public class ReviewServiceTest
    {
        private static IMapper _mapper;
        public ReviewServiceTest()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(m =>
                {
                    m.AddProfile(new MapperProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [Fact]
        public void GetAllReviewsAsync_ShouldReturnAllReviewReadDtos()
        {
            // Arrange
            var mockReviewRepo = new Mock<IReviewRepo>();
            var mockMapper = new Mock<IMapper>();
            var reviewService = new ReviewService(mockReviewRepo.Object, _mapper);

            var reviews = new List<Review>
            {
                new Review
                {
                    Id = Guid.NewGuid(),
                    Content = "content",
                    Rating = 5,
                    ProductId = Guid.NewGuid(),
                    UserId = Guid.NewGuid()
                },
                new Review
                {
                    Id = Guid.NewGuid(),
                    Content = "content",
                    Rating = 5,
                    ProductId = Guid.NewGuid(),
                    UserId = Guid.NewGuid()
                }
            };

            var reviewDtos = new List<ReviewReadDto>
            {
                new ReviewReadDto { Id = reviews[0].Id },
                new ReviewReadDto { Id = reviews[1].Id }
            };


            mockReviewRepo.Setup(reviewRepo => reviewRepo.GetAllReviewsAsync(It.IsAny<BaseQueryOptions>())).Returns(Task.FromResult<IEnumerable<Review>>(reviews));
            mockMapper.Setup(mapper => mapper.Map<Review, ReviewReadDto>(It.IsAny<Review>())).Returns((Review review) => reviewDtos.FirstOrDefault(dto => dto.Id == review.Id));

            // Act
            var result = reviewService.GetAllReviewsAsync(new BaseQueryOptions());

            // Assert
            Assert.IsType<Task<IEnumerable<ReviewCreateDto>>>(result);
            Assert.NotNull(result.Result);
            Assert.Equal(reviews.Count, result.Result.Count());
        }

        [Fact]
        public void GetAllReviewsOfProductAsync_ValidProductId_ShoudlReturnReviewReadDtoListOfProduct()
        {
            // Arrange
            var mockReviewRepo = new Mock<IReviewRepo>();
            var mockMapper = new Mock<IMapper>();
            var reviewService = new ReviewService(mockReviewRepo.Object, _mapper);

            var productId = Guid.NewGuid();
            var reviews = new List<Review>
            {
                new Review
                {
                    Id = Guid.NewGuid(),
                    Content = "content 1",
                    Rating = 5,
                    ProductId = productId,
                    UserId = Guid.NewGuid()
                },
                new Review
                {
                    Id = Guid.NewGuid(),
                    Content = "content 2",
                    Rating = 5,
                    ProductId = productId,
                    UserId = Guid.NewGuid()
                }
            };

            var reviewDtos = new List<ReviewReadDto>
            {
                new ReviewReadDto { Id = reviews[0].Id },
                new ReviewReadDto { Id = reviews[1].Id }
            };

            mockReviewRepo.Setup(reviewRepo => reviewRepo.GetAllReviewsOfProductAsync(productId)).Returns(Task.FromResult<IEnumerable<Review>>(reviews));
            mockMapper.Setup(mapper => mapper.Map<Review, ReviewReadDto>(It.IsAny<Review>())).Returns((Review review) => reviewDtos.FirstOrDefault(dto => dto.Id == review.Id));

            // Act
            var result = reviewService.GetAllReviewsOfProductAsync(productId);

            // Assert
            Assert.NotNull(result.Result);
            Assert.Equal(reviews.Count, result.Result.Count());
        }

        [Fact]
        public void GetReviewByIdAsync_ValidReviewId_ShouldReturnReviewReadDto()
        {
            // Arrange
            var mockReviewRepo = new Mock<IReviewRepo>();
            var mockMapper = new Mock<IMapper>();
            var reviewService = new ReviewService(mockReviewRepo.Object, _mapper);

            var reviewId = Guid.NewGuid();
            var review = new Review { Id = reviewId };
            var reviewDto = new ReviewReadDto { Id = reviewId };

            mockReviewRepo.Setup(reviewRepo => reviewRepo.GetReviewByIdAsync(reviewId)).Returns(Task.FromResult(review));
            mockMapper.Setup(mapper => mapper.Map<ReviewReadDto>(It.IsAny<Review>())).Returns(reviewDto);

            // Act
            var result = reviewService.GetReviewByIdAsync(reviewId);

            // Assert
            Assert.IsType<Task<ReviewReadDto>>(result);
            Assert.NotNull(result.Result);
            Assert.Equal(reviewId, result.Result.Id);
        }

        [Fact]
        public void CreateReviewAsync_ValidInput_ShouldReturnReviewReadDto()
        {
            // Arrange
            var mockReviewRepo = new Mock<IReviewRepo>();
            var mockMapper = new Mock<IMapper>();
            var reviewService = new ReviewService(mockReviewRepo.Object, _mapper);

            var reviewCreateDto = new ReviewCreateDto
            {
                ReviewRating = 5,
                ReviewContent = "content",
                ProductId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            var review = new Review
            {
                Rating = reviewCreateDto.ReviewRating,
                Content = reviewCreateDto.ReviewContent,
                ProductId = reviewCreateDto.ProductId,
                UserId = reviewCreateDto.UserId
            };

            var reviewDto = new ReviewReadDto
            {
                ReviewRating = review.Rating,
                ReviewContent = review.Content,
                ProductId = review.ProductId,
                UserId = review.UserId
            };

            mockMapper.Setup(mapper => mapper.Map<Review>(It.IsAny<ReviewCreateDto>())).Returns(review);
            mockReviewRepo.Setup(reviewRepo => reviewRepo.CreateReviewAsync(review)).Returns(Task.FromResult(review));
            mockMapper.Setup(mapper => mapper.Map<ReviewReadDto>(It.IsAny<Review>())).Returns(reviewDto);

            // Act
            var result = reviewService.CreateReviewAsync(reviewCreateDto);

            // Assert
            Assert.IsType<Task<ReviewReadDto>>(result);
            Assert.NotNull(result.Result);
            Assert.Equal(reviewCreateDto.ReviewRating, result.Result.ReviewRating);
            Assert.Equal(reviewCreateDto.ReviewContent, result.Result.ReviewContent);
            Assert.Equal(reviewCreateDto.ProductId, result.Result.ProductId);
            Assert.Equal(reviewCreateDto.UserId, result.Result.UserId);
        }

        [Fact]
        public void UpdateReviewByIdAsync_ValidInput_ShouldReturnReviewReadDto()
        {
            // Arrange  
            var mockReviewRepo = new Mock<IReviewRepo>();
            var mockMapper = new Mock<IMapper>();
            var reviewService = new ReviewService(mockReviewRepo.Object, _mapper);

            var reviewId = Guid.NewGuid();
            var reviewUpdateDto = new ReviewUpdateDto
            {
                ReviewRating = 5,
                ReviewContent = "content"
            };

            var review = new Review
            {
                Id = reviewId,
                Rating = reviewUpdateDto.ReviewRating.Value,
                Content = reviewUpdateDto.ReviewContent
            };

            var reviewDto = new ReviewReadDto
            {
                Id = review.Id,
                ReviewRating = review.Rating,
                ReviewContent = review.Content
            };

            mockMapper.Setup(mapper => mapper.Map<Review>(It.IsAny<ReviewUpdateDto>())).Returns(review);
            mockReviewRepo.Setup(reviewRepo => reviewRepo.UpdateReviewByIdAsync(review)).Returns(Task.FromResult(review));
            mockMapper.Setup(mapper => mapper.Map<ReviewReadDto>(It.IsAny<Review>())).Returns(reviewDto);

            // Act
            var result = reviewService.UpdateReviewByIdAsync(reviewId, reviewUpdateDto);

            // Assert
            Assert.IsType<Task<ReviewReadDto>>(result);
            Assert.NotNull(result.Result);
            Assert.Equal(reviewId, result.Result.Id);
            Assert.Equal(reviewUpdateDto.ReviewRating, result.Result.ReviewRating);
            Assert.Equal(reviewUpdateDto.ReviewContent, result.Result.ReviewContent);
        }

        [Fact]
        public void DeleteReviewByIdAsync_ValidId_ShouldReturnTrue()
        {
            // Arrange
            var mockReviewRepo = new Mock<IReviewRepo>();
            var mockMapper = new Mock<IMapper>();
            var reviewService = new ReviewService(mockReviewRepo.Object, _mapper);

            var reviewId = Guid.NewGuid();
            mockReviewRepo.Setup(reviewRepo => reviewRepo.DeleteReviewByIdAsync(reviewId)).Returns(Task.FromResult(true));

            // Act
            var result = reviewService.DeleteReviewByIdAsync(reviewId);

            // Assert
            Assert.IsType<Task<bool>>(result);
            Assert.True(result.Result);
        }
    }
}
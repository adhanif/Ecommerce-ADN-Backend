
using Xunit;
using Moq;
using Ecommerce.Service.src.Service;
using Ecommerce.Core.src.RepoAbstract;
using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Test.src.Service
{
    public class CategoryServiceTest
    {
        private readonly CategoryService _categoryService;
        private readonly Mock<ICategoryRepo> _categoryRepoMock = new();

        public CategoryServiceTest()
        {
            _categoryService = new CategoryService(_categoryRepoMock.Object);
            InitializeMockData();
        }

        // mock data

        private List<Category> _categories;

        private void InitializeMockData()
        {
            _categories =
            [
                new() { Id = Guid.NewGuid(), Name = "Category 1", Image = "image1.jpg" },
                new() { Id = Guid.NewGuid(), Name = "Category 2", Image = "image2.jpg" },
                new() { Id = Guid.NewGuid(), Name = "Category 3", Image = "image3.jpg" },
                new() { Id = Guid.NewGuid(), Name = "Category 4", Image = "image4.jpg" },
            ];
        }


        [Fact]
        public async Task GetAllCategoriesAsync_ShouldReturnAllCategories()
        {
            // Arrange
            var expectedCategories = _categories;
            _categoryRepoMock.Setup(repo => repo.GetAllCategoriesAsync()).ReturnsAsync(expectedCategories);

            // Act
            var result = await _categoryService.GetAllCategoriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCategories.Count, result.Count());
        }

        [Fact]
        public async Task GetCategoryByIdAsync_ValidId_ShouldReturnCategory()
        {
            // Arrange
            Guid categoryId = Guid.NewGuid();
            Category expectedCategory = new() { Id = categoryId, Name = "Category 1", Image = "image1.jpg" };

            _categoryRepoMock.Setup(repo => repo.GetCategoryByIdAsync(categoryId)).ReturnsAsync(expectedCategory);


            // Act
            var result = await _categoryService.GetCategoryByIdAsync(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCategory.Id, result.CategoryId);
            Assert.Equal(expectedCategory.Name, result.CategoryName);
            Assert.Equal(expectedCategory.Image, result.CategoryImage);
        }

        // will modify later when we have exception handler
        [Fact]
        public async Task GetCategoryByIdAsync_InvalidId_ShouldReturnNull()
        {
            // Arrange
            Guid invalidCategoryId = Guid.NewGuid();
            _categoryRepoMock.Setup(repo => repo.GetCategoryByIdAsync(invalidCategoryId)).ThrowsAsync(new Exception("Category not found"));


            // Act
            var result = await _categoryService.GetCategoryByIdAsync(invalidCategoryId);

            // Assert
            Assert.Null(result);
            await Assert.ThrowsAsync<Exception>(async () => await _categoryService.GetCategoryByIdAsync(invalidCategoryId));
        }

        [Fact]
        public async Task CreateCategoryAsync_ValidData_ShouldCreateCategory()
        {
            // Arrange
            Guid categoryId = Guid.NewGuid();
            var newCategory = new Category { Id = categoryId, Name = "New Category", Image = "image.jpg" };
            var categoryCreateDto = new CategoryCreateDto { CategoryName = newCategory.Name, CategoryImage = newCategory.Image };

            var createdCategory = new CategoryReadDto { CategoryId = categoryId, CategoryName = categoryCreateDto.CategoryName, CategoryImage = categoryCreateDto.CategoryImage };

            _categoryRepoMock.Setup(repo => repo.CreateCategoryAsync(newCategory)).ReturnsAsync(newCategory);


            // Act
            var result = await _categoryService.CreateCategoryAsync(categoryCreateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createdCategory, result);
        }

        [Fact]
        public async Task CreateCategoryAsync_InvalidData_ShouldThrowException()
        {
            // Arrange
            var invalidCategoryCreateDto = new CategoryCreateDto { CategoryName = "", CategoryImage = "" }; // Assuming both properties are required

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _categoryService.CreateCategoryAsync(invalidCategoryCreateDto));
        }

        [Fact]
        public async Task CreateCategoryAsync_DuplicateName_ShouldThrowException()
        {
            // Arrange

            var newCategory = new Category { Id = Guid.NewGuid(), Name = "Existing Category", Image = "image.jpg" };
            var categoryCreateDto = new CategoryCreateDto { CategoryName = newCategory.Name, CategoryImage = newCategory.Image };

            // Act
            _categoryRepoMock.Setup(repo => repo.CreateCategoryAsync(newCategory)).ThrowsAsync(new ArgumentException("Category is duplicated"));

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _categoryService.CreateCategoryAsync(categoryCreateDto));
        }

        [Fact]
        public async Task UpdateCategoryByIdAsync_ValidIdAndData_ShouldUpdateCategory()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var categoryUpdateDto = new CategoryUpdateDto { CategoryName = "Updated Category", CategoryImage = "updated.jpg" };
            var updatedCategory = new Category { Id = categoryId, Name = categoryUpdateDto.CategoryName, Image = categoryUpdateDto.CategoryImage };
            var categoryReadDto = new CategoryReadDto();
            categoryReadDto.Transform(updatedCategory);

            _categoryRepoMock.Setup(repo => repo.UpdateCategoryByIdAsync(updatedCategory)).ReturnsAsync(updatedCategory);


            // Act
            var result = await _categoryService.UpdateCategoryByIdAsync(categoryId, categoryUpdateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(categoryReadDto, result);
        }

        [Fact]
        public async Task DeleteCategoryByIdAsync_ValidId_ShouldReturnTrue()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            _categoryRepoMock.Setup(repo => repo.DeleteCategoryByIdAsync(categoryId)).ReturnsAsync(true);

            // Act
            var result = await _categoryService.DeleteCategoryByIdAsync(categoryId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteCategoryByIdAsync_InValidId_ShouldReturnFalse()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            _categoryRepoMock.Setup(repo => repo.DeleteCategoryByIdAsync(categoryId)).ReturnsAsync(false);

            // Act
            var result = await _categoryService.DeleteCategoryByIdAsync(categoryId);

            // Assert
            Assert.False(result);
        }

    }
}
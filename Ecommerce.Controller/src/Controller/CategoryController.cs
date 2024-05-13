using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Ecommerce.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IEnumerable<CategoryReadDto>> GetAllCategoriesAsync() // endpoint: /categories
        {

            return await _categoryService.GetAllCategoriesAsync();

        }

        [HttpGet("{categoryId}")] // endpoint: /categories/:category_id
        public async Task<CategoryReadDto> GetCategoryByIdAsync([FromRoute] Guid categoryId)
        {
            return await _categoryService.GetCategoryByIdAsync(categoryId);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost] // endpoint: /categories
        public async Task<CategoryReadDto> CreateCategoryAsync([FromBody] CategoryCreateDto categoryCreateDto)
        {
            return await _categoryService.CreateCategoryAsync(categoryCreateDto);

        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{categoryId}")] // endpoint: /categories/:category_id
        public async Task<CategoryReadDto> UpdateCategoryByIdAsync([FromRoute] Guid categoryId, [FromBody] CategoryUpdateDto categoryUpdateDto)
        {
            return await _categoryService.UpdateCategoryByIdAsync(categoryId, categoryUpdateDto);

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{categoryId}")] // endpoint: /categories/:category_id
        public async Task<bool> DeleteCategoryByIdAsync([FromRoute] Guid categoryId)
        {
            return await _categoryService.DeleteCategoryByIdAsync(categoryId);
        }
    }
}

using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstract
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryReadDto>> GetAllCategoriesAsync();
        Task<CategoryReadDto> GetCategoryByIdAsync(Guid categoryId);
        Task<CategoryReadDto> CreateCategoryAsync(CategoryCreateDto categoryCreateDto);
        Task<CategoryReadDto> UpdateCategoryByIdAsync(Guid categoryId, CategoryUpdateDto categoryUpdateDto);
        Task<bool> DeleteCategoryByIdAsync(Guid categoryId);
    }
}
using Ecommerce.Core.src.Entity;

namespace Ecommerce.Core.src.RepoAbstract
{
    public interface ICategoryRepo
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(Guid categoryId);
        Task<Category> CreateCategoryAsync(Category newCategory);
        Task<Category> UpdateCategoryByIdAsync(Category updatedCategory);
        Task<bool> DeleteCategoryByIdAsync(Guid categoryId);
    }
}
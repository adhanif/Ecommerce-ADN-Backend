
using System.Text.RegularExpressions;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstract;
using Ecommerce.WebAPI.src.Database;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebAPI.src.Repo
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Category> _categories;
        public CategoryRepo(AppDbContext context)
        {
            _context = context;
            _categories = _context.Categories;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categories.ToListAsync();
        }
        public async Task<Category> GetCategoryByIdAsync(Guid categoryId)
        {
            var foundCategory = await _categories.FindAsync(categoryId) ?? throw AppException.NotFound($"Category not found: {categoryId}");
            return foundCategory;
        }
        public async Task<Category> CreateCategoryAsync(Category newCategory)
        {
            var duplicatedCategory = await _categories.FirstOrDefaultAsync(c => c.Name == newCategory.Name);
            if(duplicatedCategory is not null) throw AppException.DuplicateCategotyNameException();

            await _categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();
            return newCategory;
        }
        public async Task<Category> UpdateCategoryByIdAsync(Category updatedCategory)
        {
            // check if category exists
            var foundCategory = await _categories.FindAsync(updatedCategory.Id) ?? throw AppException.NotFound("Category not  found");

            // check if the new name already exist
            var duplicatedCategory = await _categories.FirstOrDefaultAsync(c => c.Name == updatedCategory.Name && c.Id != updatedCategory.Id);
            if(duplicatedCategory is not null) throw AppException.DuplicateCategotyNameException();

            _categories.Update(updatedCategory);
            await _context.SaveChangesAsync();
            return updatedCategory;
        }

        public async Task<bool> DeleteCategoryByIdAsync(Guid categoryId)
        {
            var foundCategory = await _categories.FindAsync(categoryId) ?? throw AppException.NotFound($"Deletion failed, because category not found: {categoryId}");
            _categories.Remove(foundCategory);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
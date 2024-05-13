using Ecommerce.Core.src.Entity;

namespace Ecommerce.Service.src.DTO
{
    public class CategoryReadDto 
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryImage { get; set; }
        public void Transform(Category category)
        {
            CategoryId = category.Id;
            CategoryName = category.Name;
            CategoryImage = category.Image;
        }
    }

    public class CategoryCreateDto
    {
        public string CategoryName { get; set; }
        public string CategoryImage { get; set; }

        public Category CreateCategory()
        {
            return new Category
            {
                Name = CategoryName,
                Image = CategoryImage,
            };
        }
    }

    public class CategoryUpdateDto
    {
        public string? CategoryName { get; set; }
        public string? CategoryImage { get; set; }

        public Category UpdateCategory(Category oldCategory)
        {
            oldCategory.Name = CategoryName ?? oldCategory.Name;
            oldCategory.Image = CategoryImage ?? oldCategory.Image;
            return oldCategory;
        }
    }
}
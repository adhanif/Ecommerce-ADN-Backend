using Ecommerce.Core.src.Entity;

namespace Ecommerce.Service.src.DTO
{
    public class CategoryReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public void Transform(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            Image = category.Image;
        }
    }

    public class CategoryCreateDto
    {
        public string Name { get; set; }
        public string Image { get; set; }

        public Category CreateCategory()
        {
            return new Category
            {
                Name = Name,
                Image = Image,
            };
        }
    }

    public class CategoryUpdateDto
    {
        public string? Name { get; set; }
        public string? Image { get; set; }

        public Category UpdateCategory(Category oldCategory)
        {
            oldCategory.Name = Name ?? oldCategory.Name;
            oldCategory.Image = Image ?? oldCategory.Image;
            return oldCategory;
        }
    }
}
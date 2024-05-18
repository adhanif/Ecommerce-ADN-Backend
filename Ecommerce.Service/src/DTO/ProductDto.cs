using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Ecommerce.Core.src.Entity;

namespace Ecommerce.Service.src.DTO
{
    public class ProductReadDto : BaseEntity
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        [JsonIgnore]
        public Guid CategoryId { get; set; } //  foreign key navigate to category
        public int Inventory { get; set; }
        public ICollection<ProductImage> Images { get; set; }
        public CategoryReadDto Category { get; set; }

        // public void Transform(Product product, CategoryReadDto categoryDto)
        // {
        //     Title = product.Title;
        //     Description = product.Description;
        //     Price = product.Price;
        //     CategoryId = product.CategoryId;
        //     Inventory = product.Inventory;
        //     CreatedDate = product.CreatedDate;
        //     UpdatedDate = product.UpdatedDate;
        //     Images = product.Images.Select(image => new ProductImageReadDto
        //     {
        //         Id = image.Id,
        //         Url = image.Url
        //     }).ToList();
        //     Category = categoryDto;
        // }
    }

    public class ProductCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public Guid CategoryId { get; set; }
        public int Inventory { get; set; }
        // public IEnumerable<ProductImageCreateDto> Images { get; set; }
        public IEnumerable<byte[]> ImageData { get; set; }

        public Product CreateProduct()
        {
            // Map Images from ImageCreateDto to Image entities
            // var ImagesDto = Images?.Select(imageDto => new ProductImage
            // {
            //     Id = Guid.NewGuid(),
            //     Url = imageDto.Url
            // }).ToList() ?? new List<ProductImage>();

            return new Product
            {
                Title = Title,
                Description = Description,
                Price = Price,
                CategoryId = CategoryId,
                Inventory = Inventory,
                // Images = ImagesDto,
            };
        }
    }


    public class ProductUpdateDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Price { get; set; }
        public Guid? CategoryId { get; set; }
        public int? Inventory { get; set; }
        public IEnumerable<ProductImageUpdateDto>? ImagesToUpdate { get; set; }
        public List<byte[]> ImageData { get; set; }

        public void UpdateProduct(Product oldproduct)
        {
            if (Title != null)
                oldproduct.Title = Title;
            if (Description != null)
                oldproduct.Description = Description;
            if (Price.HasValue)
                oldproduct.Price = Price.Value;
            if (CategoryId.HasValue)
                oldproduct.CategoryId = CategoryId.Value;
            if (Inventory.HasValue)
                oldproduct.Inventory = Inventory.Value;
            // if (ImagesToUpdate != null)
            // {
            //     // Map and update each Image in the ImagesToUpdate list
            //     foreach (var imageDto in ImagesToUpdate)
            //     {
            //         var imageToUpdate = oldproduct.Images.FirstOrDefault(img => img.Id == imageDto.Id);
            //         if (imageToUpdate != null)
            //         {
            //             imageToUpdate.Url = imageDto.Url;
            //         }
            //     }
            // }
        }
    }

    public class ProductReviewReadDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public Guid CategoryId { get; set; }
    }
}
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Ecommerce.Core.src.Entity;

namespace Ecommerce.Service.src.DTO
{
    public class ProductReadDto : BaseEntity
    {

        public string ProductTitle { get; set; }
        public string ProductDescription { get; set; }
        public int ProductPrice { get; set; }
        [JsonIgnore]
        public Guid CategoryId { get; set; } //  foreign key navigate to category
        public int ProductInventory { get; set; }
        public IEnumerable<ProductImageReadDto>? ProductImages { get; set; }
        public CategoryReadDto Category { get; set; }

        public void Transform(Product product, CategoryReadDto categoryDto)
        {
            ProductTitle = product.Title;
            ProductDescription = product.Description;
            ProductPrice = product.Price;
            CategoryId = product.CategoryId;
            ProductInventory = product.Inventory;
            CreatedDate = product.CreatedDate;
            UpdatedDate = product.UpdatedDate;
            ProductImages = product.ProductImages.Select(image => new ProductImageReadDto
            {
                Id = image.Id,
                Url = image.Url
            }).ToList();
            Category = categoryDto;
        }
    }

    public class ProductCreateDto
    {
        public string ProductTitle { get; set; }
        public string ProductDescription { get; set; }
        public int ProductPrice { get; set; }
        public Guid CategoryId { get; set; }
        public int ProductInventory { get; set; }
        public IEnumerable<ProductImageCreateDto>? ProductImages { get; set; }

        public Product CreateProduct()
        {
            // Map Images from ImageCreateDto to Image entities
            var ProductImagesDto = ProductImages?.Select(imageDto => new ProductImage
            {
                Id = Guid.NewGuid(),
                Url = imageDto.Url
            }).ToList() ?? new List<ProductImage>();

            return new Product
            {
                Title = ProductTitle,
                Description = ProductDescription,
                Price = ProductPrice,
                CategoryId = CategoryId,
                Inventory = ProductInventory,
                ProductImages = ProductImagesDto,
            };
        }
    }


    public class ProductUpdateDto
    {
        public Guid Id { get; set; }
        public string? ProductTitle { get; set; }
        public string? ProductDescription { get; set; }
        public int? ProductPrice { get; set; }
        public Guid? CategoryId { get; set; }
        public int? ProductInventory { get; set; }
        public IEnumerable<ProductImageUpdateDto>? ImagesToUpdate { get; set; }


        public void UpdateProduct(Product oldproduct)
        {
            if (ProductTitle != null)
                oldproduct.Title = ProductTitle;
            if (ProductDescription != null)
                oldproduct.Description = ProductDescription;
            if (ProductPrice.HasValue)
                oldproduct.Price = ProductPrice.Value;
            if (CategoryId.HasValue)
                oldproduct.CategoryId = CategoryId.Value;
            if (ProductInventory.HasValue)
                oldproduct.Inventory = ProductInventory.Value;
            if (ImagesToUpdate != null)
            {
                // Map and update each Image in the ImagesToUpdate list
                foreach (var imageDto in ImagesToUpdate)
                {
                    var imageToUpdate = oldproduct.ProductImages.FirstOrDefault(img => img.Id == imageDto.Id);
                    if (imageToUpdate != null)
                    {
                        imageToUpdate.Url = imageDto.Url;
                    }
                }
            }
        }
    }

    public class ProductReviewReadDto
    {
        public string ProductTitle { get; set; }
        public string ProductDescription { get; set; }
        public int ProductPrice { get; set; }
        public Guid CategoryId { get; set; }
    }
}
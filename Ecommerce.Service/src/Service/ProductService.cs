using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstract;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;

namespace Ecommerce.Service.src.Service
{
    public class ProductService : IProductService
    {

        private readonly IProductRepo _productRepo;
        private IMapper _mapper;
        private readonly ICategoryRepo _categoryRepo;
        private readonly IProductImageRepo _productImageRepo;

        public ProductService(IProductRepo productRepo, IMapper mapper, ICategoryRepo categoryRepo, IProductImageRepo productImageRepo)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _categoryRepo = categoryRepo;
            _productImageRepo = productImageRepo;
        }

        public async Task<IEnumerable<ProductReadDto>> GetAllProductsAsync(ProductQueryOptions? productQueryOptions)
        {
            try
            {
                var products = await _productRepo.GetAllProductsAsync(productQueryOptions);
                var productDtos = _mapper.Map<List<ProductReadDto>>(products);
                foreach (var productDto in productDtos)
                {
                    // Fetch category information for the product
                    var category = await _categoryRepo.GetCategoryByIdAsync(productDto.CategoryId);
                    var categoryDto = _mapper.Map<CategoryReadDto>(category);
                    // Set the category property in the product DTO
                    productDto.Category = categoryDto;
                }

                return productDtos;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProductReadDto>> GetProductsByCategoryAsync(Guid categoryId)
        {
            var foundCategory = await _categoryRepo.GetCategoryByIdAsync(categoryId);
            if (foundCategory is null)
            {
                throw AppException.NotFound("Category not found");
            }
            var products = await _productRepo.GetProductsByCategoryAsync(categoryId);
            var productDtos = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductReadDto>>(products);

            return productDtos;
        }

        public async Task<ProductReadDto> GetProductByIdAsync(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                throw AppException.BadRequest();
            }
            try
            {
                var product = await _productRepo.GetProductByIdAsync(productId);
                var productDto = _mapper.Map<ProductReadDto>(product);
                // Fetch category information for the product
                var category = await _categoryRepo.GetCategoryByIdAsync(productDto.CategoryId);
                var categoryDto = _mapper.Map<CategoryReadDto>(category);
                // Set the category property in the product DTO
                productDto.Category = categoryDto;
                return productDto;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<ProductReadDto> CreateProductAsync(ProductCreateDto productCreateDto)
        {
            try
            {
                if (productCreateDto == null)
                {
                    throw new ArgumentNullException(nameof(productCreateDto), "ProductC cannot be null");
                }
                // Check if the product name is provided
                if (string.IsNullOrWhiteSpace(productCreateDto.Title))
                {
                    throw AppException.InvalidInputException("Product name cannot be empty");
                }

                // Check if the price is greater than zero
                if (productCreateDto.Price <= 0)
                {
                    throw AppException.InvalidInputException("Price should be greated than zero.");
                }

                var category = await _categoryRepo.GetCategoryByIdAsync(productCreateDto.CategoryId);
                if (category == null)
                {
                    throw AppException.NotFound("Category not found");
                }

                var productEntity = _mapper.Map<Product>(productCreateDto);
                productEntity.Images = productCreateDto.ImageData.Select(imageData => new ProductImage { Data = imageData, ProductId = productEntity.Id }).ToList();
                var createdProduct = await _productRepo.CreateProductAsync(productEntity);
                var productReadDto = _mapper.Map<ProductReadDto>(createdProduct);
                productReadDto.Category = _mapper.Map<CategoryReadDto>(category);
                return productReadDto;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<bool> DeleteProductByIdAsync(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                throw new Exception("bad request");
            }
            try
            {
                var deletedProduct = await _productRepo.DeleteProductByIdAsync(productId);

                if (!deletedProduct)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductReadDto> UpdateProductByIdAsync(Guid productId, ProductUpdateDto productUpdateDto)
        {
            try
            {
                var foundProduct = await _productRepo.GetProductByIdAsync(productId);

                foundProduct.Title = productUpdateDto.Title ?? foundProduct.Title;
                foundProduct.Description = productUpdateDto.Description ?? foundProduct.Description;
                foundProduct.CategoryId = productUpdateDto.CategoryId ?? foundProduct.CategoryId;

                foundProduct.Price = productUpdateDto.Price ?? foundProduct.Price;
                // Update inventory by adding the new inventory value
                if (productUpdateDto.Inventory.HasValue)
                {
                    foundProduct.Inventory += productUpdateDto.Inventory.Value;
                }

                // Save changes to the product
                await _productRepo.UpdateProductByIdAsync(foundProduct);


                // Handle product images
                if (productUpdateDto.ImageData != null && productUpdateDto.ImageData.Any())
                {
                    // Delete existing images
                    await _productImageRepo.DeleteProductImagesByProductIdAsync(productId);

                    // Add new images
                    foreach (var imageData in productUpdateDto.ImageData)
                    {
                        var productImage = new ProductImage
                        {
                            ProductId = productId,
                            Data = imageData
                        };
                        foundProduct.Images.Add(productImage);
                    }
                }
                await _productRepo.UpdateProductByIdAsync(foundProduct);
                // Construct the updated product DTO to return
                var updatedProductDto = _mapper.Map<ProductReadDto>(foundProduct);
                return updatedProductDto;

            }
            catch (Exception)
            {
                throw;
            }
        }
        // Method to validate image URL
        bool IsImageUrlValid(string imageUrl)
        {
            // Regular expression pattern to match common image file extensions (e.g., .jpg, .jpeg, .png, .gif)
            string pattern = @"^(http(s?):)([/|.|\w|\s|-])*\.(?:jpg|jpeg|gif|png)$";

            // Create a regular expression object
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            // Check if the URL matches the pattern
            return regex.IsMatch(imageUrl);
        }

    }
}
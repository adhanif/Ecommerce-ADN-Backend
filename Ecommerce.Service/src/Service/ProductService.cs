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


        public async Task<ProductReadDto> CreateProductAsync(ProductCreateDto newProduct)
        {
            try
            {
                if (newProduct == null)
                {
                    throw new ArgumentNullException(nameof(newProduct), "ProductC cannot be null");
                }
                // Check if the product name is provided
                if (string.IsNullOrWhiteSpace(newProduct.Title))
                {
                    throw AppException.InvalidInputException("Product name cannot be empty");
                }

                // Check if the price is greater than zero
                if (newProduct.Price <= 0)
                {
                    throw AppException.InvalidInputException("Price should be greated than zero.");
                }

                // Validate image URLs
                if (newProduct.Images is not null)
                {

                    foreach (var image in newProduct.Images)
                    {
                        // Check if the URL is provided
                        if (string.IsNullOrWhiteSpace(image.Url))
                        {
                            throw AppException.InvalidInputException("Image URL cannot be empty");
                        }

                        // Check if the URL points to a valid image format 
                        if (!IsImageUrlValid(image.Url))
                        {
                            throw AppException.InvalidInputException("Invalid image format");
                        }
                    }
                }
                // Check if the specified category ID exists
                var category = await _categoryRepo.GetCategoryByIdAsync(newProduct.CategoryId);
                if (category == null)
                {
                    throw AppException.NotFound("Category not found");
                }
                var productEntity = _mapper.Map<Product>(newProduct);
                productEntity.Images = _mapper.Map<List<ProductImage>>(newProduct.Images);
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


        public Task<IEnumerable<ProductReadDto>> GetMostPurchasedProductsAsync(int topNumber)
        {
            throw new NotImplementedException();
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

                // Find product images
                var Images = await _productImageRepo.GetProductImagesByProductIdAsync(productId);


                // Update product images
                if (productUpdateDto.ImagesToUpdate is not null && productUpdateDto.ImagesToUpdate.Any())
                {
                    foreach (var imageDto in productUpdateDto.ImagesToUpdate)
                    {
                        // Find the image to update by URL
                        var imageToUpdate = Images.FirstOrDefault(img => img.Url == imageDto.Url);

                        if (imageToUpdate is not null)
                        {
                            // Update image URL if it has changed
                            if (imageToUpdate.Url != imageDto.Url)
                            {
                                // Update the image URL using the repository method
                                var updateResult = _productImageRepo.UpdateImageUrlAsync(imageToUpdate.Id, imageDto.Url);
                            }
                        }
                        else
                        {
                            // Handle the case where the image URL from the DTO doesn't match any existing images
                            throw new Exception($"Image with URL {imageDto.Url} not found.");
                        }

                        // Validate image URL
                        if (!IsImageUrlValid(imageDto.Url))
                        {
                            throw AppException.InvalidInputException("Invalid image URL format");
                        }
                    }
                }


                // Save changes to the database
                var updatedProduct = await _productRepo.UpdateProductByIdAsync(foundProduct);

                // Fetch category information for the updated product
                var category = await _categoryRepo.GetCategoryByIdAsync(updatedProduct.CategoryId);
                var categoryDto = _mapper.Map<CategoryReadDto>(category);
                // Map the updated product entity to ProductReadDto
                var updatedProductDto = _mapper.Map<Product, ProductReadDto>(updatedProduct);
                // Update the Inventory value in the returned DTO
                updatedProductDto.Inventory = foundProduct.Inventory;
                // Set the category property in the updated product DTO
                updatedProductDto.Category = categoryDto;
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
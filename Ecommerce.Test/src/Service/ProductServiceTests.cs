// using AutoMapper;
// using Ecommerce.Core.src.Common;
// using Ecommerce.Core.src.Entity;
// using Ecommerce.Core.src.RepoAbstract;
// using Ecommerce.Service.src.DTO;
// using Ecommerce.Service.src.Service;
// using Ecommerce.Service.src.Shared;
// using Moq;
// using Xunit;

// namespace Ecommerce.Test.src.Service
// {
//     public class ProductServiceTests
//     {
//         private readonly ProductService _productService;
//         private readonly Mock<IProductRepo> _productRepoMock = new();
//         private readonly Mock<IMapper> _mapperMock = new();
//         private readonly IMapper _mapper;

//         public ProductServiceTests()
//         {
//             if (_mapper == null)
//             {
//                 var mappingConfig = new MapperConfiguration(m =>
//                 {
//                     m.AddProfile(new MapperProfile());
//                 });
//                 IMapper mapper = mappingConfig.CreateMapper();
//                 _mapper = mapper;
//             }


//             _productService = new ProductService(_productRepoMock.Object, _mapperMock.Object);
//         }


//         [Fact]
//         public async Task GetAllProductsAsync_WithoutOptions_ShouldReturnAllProducts()
//         {
//             // Arrange
//             var expectedProducts = new List<Product>
//             {
//                 new Product
//                 {
//                     Id = Guid.NewGuid(),
//                     Title = "Product 1",
//                     Description = "Description 1",
//                     Price = 10,
//                     CategoryId = Guid.NewGuid(),
//                     ProductImages =
//                     [
//                         new ProductImage { Id = Guid.NewGuid(), Url = "image1.jpg" },
//                         new ProductImage { Id = Guid.NewGuid(), Url = "image2.jpg" }
//                     ]
//                 },
//                 new Product
//                 {
//                     Id = Guid.NewGuid(),
//                     Title = "Product 2",
//                     Description = "Description 2",
//                     Price = 20,
//                     CategoryId = Guid.NewGuid(),
//                     ProductImages =
//                     [
//                         new ProductImage { Id = Guid.NewGuid(), Url = "image3.jpg" },
//                         new ProductImage { Id = Guid.NewGuid(), Url = "image4.jpg" }
//                     ]
//                 }
//             };

//             _productRepoMock.Setup(repo => repo.GetAllProductsAsync(null))
//                             .ReturnsAsync(expectedProducts);

//             // Act
//             var result = await _productService.GetAllProductsAsync(null);

//             // Assert
//             Assert.NotNull(result);
//             Assert.Equal(expectedProducts.Count, result.Count());
//         }

//         [Fact]
//         public async Task GetAllProductsAsync_ValidOptions_ReturnsProductReadDtos()
//         {
//             // Arrange
//             var categoryId1 = Guid.NewGuid();
//             var categoryId2 = Guid.NewGuid();
//             var options = new ProductQueryOptions
//             {
//                 Title = "test",
//                 Min_Price = 50,
//                 Max_Price = 220,
//                 Category_Id = categoryId1,
//                 SortBy = "title",
//                 SortOrder = "asc",
//                 Offset = 0,
//                 Limit = 10
//             };

//             var products = new List<Product>
//     {
//         new() { Title = "Test Product 1", Description = "Description 1", Price = 100, CategoryId = categoryId1 },
//         new() { Title = "Test Product 2", Description = "Description 2", Price = 150, CategoryId = categoryId1 },
//         new() { Title = "Test Product 3", Description = "Description 3", Price = 200, CategoryId = categoryId2 },
//         new() { Title = "Test Product 4", Description = "Description 4", Price = 250, CategoryId = categoryId2 }
//     };

//             _productRepoMock.Setup(repo => repo.GetAllProductsAsync(options))
//                             .ReturnsAsync(products);

//             // Act
//             var result = await _productService.GetAllProductsAsync(options);

//             // Assert
//             Assert.NotNull(result);
//             Assert.Equal(2, result.Count());
//         }



//         [Fact]
//         public async Task GetProductByIdAsync_ValidId_ShouldReturnProduct()
//         {
//             // Arrange
//             var productId = Guid.NewGuid();
//             var expectedProduct = new Product
//             {
//                 Id = productId,
//                 Title = "Test Product",
//                 Description = "Test Description",
//                 Price = 100,
//                 CategoryId = Guid.NewGuid(),
//                 ProductImages =
//         [
//             new ProductImage { Id = Guid.NewGuid(), Url = "image1.jpg" },
//             new ProductImage { Id = Guid.NewGuid(), Url = "image2.jpg" }
//         ]
//             };
//             _productRepoMock.Setup(repo => repo.GetProductByIdAsync(productId))
//                             .ReturnsAsync(expectedProduct);

//             // Act
//             var result = await _productService.GetProductByIdAsync(productId);

//             // Assert
//             Assert.NotNull(result);
//             Assert.Equal(expectedProduct.Id, result.Id);
//         }


//         [Fact]
//         public async Task GetProductByIdAsync_InvalidId_ShouldReturnNull()
//         {
//             // Arrange
//             var invalidProductId = Guid.NewGuid();
//             var expectedProduct = new Product
//             {
//                 Id = invalidProductId,
//                 Title = "Test Product",
//                 Description = "Test Description",
//                 Price = 100,
//                 CategoryId = Guid.NewGuid(),
//                 ProductImages =
//         [
//             new ProductImage { Id = Guid.NewGuid(), Url = "image1.jpg" },
//             new ProductImage { Id = Guid.NewGuid(), Url = "image2.jpg" }
//         ]
//             };
//             _productRepoMock.Setup(repo => repo.GetProductByIdAsync(invalidProductId))
//                             .ThrowsAsync(AppException.NotFound("Product not found"));
//             // Act
//             var result = await _productService.GetProductByIdAsync(invalidProductId);

//             // Assert
//             Assert.Null(result); // Assert that the result is null for the invalid ID
//         }

//         [Fact]
//         public async Task CreateProductAsync_ValidInput_ReturnsProductReadDto()
//         {
//             // Arrange
//             var newProduct = new ProductCreateDto
//             {
//                 ProductTitle = "Test Product",
//                 ProductDescription = "Test Description",
//                 ProductPrice = 100,
//                 CategoryId = Guid.NewGuid(),
//                 ProductImages =
//                 [
//                     new ProductImageCreateDto { Url = "image_url_1" },
//                     new ProductImageCreateDto { Url = "image_url_2" }
//                 ]
//             };

//             var productEntity = new Product
//             {
//                 Title = newProduct.ProductTitle,
//                 Description = newProduct.ProductDescription,
//                 Price = newProduct.ProductPrice,
//                 CategoryId = newProduct.CategoryId,
//                 ProductImages = newProduct.ProductImages.Select(imageDto => new ProductImage { Url = imageDto.Url }).ToList()
//             };

//             _productRepoMock.Setup(repo => repo.CreateProductAsync(It.IsAny<Product>()))
//                             .ReturnsAsync(productEntity);

//             // Act
//             var result = await _productService.CreateProductAsync(newProduct);

//             // Assert
//             Assert.NotNull(result);
//             Assert.Equal(newProduct.ProductTitle, result.ProductTitle);
//             Assert.Equal(newProduct.ProductDescription, result.ProductDescription);
//             Assert.Equal(newProduct.ProductPrice, result.ProductPrice);
//             Assert.Equal(newProduct.CategoryId, result.CategoryId);
//             Assert.Equal(newProduct.ProductImages.Count(), result?.ProductImages?.Count());
//         }

//         [Fact]
//         public async Task DeleteProductByIdAsync_ValidId_ReturnsTrue()
//         {
//             // Arrange
//             var productId = Guid.NewGuid();

//             _productRepoMock.Setup(repo => repo.DeleteProductByIdAsync(productId))
//                             .ReturnsAsync(true);

//             // Act
//             var result = await _productService.DeleteProductByIdAsync(productId);

//             // Assert
//             Assert.True(result);
//         }

//         [Fact]
//         public async Task UpdateProductByIdAsync_ValidInput_ReturnsProductReadDto()
//         {
//             // Arrange
//             var productId = Guid.NewGuid();
//             var productUpdateDto = new ProductUpdateDto
//             {
//                 ProductTitle = "Updated Product",
//                 ProductDescription = "Updated Description",
//                 ProductPrice = 150
//             };


//             var updatedProductEntity = new Product
//             {
//                 Id = productId,
//                 Title = productUpdateDto.ProductTitle,
//                 Description = productUpdateDto.ProductDescription,
//                 Price = (int)productUpdateDto.ProductPrice
//             };

//             _productRepoMock.Setup(repo => repo.UpdateProductByIdAsync(It.IsAny<Product>())).ReturnsAsync(updatedProductEntity);

//             // Act
//             var result = await _productService.UpdateProductByIdAsync(productId, productUpdateDto);

//             // Assert
//             Assert.NotNull(result);
//             Assert.Equal(productUpdateDto.ProductTitle, result.ProductTitle);
//             Assert.Equal(productUpdateDto.ProductDescription, result.ProductDescription);
//             Assert.Equal(productUpdateDto.ProductPrice, result.ProductPrice);
//         }

//         [Fact]
//         public async Task UpdateProductByIdAsync_InvalidId_ReturnsNull()
//         {
//             // Arrange
//             var productId = Guid.NewGuid();
//             var productUpdateDto = new ProductUpdateDto
//             {
//                 ProductTitle = "Updated Product",
//                 ProductDescription = "Updated Description",
//                 ProductPrice = 150
//             };

//             _productRepoMock.Setup(repo => repo.UpdateProductByIdAsync(It.IsAny<Product>()))
//                             .ThrowsAsync(AppException.NotFound("Product not found"));

//             // Act
//             var result = await _productService.UpdateProductByIdAsync(productId, productUpdateDto);

//             // Assert
//             Assert.Null(result);
//         }

//         [Fact]
//         public async Task GetMostPurchasedProductsAsync_ValidTopNumber_ReturnsProductReadDtos()
//         {
//             // Arrange
//             var topNumber = 2;
//             var products = new List<Product>
//             {
//                 new Product { Title = "Product 1", Description = "Description 1", Price = 100 },
//                 new Product { Title = "Product 2", Description = "Description 2", Price = 200 }
//             };

//             _productRepoMock.Setup(repo => repo.GetMostPurchasedProductsAsync(topNumber))
//                             .ReturnsAsync(products);

//             // Act
//             var result = await _productService.GetMostPurchasedProductsAsync(topNumber);

//             // Assert
//             Assert.NotNull(result);
//             Assert.Equal(products.Count, result.Count());
//         }
//     }
// }
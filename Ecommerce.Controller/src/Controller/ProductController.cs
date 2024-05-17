using Ecommerce.Core.src.Common;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/products")]
    public class ProductController : ControllerBase
    {
        private IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Consumes("multipart/form-data")]
        [HttpPost("form-create")]
        public async Task<ActionResult<ProductReadDto>> CreateFromFormAsync([FromForm] ProductForm productForm)
        {
            if (productForm == null || productForm.Images == null || productForm.Images.Count == 0)
            {
                return BadRequest("Product data and images are required.");
            }

            var imageList = new List<byte[]>();
            foreach (var image in productForm.Images)
            {
                if (image.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await image.CopyToAsync(ms);
                        imageList.Add(ms.ToArray());
                    }
                }
            }
            var productCreateDto = new ProductCreateDto
            {
                Title = productForm.Title,
                Description = productForm.Description,
                Price = productForm.Price,
                CategoryId = productForm.CategoryId,
                Inventory = productForm.Inventory,
                ImageData = imageList
            };

            var createdProduct = await _productService.CreateProductAsync(productCreateDto);
            return Ok(createdProduct);
        }


        [HttpGet("")]
        public async Task<IEnumerable<ProductReadDto>> GetAllProductsAsync([FromQuery] ProductQueryOptions? options)
        {

            return await _productService.GetAllProductsAsync(options);
        }

        [HttpGet("{productId}")]
        public async Task<ProductReadDto> GetProductByIdAsync([FromRoute] Guid productId)
        {
            return await _productService.GetProductByIdAsync(productId);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("")]
        public async Task<ProductReadDto> CreateProductAsync([FromBody] ProductCreateDto productCreateDto)
        {

            return await _productService.CreateProductAsync(productCreateDto);

        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{productId}")]
        public async Task<ProductReadDto> UpdateProductByIdAsync([FromRoute] Guid productId, [FromBody] ProductUpdateDto productUpdateDto)
        {

            return await _productService.UpdateProductByIdAsync(productId, productUpdateDto);

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{productId}")]
        public async Task<bool> DeleteProductByIdAsync([FromRoute] Guid productId)
        {

            return await _productService.DeleteProductByIdAsync(productId);

        }
    }


    public class ProductForm // does not deal with data base data, nor business logic
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public Guid CategoryId { get; set; }
        public int Inventory { get; set; }
        public required List<IFormFile> Images { get; set; }

    }
}
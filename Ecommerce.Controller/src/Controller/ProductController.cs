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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        [HttpPut("form-update")]
        public async Task<ActionResult<ProductReadDto>> UpdateFromFormAsync(Guid productId, [FromForm] ProductForm productForm)
        {

            var imageList = new List<byte[]>();
            if (productForm.Images is not null)
            {
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
            }
            var productCreateDto = new ProductUpdateDto
            {
                Title = productForm.Title,
                Description = productForm.Description,
                Price = productForm.Price,
                CategoryId = productForm.CategoryId,
                Inventory = productForm.Inventory,
                ImageData = imageList
            };

            var createdProduct = await _productService.UpdateProductByIdAsync(productId, productCreateDto);
            return Ok(createdProduct);
        }




        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetAllProductsAsync([FromQuery] ProductQueryOptions? options)
        {

            Console.WriteLine(options?.Category_Id);

            var products = await _productService.GetAllProductsAsync(options);
            return Ok(products);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetProductsByCategoryAsync([FromRoute] Guid categoryId)
        {
            var result = await _productService.GetProductsByCategoryAsync(categoryId);
            return Ok(result);
        }


        [HttpGet("{productId}")]
        public async Task<ActionResult<ProductReadDto>> GetProductByIdAsync([FromRoute] Guid productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("{productId}")]
        public async Task<ActionResult<ProductReadDto>> UpdateProductByIdAsync([FromRoute] Guid productId, [FromBody] ProductUpdateDto productUpdateDto)
        {

            var updatedProduct = await _productService.UpdateProductByIdAsync(productId, productUpdateDto);
            return Ok(updatedProduct);

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{productId}")]
        public async Task<ActionResult<bool>> DeleteProductByIdAsync([FromRoute] Guid productId)
        {
            var deletedProduct = await _productService.DeleteProductByIdAsync(productId);
            return Ok(deletedProduct);
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
using Ecommerce.Core.src.Common;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;
using Microsoft.AspNetCore.Authorization;
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
}
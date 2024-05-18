using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;

namespace Ecommerce.Core.src.RepoAbstract
{
    public interface IProductRepo
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(ProductQueryOptions? options);
        Task<Product> GetProductByIdAsync(Guid productId);
        Task<Product> CreateProductAsync(Product newProduct);
        Task<Product> UpdateProductByIdAsync(Product updatedProduct);
        Task<bool> DeleteProductByIdAsync(Guid productId);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(Guid categoryId);
    }
}
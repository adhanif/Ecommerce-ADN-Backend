using Ecommerce.Core.src.Entity;

namespace Ecommerce.Core.src.RepoAbstract
{
    public interface IProductImageRepo
    {
        Task<IEnumerable<ProductImage>> GetProductImagesByProductIdAsync(Guid productId);
        Task<ProductImage> GetImageByIdAsync(Guid imageId);
        Task<bool> DeleteProductImagesByProductIdAsync(Guid productId);
    }
}
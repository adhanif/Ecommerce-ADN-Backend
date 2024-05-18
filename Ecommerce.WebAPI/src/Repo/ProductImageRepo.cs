using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstract;
using Ecommerce.WebAPI.src.Database;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebAPI.src.Repo
{
    public class ProductImageRepo : IProductImageRepo
    {

        private readonly AppDbContext _context;
        private readonly DbSet<ProductImage> _productImages;

        public ProductImageRepo(AppDbContext context)
        {
            _context = context;
            _productImages = _context.Images;
        }

        public async Task<bool> DeleteProductImagesByProductIdAsync(Guid productId)
        {
            var imagesToDelete = await _productImages
                .Where(image => image.ProductId == productId)
                .ToListAsync();
            _productImages.RemoveRange(imagesToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ProductImage> GetImageByIdAsync(Guid imageId)
        {
            return await _productImages.FindAsync(imageId);
        }

        public async Task<IEnumerable<ProductImage>> GetProductImagesByProductIdAsync(Guid productId)
        {
            return await _productImages
                .Where(image => image.ProductId == productId)
                .ToListAsync();
        }


    }
}
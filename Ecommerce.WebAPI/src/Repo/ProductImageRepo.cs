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

        public async Task UpdateImageUrlAsync(Guid imageId, string newUrl)
        {
            // Retrieve the image by its ID
            var image = await GetImageByIdAsync(imageId);

            // // Check if the image is found
            // if (image != null)
            // {
            //     // Update the image URL with the new one
            //     image.Url = newUrl;

            //     // Save the changes to the database
            //     await _context.SaveChangesAsync();
            // }
            // else
            // {
            //     // Handle the case where the image is not found
            //     throw new ArgumentException("Image not found", nameof(imageId));
            // }
        }
    }
}
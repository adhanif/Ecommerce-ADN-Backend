using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.Common;
using Ecommerce.WebAPI.src.Database;
using Ecommerce.Core.src.RepoAbstract;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Service.src.Service;

namespace Ecommerce.WebAPI.src.Repo
{
    public class ProductRepo : IProductRepo
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Product> _products;

        public ProductRepo(AppDbContext context)
        {
            _context = context;
            _products = _context.Products;
        }

        public async Task<Product> CreateProductAsync(Product newProduct)
        {
            var foundProduct = await _products.FirstOrDefaultAsync(p => p.Title == newProduct.Title);
            if (foundProduct is null)
            {
                await _products.AddAsync(newProduct);
                await _context.SaveChangesAsync();
                return newProduct;
            }
            else
            {
                throw AppException.DuplicateEmailException("Product Title already exist");
            }

        }

        public async Task<bool> DeleteProductByIdAsync(Guid productId)
        {
            var foundProduct = await _products.FindAsync(productId);
            if (foundProduct is null)
            {
                throw AppException.NotFound("product not found");
            };
            _products.Remove(foundProduct);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(ProductQueryOptions? options)
        {
            // var query = _products.AsQueryable();
            var query = _products.AsQueryable();
            query = query.Include(p => p.Images);
            // Apply filters if ProductQueryOptions is not null
            if (options != null)
            {
                // Filter by search title
                if (!string.IsNullOrEmpty(options.Title))
                {

                    var lowercaseTitle = options.Title.ToLower(); // Convert title to lowercase
                    query = query.Where(p => p.Title.ToLower().Contains(lowercaseTitle));

                }

                // Filter by price range
                if (options.Min_Price.HasValue)
                {
                    query = query.Where(p => p.Price >= options.Min_Price.Value);
                }

                if (options.Max_Price.HasValue)
                {
                    query = query.Where(p => p.Price <= options.Max_Price.Value);
                }

                // Filter by category ID
                if (options.Category_Id.HasValue)
                {
                    query = query.Where(p => p.CategoryId == options.Category_Id);
                }

                // Sorting
                if (!string.IsNullOrEmpty(options.SortBy))
                {
                    switch (options.SortBy.ToLower())
                    {
                        case "title":
                            query = options.SortOrder == "desc" ? query.OrderByDescending(p => p.Title) : query.OrderBy(p => p.Title);
                            break;
                        case "price":
                            query = options.SortOrder == "desc" ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price);
                            break;

                        default:
                            // Default sorting by created date if sort by is not specified or invalid
                            query = options.SortOrder == "desc" ? query.OrderByDescending(p => p.CreatedDate) : query.OrderBy(p => p.CreatedDate);
                            break;
                    }
                }

                // Pagination
                if (options.Offset != null && options.Limit != null)
                {
                    int offset = options.Offset.Value;
                    int limit = options.Limit.Value;
                    query = query.Skip(offset).Take(limit);
                }
            }

            // Execute the query
            var products = await query.ToListAsync(); ;
            return products;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(Guid categoryId)
        {
            var products = await _products
            .Include(p => p.Images)
            .Where(p => p.CategoryId == categoryId).ToListAsync();
            return products;
        }



        public async Task<Product> GetProductByIdAsync(Guid productId)
        {
            var foundproduct = await _context.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == productId);
            if (foundproduct is null)
            {
                throw AppException.NotFound("Product not found");
            }
            return foundproduct;
        }

        public async Task<Product> UpdateProductByIdAsync(Product updatedProduct)
        {
            // Load related images
            await _context.Entry(updatedProduct)
                .Collection(p => p.Images)
                .LoadAsync();
            _products.Update(updatedProduct);
            await _context.SaveChangesAsync();
            return updatedProduct;
        }
    }
}
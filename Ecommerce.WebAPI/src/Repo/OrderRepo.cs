using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstract;
using Ecommerce.WebAPI.src.Database;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebAPI.src.Repo
{
    public class OrderRepo : IOrderRepo
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Order> _orders;
        private DbSet<Product> _products;
        private readonly DbSet<OrderProduct> _orderProducts;

        public OrderRepo(AppDbContext context)
        {
            _context = context;
            _orders = _context.Orders;
            _products = _context.Products;
            _orderProducts = _context.OrderProducts;
        }

        public async Task<Order> CreateOrderAsync(Order createdOrder)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // TODO: Funtion for updating the decrease of product inventory due to the quantity in the product order. (Will be implemented when having the product inventory).
                    // TODO: Total price of transaction...

                    await _orders.AddAsync(createdOrder);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return createdOrder;
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<bool> DeleteOrderByIdAsync(Guid orderId)
        {
            var foundOrder = await _orders.FindAsync(orderId);
            if (foundOrder is null)
            {
                throw AppException.NotFound("Order not found for ID: " + orderId);
            }

            _orders.Remove(foundOrder);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync(BaseQueryOptions? options)
        {
            var query = _orders.AsQueryable();

            // Pagination
            if (options is not null)
            {
                query = query.OrderBy(o => o.CreatedDate)
                             .Skip(options.Offset)
                             .Take(options.Limit);
            }

            var orders = await query.ToListAsync();
            return orders;
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            var foundOrder = await _orders.FindAsync(orderId);
            return foundOrder;
        }

        public async Task<Order> UpdateOrderByIdAsync(Order updatedOrder)
        {
            _orders.Update(updatedOrder);
            await _context.SaveChangesAsync();
            return updatedOrder;
        }
    }
}
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

                    foreach (var orderProduct in createdOrder.OrderProducts)
                    {
                        var foundProduct = _products.FirstOrDefault(p => p.Id == orderProduct.Product.Id);
                        Console.WriteLine(foundProduct);

                        if (foundProduct.Inventory >= orderProduct.Quantity)
                        {

                            foundProduct.Inventory -= orderProduct.Quantity;
                            _context.Products.Update(foundProduct);
                            foundProduct.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);
                            _context.SaveChanges();
                        }
                        else
                        {
                            throw AppException.BadRequest($"Insufficient Products in the inventory: '{foundProduct.Title}'");
                        }
                    }

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
            if (foundOrder == null)
            {
                throw AppException.NotFound("Order not found ");
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Ensure OrderProducts is loaded
                    await _context.Entry(foundOrder).Collection(o => o.OrderProducts).LoadAsync();

                    foreach (var orderProduct in foundOrder.OrderProducts)
                    {
                        // Ensure Product is loaded
                        await _context.Entry(orderProduct).Reference(op => op.Product).LoadAsync();

                        if (orderProduct.Product != null)
                        {
                            orderProduct.Product.Inventory += orderProduct.Quantity;
                            _context.Products.Update(orderProduct.Product);
                        }
                    }

                    _orders.Remove(foundOrder);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    transaction.Rollback();
                    throw;
                }
            }
        }


        public async Task<IEnumerable<Order>> GetAllOrdersAsync(BaseQueryOptions? options)
        {
            var query = _orders
         .Include(o => o.OrderProducts) // Include order products
         .AsQueryable();

            // Pagination
            if (options is not null)
            {
                query = query.OrderBy(o => o.CreatedDate);
                //  .Skip(options.Offset)
                //  .Take(options.Limit);
                if (options.Offset != null && options.Limit != null)
                {
                    int offset = options.Offset.Value;
                    int limit = options.Limit.Value;
                    query = query.Skip(offset).Take(limit);
                }
            }

            var orders = await query.ToListAsync();
            return orders;
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {

            var foundOrder = await _orders.Include(o => o.OrderProducts).FirstOrDefaultAsync(o => o.Id == orderId);
            return foundOrder;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId)
        {
            var query = _orders.AsQueryable();
            query = query.Include(o => o.OrderProducts)
                         .ThenInclude(op => op.Product)
                         .Where(o => o.UserId == userId);
            var result = await query.ToListAsync();
            return result;
        }
    }
}
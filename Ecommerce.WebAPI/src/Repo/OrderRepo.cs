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
                        var foundProduct = _products.FirstOrDefault(product => product == orderProduct.Product);
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
            if (foundOrder is null)
            {
                throw AppException.NotFound("Order not found for ID: " + orderId);
            }
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var orderProduct in foundOrder.OrderProducts)
                    {
                        var foundProduct = orderProduct.Product;
                        foundProduct.Inventory += orderProduct.Quantity;
                        _context.Products.Update(foundProduct);
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
                query = query.OrderBy(o => o.CreatedDate)
                             .Skip(options.Offset)
                             .Take(options.Limit);
            }

            var orders = await query.ToListAsync();
            return orders;
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {

            var foundOrder = await _orders.Include(o => o.OrderProducts).FirstOrDefaultAsync(o => o.Id == orderId);
            return foundOrder;
        }

        public async Task<Order> UpdateOrderByIdAsync(Order updatedOrder)
        {
            // var foundOrder = _orders.Update(updatedOrder) ?? throw AppException.NotFound("Order not found"); ;
            // await _context.SaveChangesAsync();
            // return updatedOrder;

            var foundOrder = await _orders.FindAsync(updatedOrder.Id);
            if (foundOrder == null)
            {
                throw AppException.NotFound($"Order not found for ID: {updatedOrder.Id}");
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Update order properties
                    foundOrder.Address = updatedOrder.Address;

                    // Update order products quantity
                    foreach (var updatedOrderProduct in updatedOrder.OrderProducts)
                    {
                        var existingOrderProduct = foundOrder.OrderProducts.FirstOrDefault(op => op.OrderId == updatedOrderProduct.OrderId);
                        if (existingOrderProduct != null)
                        {
                            existingOrderProduct.Quantity = updatedOrderProduct.Quantity;
                        }
                        else
                        {
                            foundOrder.OrderProducts.Append(updatedOrderProduct);
                        }
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return foundOrder;
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    transaction.Rollback();
                    throw;
                }
            }
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
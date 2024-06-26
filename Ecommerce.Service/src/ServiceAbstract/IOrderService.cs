using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstract
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderReadDto>> GetAllOrdersAsync(BaseQueryOptions options);
        Task<OrderReadDto> GetOrderByIdAsync(Guid orderId);
        Task<OrderReadDto> CreateOrderAsync(Guid userId, OrderCreateDto orderCreateDto);
        Task<bool> DeleteOrderByIdAsync(Guid orderId);
        Task<IEnumerable<OrderReadDto>> GetOrdersByUserIdAsync(Guid userId);
        Task<OrderReadDto> UpdateOrderAsync(Guid orderId, OrderStatus newStatus);
    }
}
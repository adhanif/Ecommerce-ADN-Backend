using System.Security.Claims;
using Ecommerce.Core.src.Common;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/orders")]
    public class OrderController : ControllerBase
    {
        private IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet()]
        public async Task<IEnumerable<OrderReadDto>> GetAllOrdersAsync([FromQuery] BaseQueryOptions options)
        {
            return await _orderService.GetAllOrdersAsync(options); // Will be modified later
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{orderId}")]
        public async Task<OrderReadDto> GetOrderByIdAsync([FromRoute] Guid orderId)
        {
            return await _orderService.GetOrderByIdAsync(orderId); // Will be modified later
        }

        [Authorize()]
        [HttpPost()]
        public async Task<OrderReadDto> CreateOrderAsync([FromBody] OrderCreateDto orderCreateDto)
        {
            var userId = GetUserIdClaim();
            return await _orderService.CreateOrderAsync(userId, orderCreateDto); // Will be modified later
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{orderId}")]
        public async Task<OrderReadUpdateDto> UpdateOrderByIdAsync([FromRoute] Guid orderId, [FromBody] OrderUpdateDto orderUpdateDto)
        {
            orderUpdateDto.OrderId = orderId; // If order is found...
            return await _orderService.UpdateOrderByIdAsync(orderId, orderUpdateDto); // Will be modified later
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{orderId}")]
        public async Task<bool> DeleteAnOrderByIdAsync([FromRoute] Guid orderId)
        {
            return await _orderService.DeleteOrderByIdAsync(orderId); // Will be modified later
        }

        private Guid GetUserIdClaim()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new Exception("User ID claim not found");
            }
            if (!Guid.TryParse(userIdClaim.Value, out var userId))
            {
                throw new Exception("Invalid user ID format");
            }
            return userId;
        }

        // Update order product information (Quantity, productId, ...) -> Will implement later.
        // UpdateOrderStatusAsync(OrderId, OrderUpdateDto orderUpdate);
    }
}
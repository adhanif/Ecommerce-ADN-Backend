using System.Security.Claims;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.ValueObject;
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
        private IUserService _userService;

        public OrderController(IOrderService orderService, IUserService userService)
        {
            _userService = userService;
            _orderService = orderService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<OrderReadDto>>> GetAllOrdersAsync([FromQuery] BaseQueryOptions options)
        {
            var orders = await _orderService.GetAllOrdersAsync(options);
            return Ok(orders);
        }

        [Authorize]
        [HttpPost()]
        public async Task<ActionResult<OrderReadDto>> CreateOrderAsync([FromBody] OrderCreateDto orderCreateDto)
        {
            var userId = GetUserIdClaim();
            var createdOrder = await _orderService.CreateOrderAsync(userId, orderCreateDto);
            return Ok(createdOrder);
        }


        //GET http://localhost:5227/api/v1/orders/user/:userId
        [Authorize(Roles = "Admin")]
        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderReadDto>> GetOrderByIdAsync([FromRoute] Guid orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            return Ok(order);
        }




        [Authorize(Roles = "Admin")]
        [HttpDelete("{orderId}")]
        public async Task<ActionResult<bool>> DeleteAnOrderByIdAsync([FromRoute] Guid orderId)
        {
            var deletedOrder = await _orderService.DeleteOrderByIdAsync(orderId);
            return Ok(deletedOrder);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{orderId}")]
        public async Task<ActionResult<OrderUpdateDto>> UpdateOrderStatusAsync([FromRoute] Guid orderId, [FromBody] OrderUpdateDto orderUpdateDto)
        {

            var updatedOrder = await _orderService.UpdateOrderAsync(orderId, orderUpdateDto.OrderStatus.Value);
            return Ok(updatedOrder);
        }


        [HttpGet("user/{userId}")]
        public async Task<ActionResult<OrderReadDto>> GetOrdersByUserIdAsync([FromRoute] Guid userId)
        {
            var authenticatedClaims = HttpContext.User;
            var foundId = authenticatedClaims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            UserReadDto foundUser = await _userService.GetUserByIdAsync(userId);
            if (foundUser is null)
            {
                return NotFound();
            }
            if (Guid.Parse(foundId) != foundUser.Id)
            {
                return Forbid();
            }

            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
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


    }
}
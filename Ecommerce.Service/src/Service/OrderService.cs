using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.RepoAbstract;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;
using AutoMapper;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.ValueObject;

namespace Ecommerce.Service.src.Service
{
    public class OrderService : IOrderService
    {
        private IMapper _mapper;
        private readonly IOrderRepo _orderRepo;
        private IProductRepo _productRepo;
        private IUserRepo _userRepo;

        public OrderService(IOrderRepo orderRepo, IMapper mapper, IProductRepo productRepo, IUserRepo userRepo)
        {
            _mapper = mapper;
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _userRepo = userRepo;
        }

        public async Task<OrderReadDto> CreateOrderAsync(Guid userId, OrderCreateDto orderCreateDto)
        {
            var foundUser = await _userRepo.GetUserByIdAsync(userId);
            if (foundUser is null)
            {
                throw AppException.NotFound("User not found");
            }

            var order = _mapper.Map<Order>(orderCreateDto);
            order.User = foundUser;
            var newOrderProducts = new List<OrderProduct>();
            foreach (var orderProductDto in orderCreateDto.OrderProducts)
            {
                var foundProduct = await _productRepo.GetProductByIdAsync(orderProductDto.ProductId);
                if (foundProduct is null)
                {
                    throw AppException.NotFound("Product not found");
                }
                newOrderProducts.Add(new OrderProduct
                {
                    Product = foundProduct,
                    Quantity = orderProductDto.Quantity
                });
            }
            order.OrderProducts = newOrderProducts;
            order.Status = OrderStatus.Pending;

            var createdOrder = await _orderRepo.CreateOrderAsync(order);
            var orderReadDto = _mapper.Map<OrderReadDto>(createdOrder);
            return orderReadDto;
        }

        public async Task<bool> DeleteOrderByIdAsync(Guid orderId)
        {
            if (orderId == Guid.Empty)
            {
                AppException.BadRequest("OrderId is required");
            }
            try
            {
                var targetOrder = await _orderRepo.GetOrderByIdAsync(orderId);
                if (targetOrder is not null)
                {
                    await _orderRepo.DeleteOrderByIdAsync(orderId);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<OrderReadDto>> GetAllOrdersAsync(BaseQueryOptions? options)
        {
            var orders = await _orderRepo.GetAllOrdersAsync(options);
            var orderDtos = new List<OrderReadDto>();

            foreach (var order in orders)
            {
                var orderDto = await MapOrderToDTO(order);
                orderDtos.Add(orderDto);
            }

            return orderDtos;
        }

        public async Task<OrderReadDto> GetOrderByIdAsync(Guid orderId)
        {
            if (orderId == Guid.Empty)
            {
                AppException.BadRequest("OrderId is required");
            }
            try
            {
                var foundOrder = await _orderRepo.GetOrderByIdAsync(orderId);
                if (foundOrder is null)
                {
                    throw AppException.NotFound("Order not found");
                }
                var orderDto = _mapper.Map<OrderReadDto>(foundOrder);
                var user = await _userRepo.GetUserByIdAsync(foundOrder.UserId);
                orderDto.User = _mapper.Map<UserReadDto>(user);
                orderDto.OrderProducts = _mapper.Map<IEnumerable<OrderProductReadDto>>(orderDto.OrderProducts);
                return orderDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OrderReadUpdateDto> UpdateOrderByIdAsync(Guid orderId, OrderUpdateDto orderUpdateDto)
        {
            var foundOrder = await _orderRepo.GetOrderByIdAsync(orderId);

            if (orderId == Guid.Empty)
            {
                throw AppException.BadRequest("Order id is required");
            }
            if (foundOrder is null)
            {
                throw AppException.NotFound($"Order not found");
            }

            // Update order status and date
            foundOrder.Status = orderUpdateDto.OrderStatus;
            foundOrder.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);

            // Save changes
            var updatedOrder = await _orderRepo.UpdateOrderByIdAsync(foundOrder);

            // Fetch user information
            var user = await _userRepo.GetUserByIdAsync(updatedOrder.UserId);

            var orderDto = _mapper.Map<OrderReadUpdateDto>(updatedOrder);
            orderDto.User = _mapper.Map<UserReadDto>(user);
            orderDto.OrderStatus = orderUpdateDto.OrderStatus;

            return orderDto;
        }

        #region Helper class
        private async Task<OrderReadDto> MapOrderToDTO(Order order)
        {
            var orderDto = _mapper.Map<OrderReadDto>(order);
            var user = await _userRepo.GetUserByIdAsync(order.UserId);

            // Map User
            orderDto.User = _mapper.Map<UserReadDto>(user);

            // Map OrderProducts
            await MapOrderProductsToDTOs(order, orderDto);

            return orderDto;

        }

        private async Task MapOrderProductsToDTOs(Order order, OrderReadDto orderDto)
        {
            if (order.OrderProducts != null && order.OrderProducts.Any())
            {
                var orderProductDtos = new List<OrderProductReadDto>();

                foreach (var orderProduct in order.OrderProducts)
                {
                    var product = await _productRepo.GetProductByIdAsync(orderProduct.ProductId);
                    var productDTO = _mapper.Map<ProductReadDto>(product);

                    var orderProductDto = _mapper.Map<OrderProductReadDto>(orderProduct);
                    orderProductDto.ProductTitle = productDTO.ProductTitle;
                    orderProductDto.ProductPrice = productDTO.ProductPrice;

                    orderProductDtos.Add(orderProductDto);
                }

                orderDto.OrderProducts = orderProductDtos;
            }
        }

        #endregion
    }
}
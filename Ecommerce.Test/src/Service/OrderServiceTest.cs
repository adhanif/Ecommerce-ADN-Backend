using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AutoMapper;
using Ecommerce.Service.src.Shared;
using Ecommerce.Core.src.RepoAbstract;
using Ecommerce.Service.src.Service;
using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.ValueObject;


namespace Ecommerce.Test.src.Service
{
    public class OrderServiceTest
    {
        private static IMapper _mapper;
        public OrderServiceTest()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(m =>
                {
                    m.AddProfile(new MapperProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [Fact]
        public void DeleteOrderByIdAsync_ValidId_ShouldReturnTrue()
        {
            // Arrange
            var mockOrderRepo = new Mock<IOrderRepo>();
            var mockMapper = new Mock<IMapper>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockProductRepo = new Mock<IProductRepo>();
            var orderService = new OrderService(mockOrderRepo.Object, mockMapper.Object, mockProductRepo.Object, mockUserRepo.Object);
            var orderId = Guid.NewGuid();
            var targetOrder = new Order { Id = orderId };

            // Act
            mockOrderRepo.Setup(orderRepo => orderRepo.GetOrderByIdAsync(orderId)).ReturnsAsync(targetOrder);
            var result = orderService.DeleteOrderByIdAsync(orderId);

            // Assert
            Assert.True(result.Result);
            mockOrderRepo.Verify(orderRepo => orderRepo.DeleteOrderByIdAsync(orderId), Times.Once);
        }

        [Fact]
        public void DeleteOrderByIdAsync_EmptyId_ShouldThrowsException()
        {
            // Arrange
            var mockOrderRepo = new Mock<IOrderRepo>();
            var mockMapper = new Mock<IMapper>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockProductRepo = new Mock<IProductRepo>();
            var orderService = new OrderService(mockOrderRepo.Object, mockMapper.Object, mockProductRepo.Object, mockUserRepo.Object);
            var orderId = Guid.Empty;

            // Assert
            Assert.ThrowsAsync<Exception>(() => orderService.DeleteOrderByIdAsync(orderId));
            mockOrderRepo.Verify(orderRepo => orderRepo.DeleteOrderByIdAsync(It.IsAny<Guid>()), Times.Never());
        }

        [Fact]
        public void DeleteOrderByIdAsync_NonExistedId_ShouldReturnFalse()
        {
            // Arrange
            var mockOrderRepo = new Mock<IOrderRepo>();
            var mockMapper = new Mock<IMapper>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockProductRepo = new Mock<IProductRepo>();
            var orderService = new OrderService(mockOrderRepo.Object, mockMapper.Object, mockProductRepo.Object, mockUserRepo.Object);
            var orderId = Guid.NewGuid();

            // Act
            mockOrderRepo.Setup(orderRepo => orderRepo.GetOrderByIdAsync(orderId)).ReturnsAsync(() => null);
            var result = orderService.DeleteOrderByIdAsync(orderId);

            // Assert
            Assert.False(result.Result);
            mockOrderRepo.Verify(repo => repo.DeleteOrderByIdAsync(It.IsAny<Guid>()), Times.Never());
        }

        [Fact]
        public void CreateOrderAsync_ValidInput_ShouldReturnOrderReadDto()
        {
            // Arrange
            var mockOrderRepo = new Mock<IOrderRepo>();
            var mockMapper = new Mock<IMapper>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockProductRepo = new Mock<IProductRepo>();
            var orderService = new OrderService(mockOrderRepo.Object, _mapper, mockProductRepo.Object, mockUserRepo.Object);

            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                Name = "Test User",
                Email = "testuser@gmail.com",
                Password = "123456",
                Avatar = "sample.jpg",
                UserRole = Core.src.ValueObject.UserRole.Customer
            };

            mockUserRepo.Setup(userRepo => userRepo.GetUserByIdAsync(existingUser.Id)).Returns(Task.FromResult(existingUser));

            var orderProducts = new List<OrderProduct>()
            {
                new OrderProduct { ProductId = Guid.NewGuid(), Quantity = 2 },
                new OrderProduct { ProductId = Guid.NewGuid(), Quantity = 3 }
            };
            var orderProductCreateDtos = new List<OrderProductCreateDto>
            {
                new OrderProductCreateDto {ProductId = Guid.NewGuid(), Quantity = 2 },
                new OrderProductCreateDto { ProductId = Guid.NewGuid(), Quantity = 3 }
            };
            var newOrder = new OrderCreateDto { OrderProducts = orderProductCreateDtos };

            mockMapper.Setup(mapper => mapper.Map<List<OrderProduct>>(It.IsAny<List<OrderProductCreateDto>>())).Returns((List<OrderProductCreateDto> input) => orderProducts);
            mockOrderRepo.Setup(orderRepo => orderRepo.CreateOrderAsync(existingUser.Id, It.IsAny<List<OrderProduct>>())).Returns((Guid uid, List<OrderProduct> products) =>
            {
                var order = new Order();
                order.OrderProducts = new List<OrderProduct>();
                return order;
            });

            // Act
            var result = orderService.CreateOrderAsync(existingUser.Id, newOrder);

            // Assert
            Assert.IsType<Task<OrderReadDto>>(result.Result);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public void GetAllOrders_ShouldReturnAllOrderReadDtos()
        {
            // Arrange
            var mockOrderRepo = new Mock<IOrderRepo>();
            var mockMapper = new Mock<IMapper>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockProductRepo = new Mock<IProductRepo>();
            var orderService = new OrderService(mockOrderRepo.Object, mockMapper.Object, mockProductRepo.Object, mockUserRepo.Object);

            var orders = new List<Order>
            {
                new Order { Id = Guid.NewGuid() },
                new Order { Id = Guid.NewGuid() }
            };

            var orderDtos = new List<OrderReadDto>
            {
                new OrderReadDto { Id = orders[0].Id },
                new OrderReadDto { Id = orders[1].Id }
            };

            mockOrderRepo.Setup(orderRepo => orderRepo.GetAllOrdersAsync(It.IsAny<BaseQueryOptions>())).Returns(Task.FromResult<IEnumerable<Order>>(orders));
            mockMapper.Setup(mapper => mapper.Map<Order, OrderReadDto>(It.IsAny<Order>())).Returns((Order order) => orderDtos.FirstOrDefault(dto => dto.Id == order.Id));

            // Act
            var result = orderService.GetAllOrdersAsync(new BaseQueryOptions());

            // Assert
            Assert.IsType<Task<IEnumerable<OrderReadDto>>>(result);
            Assert.NotNull(result.Result);
            Assert.Equal(orders.Count, result.Result.Count());
        }

        [Fact]
        public void GetOrderByIdAsync_ValidId_ShouldReturnOrderReadDto()
        {
            // Arrange
            var mockOrderRepo = new Mock<IOrderRepo>();
            var mockMapper = new Mock<IMapper>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockProductRepo = new Mock<IProductRepo>();
            var orderService = new OrderService(mockOrderRepo.Object, mockMapper.Object, mockProductRepo.Object, mockUserRepo.Object);

            var orderId = Guid.NewGuid();
            var order = new Order { Id = orderId };
            var orderDto = new OrderReadDto { Id = orderId };

            mockOrderRepo.Setup(orderRepo => orderRepo.GetOrderByIdAsync(orderId)).Returns(Task.FromResult(order));
            mockMapper.Setup(mapper => mapper.Map<OrderReadDto>(It.IsAny<Order>())).Returns(orderDto);

            // Act
            var result = orderService.GetOrderByIdAsync(orderId);

            // Assert
            Assert.IsType<Task<OrderReadDto>>(result);
            Assert.NotNull(result.Result);
            Assert.Equal(orderId, result.Result.Id);
        }

        [Fact]
        public void GetOrderByIdAsync_EmptyId_ShouldThrowsException()
        {
            var mockOrderRepo = new Mock<IOrderRepo>();
            var mockMapper = new Mock<IMapper>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockProductRepo = new Mock<IProductRepo>();
            var orderService = new OrderService(mockOrderRepo.Object, mockMapper.Object, mockProductRepo.Object, mockUserRepo.Object);

            var orderId = Guid.Empty;

            // Assert
            Assert.ThrowsAsync<Exception>(() => orderService.GetOrderByIdAsync(orderId));
            mockOrderRepo.Verify(orderRepo => orderRepo.GetOrderByIdAsync(It.IsAny<Guid>()), Times.Never());
        }

        [Fact]
        public void GetOrderByIdAsync_NonExistedId_ShouldReturnNull()
        {
            var mockOrderRepo = new Mock<IOrderRepo>();
            var mockMapper = new Mock<IMapper>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockProductRepo = new Mock<IProductRepo>();
            var orderService = new OrderService(mockOrderRepo.Object, mockMapper.Object, mockProductRepo.Object, mockUserRepo.Object);

            var orderId = Guid.NewGuid();

            mockOrderRepo.Setup(orderRepo => orderRepo.GetOrderByIdAsync(orderId)).Returns(Task.FromResult<Order>(null));

            // Act
            var result = orderService.GetOrderByIdAsync(orderId);

            // Assert
            Assert.Null(result.Result);
        }

        [Fact]
        public void UpdateOrderByIdAsync_ValidInput_ShouldReturnOrderReadUpdateDto()
        {
            var mockOrderRepo = new Mock<IOrderRepo>();
            var mockMapper = new Mock<IMapper>();
            var mockUserRepo = new Mock<IUserRepo>();
            var mockProductRepo = new Mock<IProductRepo>();
            var orderService = new OrderService(mockOrderRepo.Object, mockMapper.Object, mockProductRepo.Object, mockUserRepo.Object);

            var orderId = Guid.NewGuid();
            var updatedOrder = new OrderUpdateDto { OrderStatus = OrderStatus.Processing };
            var targetOrder = new Order { Status = OrderStatus.Pending, UserId = Guid.NewGuid() };

            mockOrderRepo.Setup(repo => repo.GetOrderByIdAsync(orderId)).Returns(Task.FromResult(targetOrder));
            mockMapper.Setup(mapper => mapper.Map<OrderReadDto>(It.IsAny<Order>())).Returns(new OrderReadDto { OrderStatus = OrderStatus.Processing });

            // Mock the user retrieval when calling GetById on UserRepo
            mockUserRepo.Setup(userRepo => userRepo.GetUserByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new User()));

            // Act
            var result = orderService.UpdateOrderByIdAsync(orderId, updatedOrder);

            // Assert
            Assert.IsType<Task<OrderReadUpdateDto>>(result);
            Assert.NotNull(result.Result);
            Assert.Equal(updatedOrder.OrderStatus, result.Result.OrderStatus);
        }
    }
}
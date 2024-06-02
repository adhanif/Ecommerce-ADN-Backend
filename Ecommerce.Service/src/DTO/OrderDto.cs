using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.ValueObject;

namespace Ecommerce.Service.src.DTO
{
    public class OrderReadDto : BaseEntity
    {
        public UserReadDto User { get; set; } // User information
        public IEnumerable<OrderProductReadDto> OrderProducts { get; set; }
        public string Address { get; set; }
        public int Total { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }

    public class OrderCreateDto
    {
        public IEnumerable<OrderProductCreateDto> OrderProducts { get; set; }
        public string Address { get; set; }
        public int Total { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }

    public class OrderUpdateDto
    {

        public OrderStatus? OrderStatus { get; set; }
        public IEnumerable<OrderProductUpdateDto>? OrderProducts { get; set; }
    }

    public class OrderReadUpdateDto : BaseEntity
    {
        // public UserReadDto User { get; set; } // User information
        public OrderStatus OrderStatus { get; set; }
        public IEnumerable<OrderProductReadDto> OrderProducts { get; set; } // Order products list
        public string Address { get; set; }
        public int Total { get; set; }
    }
}
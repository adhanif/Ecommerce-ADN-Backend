using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Core.src.ValueObject;

namespace Ecommerce.Core.src.Entity
{
    public class Order : BaseEntity
    {
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Column(TypeName = "varchar")]
        public string Address { get; set; }
        public int Total { get; set; }
        public OrderStatus Status { get; set; }
        public IEnumerable<OrderProduct> OrderProducts { get; set; }
    }

    
}
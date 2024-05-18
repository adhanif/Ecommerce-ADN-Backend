using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Core.src.Entity
{
    public class Product : BaseEntity
    {
        [Required]
        [Column(TypeName = "varchar")]
        public string Title { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        public string Description { get; set; }

        [Required]
        public int Price { get; set; }

        [ForeignKey("CategoryId")]
        public Guid CategoryId { get; set; } //  foreign key navigate to category

        [Required, Range(0, int.MaxValue, ErrorMessage = "Inventory should not be negative number")]
        public int Inventory { get; set; } = 0;

        [Required]
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

        override public string ToString()
        {
            return $"Product Title: {Title}, Product Description: {Description}, Product Price: {Price},Product CategoryId: {CategoryId}, Inventory:{Inventory} ";
        }
    }
}
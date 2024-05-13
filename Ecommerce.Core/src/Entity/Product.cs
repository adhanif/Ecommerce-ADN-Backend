using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Core.src.Entity
{
    public class Product : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        [ForeignKey("CategoryId")]
        public Guid CategoryId { get; set; } //  foreign key navigate to category
        public int Inventory { get; set; } = 0;
        public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

        override public string ToString()
        {
            return $"Product Title: {Title}, Product Description: {Description}, Product Price: {Price},Product CategoryId: {CategoryId}, Inventory:{Inventory} ";
        }
    }
}
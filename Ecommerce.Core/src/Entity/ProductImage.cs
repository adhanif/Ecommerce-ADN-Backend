using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Core.src.Validation;

namespace Ecommerce.Core.src.Entity
{
    public class ProductImage : BaseEntity
    {
        // [UrlValidation]
        // public string Url { get; set; }
        [ForeignKey("ProductId")]
        public Guid ProductId { get; set; } // Foreign key navigate to product
        public byte[] Data { get; set; }
    }
}
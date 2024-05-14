using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Core.src.Validation;

namespace Ecommerce.Core.src.Entity
{
    public class Address : BaseEntity
    {
        [Column(TypeName = "character varying(50)")]
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        [ZipCodeValidation]
        public string ZipCode { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
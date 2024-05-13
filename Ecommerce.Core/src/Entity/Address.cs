using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ecommerce.Core.src.Validation;

namespace Ecommerce.Core.src.Entity
{
    public class Address
    {
        [Column(TypeName = "character varying(50)")]
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        [ZipCodeValidation]
        public string ZipCode { get; set; }
        public string HouseNumber { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}
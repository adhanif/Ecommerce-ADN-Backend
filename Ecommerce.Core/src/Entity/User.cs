using System.ComponentModel.DataAnnotations;
using Ecommerce.Core.src.Validation;
using Ecommerce.Core.src.ValueObject;

namespace Ecommerce.Core.src.Entity
{
    public class User : BaseEntity
    {
        [StringLength(1, ErrorMessage = "Name must not be empty")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [StrongPassword]
        public string Password { get; set; }
        public byte[] Salt { get; set; }
        
        public string? Avatar { get; set; }
        public UserRole Role { get; set; }
        public IEnumerable<Address> Addresses { get; set; }


        override public string ToString()
        {
            return $"User Name: {Name}, User Email: {Email}, User Avatar: {Avatar}, User Role: {Role}";
        }
    }
}
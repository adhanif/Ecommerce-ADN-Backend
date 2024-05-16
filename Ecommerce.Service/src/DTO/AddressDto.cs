using System.Data.Common;
using Ecommerce.Core.src.Entity;

namespace Ecommerce.Service.src.DTO
{
    public class AddressReadDto : BaseEntity
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public Guid UserId { get; set; } //  foreign key navigate to user
        public UserReadDto User { get; set; }

        public void Transform(Address address)
        {
            {
                Id = address.Id;
                Street = address.Street;
                City = address.City;
                Country = address.Country;
                ZipCode = address.ZipCode;
                PhoneNumber = address.PhoneNumber;
                CreatedDate = address.CreatedDate;
                UpdatedDate = address.UpdatedDate;
                UserId = address.UserId;
            }
        }
    }
    public class AddressCreateDto
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public Guid UserId { get; set; }

        public Address CreateAddress()
        {
            return new Address
            {
                Street = Street,
                City = City,
                Country = Country,
                ZipCode = ZipCode,
                PhoneNumber = PhoneNumber,
                UserId = UserId,
            };
        }
    }

    public class AddressUpdateDto
    {
        public Guid AddressId { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? ZipCode { get; set; }
        public string? PhoneNumber { get; set; }
        public Guid UserId { get; set; }

        public Address UpdateAddress(Address oldAddress)
        {

            {
                if (Street != null)
                    oldAddress.Street = Street;

                if (City != null)
                    oldAddress.City = City;

                if (Country != null)
                    oldAddress.Country = Country;

                if (ZipCode != null)
                    oldAddress.ZipCode = ZipCode;

                if (PhoneNumber != null)
                    oldAddress.PhoneNumber = PhoneNumber;

                return oldAddress;
            }
        }
    }
}
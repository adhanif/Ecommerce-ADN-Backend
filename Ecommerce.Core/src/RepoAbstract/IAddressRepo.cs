using Ecommerce.Core.src.Entity;

namespace Ecommerce.Core.src.RepoAbstract
{
    public interface IAddressRepo
    {
        Task<Address> GetAddressByIdAsync(Guid id);
        Task<Address> CreateAddressAsync(Address newAddress);
        Task<Address> UpdateAddressAsync(Address address);
        Task DeleteAddressAsync(Guid id);
    }
}
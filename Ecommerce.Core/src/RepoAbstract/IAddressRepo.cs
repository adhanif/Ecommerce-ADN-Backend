using Ecommerce.Core.src.Entity;

namespace Ecommerce.Core.src.RepoAbstract
{
    public interface IAddressRepo
    {
        Task<Address> GetAddressByIdAsync(Guid addressId);
        Task<Address> CreateAddressAsync(Address newAddress);
        Task<Address> UpdateAddressAsync(Address address);
        Task<bool> DeleteAddressAsync(Guid id);
        Task<IEnumerable<Address>> GetAllAddressesOfUserByIdAsync(Guid userId);
    }
}
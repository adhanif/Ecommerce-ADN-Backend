using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstract
{
    public interface IAddressService
    {
        Task<AddressReadDto> GetAddressByIdAsync(Guid addressId);
        Task<AddressReadDto> CreateAddressAsync(AddressCreateDto addressCreateDto);
        Task<AddressReadDto> UpdateAddressAsync(Address address);
        Task<bool> DeleteAddressAsync(Guid addressId);
        Task<IEnumerable<AddressReadDto>> GetAllAddressesOfUserByIdAsync(Guid userID);
    }
}
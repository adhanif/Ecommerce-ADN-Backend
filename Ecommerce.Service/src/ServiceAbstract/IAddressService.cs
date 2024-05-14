using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstract
{
    public interface IAddressService
    {
        Task<AddressReadDto> GetAddressByIdAsync(Guid id);
        Task<AddressReadDto> CreateAddressAsync(AddressCreateDto addressCreateDto);
        Task<AddressReadDto> UpdateAddressAsync(Address address);
        Task DeleteAddressAsync(Guid id);
    }
}
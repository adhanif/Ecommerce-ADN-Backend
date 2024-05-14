using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstract;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;

namespace Ecommerce.Service.src.Service
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepo _addressRepo;

        public AddressService(IAddressRepo addressRepo)
        {
            _addressRepo = addressRepo;
        }

        public async Task<AddressReadDto> CreateAddressAsync(AddressCreateDto addressCreateDto)
        {
            var newAddress = addressCreateDto.CreateAddress();
            var createdAddress = await _addressRepo.CreateAddressAsync(newAddress);
            var addressReadDto = new AddressReadDto();
            addressReadDto.Transform(createdAddress);
            return addressReadDto;
        }

        public Task DeleteAddressAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<AddressReadDto> GetAddressByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<AddressReadDto> UpdateAddressAsync(Address address)
        {
            throw new NotImplementedException();
        }
    }
}
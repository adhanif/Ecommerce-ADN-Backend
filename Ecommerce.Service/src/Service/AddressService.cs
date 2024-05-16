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
            try
            {
                var newAddress = addressCreateDto.CreateAddress();
                var createdAddress = await _addressRepo.CreateAddressAsync(newAddress);
                var addressReadDto = new AddressReadDto();
                addressReadDto.Transform(createdAddress);
                return addressReadDto;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<IEnumerable<AddressReadDto>> GetAllAddressesOfUserByIdAsync(Guid userId)
        {
            try
            {
                var foundAddresses = await _addressRepo.GetAllAddressesOfUserByIdAsync(userId);
                var addressReadDtos = new List<AddressReadDto>();

                foreach (var address in foundAddresses)
                {
                    var addressReadDto = new AddressReadDto();
                    addressReadDto.Transform(address);
                    addressReadDtos.Add(addressReadDto);
                }

                return addressReadDtos;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<AddressReadDto> GetAddressByIdAsync(Guid addressId)
        {
            try
            {
                var address = await _addressRepo.GetAddressByIdAsync(addressId);
                
                var addressReadDto = new AddressReadDto();
                addressReadDto.Transform(address);
                return addressReadDto;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<bool> DeleteAddressAsync(Guid addressId)
        {
            try
            {
                await _addressRepo.DeleteAddressAsync(addressId);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<AddressReadDto> UpdateAddressAsync(Guid addressId, AddressUpdateDto address)
        {
            var addressFound = await _addressRepo.GetAddressByIdAsync(addressId);

            try
            {
                addressFound.Street = address.Street ?? addressFound.Street;
                addressFound.City = address.City ?? addressFound.City;
                addressFound.Country = address.Country ?? addressFound.Country;
                addressFound.PhoneNumber = address.PhoneNumber ?? addressFound.PhoneNumber;
                addressFound.ZipCode = address.ZipCode ?? addressFound.ZipCode;
                // addressFound.UpdatedDate = DateTime.UtcNow;

                var updatedAddress = await _addressRepo.UpdateAddressAsync(addressFound);
                var addressReadDto = new AddressReadDto();
                addressReadDto.Transform(updatedAddress);
                return addressReadDto;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
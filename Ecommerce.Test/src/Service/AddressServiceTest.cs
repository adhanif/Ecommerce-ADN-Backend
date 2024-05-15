using Moq;
using Xunit;
using Ecommerce.Service.src.Service;
using Ecommerce.Core.src.RepoAbstract;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Core.src.Common;
using Ecommerce.Service.src.DTO;
using Ecommerce.Core.src.Entity;

namespace Ecommerce.Test.src.Service
{
    public class AddressServiceTest
    {
        // private readonly AddressService _addressService;
        private readonly AddressService _addressService;
        private readonly Mock<IAddressRepo> _addressRepoMock = new();


        public AddressServiceTest()
        {
            _addressService = new AddressService(_addressRepoMock.Object);
        }

        [Fact]
        public async Task CreateAddressAsync_ValidAddress_ReturnsAddress()
        {
            // Arrange
            var newAddress = new AddressCreateDto
            {
                Street = "Street",
                City = "City",
                Country = "Country",
                ZipCode = "12345",
                PhoneNumber = "131313",
                UserId = Guid.NewGuid(),
            };
            var createdAddress = new Address
            {
                Street = newAddress.Street,
                City = newAddress.City,
                Country = newAddress.Country,
                ZipCode = newAddress.ZipCode,
                PhoneNumber = newAddress.PhoneNumber,

            };

            _addressRepoMock.Setup(repo => repo.CreateAddressAsync(It.IsAny<Address>()))
                            .ReturnsAsync(createdAddress);

            // Act
            var result = await _addressService.CreateAddressAsync(newAddress);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newAddress.Street, result.Street);
            Assert.Equal(newAddress.City, result.City);
            Assert.Equal(newAddress.Country, result.Country);
            Assert.Equal(newAddress.ZipCode, result.ZipCode);
            Assert.Equal(newAddress.PhoneNumber, result.PhoneNumber);
        }


        [Fact]
        public async Task GetAllAddressesOfUserByIdAsync_ValidUserId_ReturnsAddressReadDtos()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var newAddress1 = new AddressCreateDto
            {
                Street = "Street",
                City = "City",
                Country = "Country",
                ZipCode = "12345",
                PhoneNumber = "131313",
                UserId = Guid.NewGuid(),
            };
            var createdAddress1 = new Address
            {
                Street = newAddress1.Street,
                City = newAddress1.City,
                Country = newAddress1.Country,
                ZipCode = newAddress1.ZipCode,
                PhoneNumber = newAddress1.PhoneNumber,

            };
            var newAddress2 = new AddressCreateDto
            {
                Street = "Street",
                City = "City",
                Country = "Country",
                ZipCode = "12345",
                PhoneNumber = "131313",
                UserId = Guid.NewGuid(),
            };
            var createdAddress2 = new Address
            {
                Street = newAddress2.Street,
                City = newAddress2.City,
                Country = newAddress2.Country,
                ZipCode = newAddress2.ZipCode,
                PhoneNumber = newAddress2.PhoneNumber,
            };

            var expectedAddresses = new List<Address> { createdAddress1, createdAddress2 };

            _addressRepoMock.Setup(repo => repo.GetAllAddressesOfUserByIdAsync(userId))
                            .ReturnsAsync(expectedAddresses);

            // Act
            var result = await _addressService.GetAllAddressesOfUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<AddressReadDto>>(result);
            Assert.Equal(expectedAddresses.Count, result.Count());
        }

        [Fact]
        public async Task GetAllAddressesOfUserByIdAsync_InvalidUserId_ReturnsEmptyList()
        {
            // Arrange
            var userId1 = Guid.Empty;
            // Arrange
            var userId = Guid.NewGuid();
            var newAddress1 = new AddressCreateDto
            {
                Street = "Street",
                City = "City",
                Country = "Country",
                ZipCode = "12345",
                PhoneNumber = "131313",
                UserId = Guid.NewGuid(),
            };
            var createdAddress1 = new Address
            {
                Street = newAddress1.Street,
                City = newAddress1.City,
                Country = newAddress1.Country,
                ZipCode = newAddress1.ZipCode,
                PhoneNumber = newAddress1.PhoneNumber,

            };
            var newAddress2 = new AddressCreateDto
            {
                Street = "Street",
                City = "City",
                Country = "Country",
                ZipCode = "12345",
                PhoneNumber = "131313",
                UserId = Guid.NewGuid(),
            };
            var createdAddress2 = new Address
            {
                Street = newAddress2.Street,
                City = newAddress2.City,
                Country = newAddress2.Country,
                ZipCode = newAddress2.ZipCode,
                PhoneNumber = newAddress2.PhoneNumber,
            };

            var expectedAddresses = new List<Address> { createdAddress1, createdAddress2 };

            _addressRepoMock.Setup(repo => repo.GetAllAddressesOfUserByIdAsync(userId))
                                      .ReturnsAsync(expectedAddresses);
            // Act
            var result = await _addressService.GetAllAddressesOfUserByIdAsync(userId1);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAddressByIdAsync_ExistingAddressId_ReturnsAddressReadDto()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var address = new Address
            {
                Id = addressId,
                Street = "Street",
                City = "City",
                Country = "Country",
                ZipCode = "12345",
                PhoneNumber = "131313",
                UserId = Guid.NewGuid()
            };

            _addressRepoMock.Setup(repo => repo.GetAddressByIdAsync(addressId))
                            .ReturnsAsync(address);

            // Act
            var result = await _addressService.GetAddressByIdAsync(addressId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<AddressReadDto>(result);
            Assert.Equal(address.Street, result.Street);
            Assert.Equal(address.City, result.City);
            Assert.Equal(address.Country, result.Country);
            Assert.Equal(address.ZipCode, result.ZipCode);
            Assert.Equal(address.PhoneNumber, result.PhoneNumber);
        }

        [Fact]
        public async Task GetAddressByIdAsync_NonExistingAddressId_ReturnsNull()
        {
            // Arrange
            var invalidAddressId = Guid.NewGuid();

            var expectedAddress = new Address
            {
                Id = invalidAddressId,
                Street = "Street",
                City = "City",
                Country = "Country",
                ZipCode = "12345",
                PhoneNumber = "131313",
                UserId = Guid.NewGuid(),
            };


            _addressRepoMock.Setup(repo => repo.GetAddressByIdAsync(invalidAddressId)).ThrowsAsync(AppException.NotFound("Address not found")); ;


            // Act
            var exception = await Assert.ThrowsAsync<AppException>(async () => await _addressService.GetAddressByIdAsync(invalidAddressId));

            // Assert
            Assert.Equal("Address not found", exception.Message);
        }

        [Fact]
        public async Task DeleteAddressAsync_ExistingAddressId_DeletesAddress()
        {
            // Arrange
            var addressId = Guid.NewGuid();

            _addressRepoMock.Setup(repo => repo.DeleteAddressAsync(addressId))
                            .ReturnsAsync(true);

            // Act
            var result = await _addressService.DeleteAddressAsync(addressId);

            // Assert
            Assert.True(result);
            _addressRepoMock.Verify(repo => repo.DeleteAddressAsync(addressId), Times.Once);
        }

        [Fact]
        public async Task DeleteAddressAsync_NonExistingAddressId_ReturnsFalse()
        {
            // Arrange
            var addressId = Guid.NewGuid();

            _addressRepoMock.Setup(repo => repo.DeleteAddressAsync(addressId)).ReturnsAsync(true);

            // Act
            var result = await _addressService.DeleteAddressAsync(addressId);

            // Assert
            Assert.True(result);
            _addressRepoMock.Verify(repo => repo.DeleteAddressAsync(addressId), Times.Once);
        }
        [Fact]
        public async Task UpdateAddressAsync_ExistingAddress_ReturnsUpdatedAddress()
        {
            // Arrange
            var address = new Address
            {
                Id = Guid.NewGuid(),
                Street = "Old Street",
                City = "Old City",
                Country = "Old Country",
                ZipCode = "12345",
                PhoneNumber = "131313",
                UserId = Guid.NewGuid(),
            };

            var updatedAddress = new Address
            {
                Id = address.Id,
                Street = "New Street",
                City = "New City",
                Country = "New Country",
                ZipCode = "54321",
                PhoneNumber = "313131",
                UserId = address.UserId,
            };

            _addressRepoMock.Setup(repo => repo.UpdateAddressAsync(address))
                            .ReturnsAsync(updatedAddress); // Simulate successful update
            // Act
            var result = await _addressService.UpdateAddressAsync(address);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedAddress.Street, result.Street);
            Assert.Equal(updatedAddress.City, result.City);
            Assert.Equal(updatedAddress.Country, result.Country);
            Assert.Equal(updatedAddress.ZipCode, result.ZipCode);
            Assert.Equal(updatedAddress.PhoneNumber, result.PhoneNumber);
            Assert.Equal(updatedAddress.UserId, result.UserId);
            _addressRepoMock.Verify(repo => repo.UpdateAddressAsync(address), Times.Once);
        }

        [Fact]
        public async Task UpdateAddressAsync_NonExistingAddress_ThrowsException()
        {
            // Arrange
            var address = new Address
            {
                Id = Guid.NewGuid(),
                Street = "Street",
                City = "City",
                Country = "Country",
                ZipCode = "12345",
                PhoneNumber = "131313",
                UserId = Guid.NewGuid(),
            };
           
            _addressRepoMock.Setup(repo => repo.UpdateAddressAsync(address))
                           .ThrowsAsync(AppException.NotFound("Address not found")); // Simulate non-existing address

            // Act & Assert
            var exception = await Assert.ThrowsAsync<AppException>(async () => await _addressService.UpdateAddressAsync(address));
    
            Assert.Equal("Address not found", exception.Message);
        }


    }
}
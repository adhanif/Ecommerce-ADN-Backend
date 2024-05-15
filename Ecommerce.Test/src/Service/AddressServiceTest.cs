using Moq;
using Xunit;
using System;

namespace Ecommerce.Test.src.Service
{
    public class AddressServiceTest
    {
        private readonly AddressService _addressService;
        private readonly Mock<IAddressRepo> _addressRepoMock = new();

        public AddressServiceTest()
        {
            _addressService = new AddressService(_addressRepoMock.Object);
        }

        [Fact]
        public async Task CreateAddressAsync_ValidAddress_ReturnsAddressReadDto()
        {
            // Arrange
            var addressCreateDto = new AddressCreateDto
            {
                // Set properties of valid address create DTO
            };
            var createdAddress = new Address
            {
                // Set properties of created address
            };

            _addressRepoMock.Setup(repo => repo.CreateAddressAsync(It.IsAny<Address>()))
                            .ReturnsAsync(createdAddress);

            // Act
            var result = await _addressService.CreateAddressAsync(addressCreateDto);

            // Assert
            Assert.NotNull(result);
            // Add more assertions as needed
        }



    }
}
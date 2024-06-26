using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/addresses")]


    public class AddressController : ControllerBase
    {
        private IAddressService _addressService;
        private IAuthorizationService _authorizationService;

        public AddressController(IAddressService addressService, IAuthorizationService authorizationService)
        {
            _addressService = addressService;
            _authorizationService = authorizationService;
        }

        [Authorize()]
        [HttpPost("")]
        public async Task<ActionResult<AddressReadDto>> CreateAddressAsync([FromBody] AddressCreateDto addressCreateDto)
        {
            var createdAddress = await _addressService.CreateAddressAsync(addressCreateDto);
            if (createdAddress == null)
            {
                return NotFound("Failed to create address.");
            }
            return Ok(createdAddress);
        }

        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<AddressReadDto>>> GetAllAddressesOfUserByIdAsync([FromRoute] Guid userId)
        {

            var authenticatedClaims = HttpContext.User;
            var foundUserId = authenticatedClaims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;


            if (Guid.Parse(foundUserId) != userId)
            {
                return Forbid();
            }

            var addresses = await _addressService.GetAllAddressesOfUserByIdAsync(userId);
            return Ok(addresses);
        }

        [Authorize]
        [HttpGet("{addressId}")]
        public async Task<ActionResult<AddressReadDto>> GetAddressByIdAsync([FromRoute] Guid addressId)
        {
            var authenticatedClaims = HttpContext.User;
            var foundId = authenticatedClaims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

            var foundAddress = await _addressService.GetAddressByIdAsync(addressId);

            if (foundAddress == null)
            {
                return NotFound("Address not found.");
            }

            if (Guid.Parse(foundId) != foundAddress.UserId)
            {
                return Forbid();
            }
            return Ok(foundAddress);
        }

        [Authorize]
        [HttpPut("{addressId}")]
        public async Task<ActionResult<AddressReadDto>> UpdateAddressAsync([FromRoute] Guid addressId, [FromBody] AddressUpdateDto addressUpdateDto)
        {

            var authenticatedClaims = HttpContext.User;
            var foundId = authenticatedClaims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

            if (Guid.Parse(foundId) != addressUpdateDto.UserId)
            {
                return Forbid();
            }

            var updatedAddress = await _addressService.UpdateAddressAsync(addressId, addressUpdateDto);
            return Ok(updatedAddress);
        }

        [Authorize()]
        [HttpDelete("{addressId}")]
        public async Task<ActionResult<bool>> DeleteAddressAsync([FromRoute] Guid addressId)
        {
            var authenticatedClaims = HttpContext.User;
            var foundId = authenticatedClaims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

            var foundAddress = await _addressService.GetAddressByIdAsync(addressId);

            if (Guid.Parse(foundId) != foundAddress.UserId)
            {
                return Forbid();
            }
            var deleteResult = await _addressService.DeleteAddressAsync(addressId);

            if (!deleteResult)
            {
                return NotFound("Failed to delete address.");
            }
            return Ok(true);
        }
    }
}
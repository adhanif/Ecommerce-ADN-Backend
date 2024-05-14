using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/auth/address")]


    public class AddressController : ControllerBase
    {
        private IAddressService _addressService;
        // edit 1 address by id

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [Authorize()]
        [HttpPost("")]
        public async Task<AddressReadDto> CreateAddressAsync([FromBody] AddressCreateDto addressCreateDto)
        {
            var claims = HttpContext.User;
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value);
            var createdAddress = await _addressService.CreateAddressAsync(addressCreateDto);
            createdAddress.UserId = userId;
            return createdAddress;
        }
    }
}
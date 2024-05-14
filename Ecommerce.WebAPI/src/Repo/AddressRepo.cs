using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstract;
using Ecommerce.WebAPI.src.Database;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebAPI.src.Repo
{
    public class AddressRepo : IAddressRepo
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Address> _address;
        public AddressRepo(AppDbContext context)
        {
            _context = context;
            _address = _context.Addresses;
        }

        public async Task<Address> CreateAddressAsync(Address newAddress)
        {
            await _address.AddAsync(newAddress);
            await _context.SaveChangesAsync();
            return newAddress;
        }

        public async Task<IEnumerable<Address>> GetAllAddressesOfUserByIdAsync(Guid userId)
        {
            var addresses = await _address.Where(a => a.UserId == userId).Include(a => a.User).ToListAsync();
            Console.WriteLine(addresses[0].User);
            return addresses;
        }

        public async Task<bool> DeleteAddressAsync(Guid addressId)
        {
            var foundAddress = await _address.FirstOrDefaultAsync(a => a.Id == addressId) ?? throw AppException.NotFound("Address not found");
            _address.Remove(foundAddress);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Address> GetAddressByIdAsync(Guid addressId)
        {
            var foudAddress = await _address.FirstOrDefaultAsync(a => a.Id == addressId) ?? throw AppException.NotFound("Address not found");
            return foudAddress;
        }


        public async Task<Address> UpdateAddressAsync(Address address)
        {
            var existingAddress = await _address.FirstOrDefaultAsync(a => a.Id == address.Id) ?? throw AppException.NotFound("Address not found");
            existingAddress.Street = address.Street;
            existingAddress.City = address.City;
            existingAddress.Country = address.Country;
            existingAddress.ZipCode = address.ZipCode;
            existingAddress.PhoneNumber = address.PhoneNumber;
            _context.Update(existingAddress);
            await _context.SaveChangesAsync();
            return existingAddress;
        }
    }
}
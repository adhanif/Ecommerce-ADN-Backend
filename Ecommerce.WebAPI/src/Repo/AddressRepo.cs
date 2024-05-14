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

        public Task DeleteAddressAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Address> GetAddressByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Address> UpdateAddressAsync(Address address)
        {
            throw new NotImplementedException();
        }
    }
}
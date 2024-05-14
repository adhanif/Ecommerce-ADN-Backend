using System.Net;
using System.Text.RegularExpressions;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstract;
using Ecommerce.WebAPI.src.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Ecommerce.WebAPI.src.Repo
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _context;
        private readonly DbSet<User> _users;
        public UserRepo(AppDbContext context)
        {
            _context = context;
            _users = _context.Users;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(UserQueryOptions? userQueryOptions)
        {
            var query = _users.AsQueryable();

            // Apply filters if UserQueryOptions is not null
            if (userQueryOptions is not null)
            {
                // Filter by role if provided
                if (userQueryOptions.SearchRole.HasValue)
                {
                    query = query.Where(u => u.UserRole == userQueryOptions.SearchRole.Value);
                }

                // Search by name if provided
                if (!string.IsNullOrEmpty(userQueryOptions.SearchName))
                {
                    query = query.Where(u => u.Name.Contains(userQueryOptions.SearchName));
                }

                if (userQueryOptions.Offset >= 0 && userQueryOptions.Limit > 0)
                {
                    query = query.Skip(userQueryOptions.Offset).Take(userQueryOptions.Limit);
                }

            }

            // Include addresses
            query = query.Include(u => u.Addresses);
            // Execute the query
            var users = await query.ToListAsync();
            return users;
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            var foundUser = await _users.Include(u => u.Addresses).FirstOrDefaultAsync(u => u.Id == userId) ?? throw AppException.NotFound("User not found");
            return foundUser;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var foundUser = await _users.Include(u => u.Addresses).FirstOrDefaultAsync(u => u.Email == email) ?? throw AppException.NotFound("User not found"); ;
            return foundUser;
        }

        public async Task<User> CreateUserAsync(User newUser)
        {
            var duplicatedUser = await _users.FirstOrDefaultAsync(user => user.Email == newUser.Email);
            if (duplicatedUser is not null) throw AppException.DuplicateEmailException();

            var user = await _users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return newUser;

        }

        public async Task<User> UpdateUserByIdAsync(User updatedUser)
        {
            // check if user exist
            var foundUser = await _users.FirstOrDefaultAsync(user => user.Id == updatedUser.Id) ?? throw AppException.NotFound("User not found");

            // check if the new email is duplicated
            var duplicatedUser = await _users.FirstOrDefaultAsync(u => u.Email == updatedUser.Email && u.Id != updatedUser.Id);
            if (duplicatedUser is not null) throw AppException.DuplicateEmailException();

            _users.Update(updatedUser);
            await _context.SaveChangesAsync();
            return updatedUser;
        }

        public async Task<bool> DeleteUserByIdAsync(Guid userId)
        {
            var user = await _users.FindAsync(userId) ?? throw AppException.NotFound("user not found");

            _users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> GetUserByCredentialsAsync(UserCredential userCredential)
        {

            var foundUser = await _users.FirstOrDefaultAsync(user => user.Email == userCredential.Email) ?? throw AppException.InvalidLoginCredentialsException();
            return foundUser;
        }
    }
}
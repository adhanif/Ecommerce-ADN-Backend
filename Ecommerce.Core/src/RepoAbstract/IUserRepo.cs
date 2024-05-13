
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.Common;

namespace Ecommerce.Core.src.RepoAbstract

{
    public interface IUserRepo
    {
        // with the UserQueryOptions, GetAllUsersAsync is serving for 3 endpoints: 
        // get all users, get all users by role and search users by name with pagination
        Task<IEnumerable<User>> GetAllUsersAsync(UserQueryOptions userQueryOptions);
        Task<User> GetUserByIdAsync(Guid userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> CreateUserAsync(User newUser);
        Task<User> UpdateUserByIdAsync(User updatedUser);
        Task<bool> DeleteUserByIdAsync(Guid userId);
        Task<User> GetUserByCredentialsAsync(UserCredential userCredential);
    }
}
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstract
{
    public interface IUserService
    {
        Task<IEnumerable<UserReadDto>> GetAllUsersAsync(UserQueryOptions userQueryOptions);
        Task<UserReadDto> GetUserByIdAsync(Guid userId);
        Task<UserReadDto> GetUserByEmailAsync(string email);
        Task<UserReadDto> CreateUserAsync(UserCreateDto userCreateDto);
        Task<UserReadDto> UpdateUserByIdAsync(Guid userID, UserUpdateDto userUpdateDto);
        Task<bool> DeleteUserByIdAsync(Guid userId);
        Task<UserReadDto> UpdateUserByAdminAsync(Guid userID, UserUpdateDto userUpdateDto);
    }
}
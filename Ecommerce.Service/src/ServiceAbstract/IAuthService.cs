using Ecommerce.Core.src.Common;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.ServiceAbstract
{
    public interface IAuthService
    {
        Task<string> LoginAsync(UserCredential userCredential);
        Task<string> LogoutAsync();
        Task<UserReadDto> GetCurrentProfileAsync(Guid id);
    }
}
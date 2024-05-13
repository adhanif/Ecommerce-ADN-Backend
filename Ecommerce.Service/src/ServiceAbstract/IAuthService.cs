using Ecommerce.Core.src.Common;

namespace Ecommerce.Service.src.ServiceAbstract
{
    public interface IAuthService
    {
        Task<string> LoginAsync(UserCredential userCredential);
        Task<string> LogoutAsync();
    }
}
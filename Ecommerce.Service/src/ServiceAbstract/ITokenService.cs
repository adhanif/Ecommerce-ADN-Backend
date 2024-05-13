using Ecommerce.Core.src.Entity;

namespace Ecommerce.Service.src.ServiceAbstract
{
    public interface ITokenService
    {
        string GetToken(User user);
        Task<string> InvalidateTokenAsync();
    }
}

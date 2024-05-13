
namespace Ecommerce.Service.src.ServiceAbstract
{
    public interface IPasswordService
    {
        string HashPassword(string password, out byte[] salt);
        bool VerifyPassword(string password, string passwordHash, byte[] salt);
    }
}
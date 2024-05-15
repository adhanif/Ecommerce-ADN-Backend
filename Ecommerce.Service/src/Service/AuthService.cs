using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.RepoAbstract;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstract;

namespace Ecommerce.Service.src.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepo _userRepo;
        private ITokenService _tokenService;
        private readonly IPasswordService _passwordService;
        public AuthService(IUserRepo userRepo, ITokenService tokenService, IPasswordService passwordService)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
            _passwordService = passwordService;
        }

        public async Task<string> LoginAsync(UserCredential userCredential)
        {
            var foundUser = await _userRepo.GetUserByEmailAsync(userCredential.Email) ?? throw AppException.NotFound("Email is not registered");

            var isMatch = _passwordService.VerifyPassword(userCredential.Password, foundUser.Password, foundUser.Salt);
            if (isMatch)
            {
                return _tokenService.GetToken(foundUser);
            }
            else
            {
                throw AppException.InvalidLoginCredentialsException("Incorrect password");
            }
        }

        public async Task<string> LogoutAsync()
        {
            return await _tokenService.InvalidateTokenAsync();
        }

        public async Task<UserReadDto> GetCurrentProfileAsync(Guid id)
        {
            try
            {
                var foundUser = await _userRepo.GetUserByIdAsync(id);
                if (foundUser is null)
                {
                    throw AppException.NotFound("Email is not registered");
                }
                var userReadDto = new UserReadDto();
                userReadDto.Transform(foundUser);
                return userReadDto;

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
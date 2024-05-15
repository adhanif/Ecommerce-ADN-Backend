using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstract;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Service.src.Service;
using Ecommerce.Service.src.ServiceAbstract;
using Moq;
using Xunit;

namespace Ecommerce.Test.src.Service
{
    public class AuthServiceTest
    {
        private readonly AuthService _authService;
        private readonly Mock<IUserRepo> _userRepoMock = new();
        private readonly Mock<ITokenService> _tokenServiceMock = new();
        private readonly Mock<IPasswordService> _passwordServiceMock = new();

        public AuthServiceTest()
        {
            _authService = new AuthService(_userRepoMock.Object, _tokenServiceMock.Object, _passwordServiceMock.Object);
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var userCredential = new UserCredential { Email = "john@example.com", Password = "admin@123" };
            var user = new User { Name = "Admin1", Email = "john@example.com", UserRole = UserRole.Admin };
            var token = "dummytoken";

            _userRepoMock.Setup(repo => repo.GetUserByEmailAsync(userCredential.Email))
                         .ReturnsAsync(user);

            _passwordServiceMock.Setup(service => service.VerifyPassword(userCredential.Password, user.Password, user.Salt))
               .Returns(true);

            _tokenServiceMock.Setup(service => service.GetToken(user))
                             .Returns(token);

            // Act
            var resultToken = await _authService.LoginAsync(userCredential);

            // Assert
            Assert.Equal(token, resultToken);
        }

        [Fact]
        public async Task LoginAsync_InvalidCredentials_ThrowsException()
        {
            // Arrange
            var userCredential = new UserCredential { Email = "john@example.com", Password = "admin@123" };

            var user = new User { Name = "Admin1", Email = "john@example.com", UserRole = UserRole.Admin };
            _userRepoMock.Setup(repo => repo.GetUserByEmailAsync(userCredential.Email))
                         .ThrowsAsync(new UnauthorizedAccessException("Invalid credentials"));

            _passwordServiceMock.Setup(service => service.VerifyPassword(userCredential.Password, user.Password, user.Salt))
                           .Returns(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(userCredential));
            Assert.Equal("Invalid credentials", exception.Message);
        }

    }
}
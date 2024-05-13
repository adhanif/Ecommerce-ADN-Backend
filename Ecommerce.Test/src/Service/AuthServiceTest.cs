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

        public AuthServiceTest()
        {
            _authService = new AuthService(_userRepoMock.Object, _tokenServiceMock.Object);
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var userCredential = new UserCredential { Email = "test@example.com", Password = "password" };
            var user = new User { Name = "Test User", Email = "test@example.com", UserRole = UserRole.Customer };
            var token = "dummytoken";

            _userRepoMock.Setup(repo => repo.GetUserByCredentialsAsync(userCredential))
                         .ReturnsAsync(user);

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
            var userCredential = new UserCredential { Email = "test@example.com", Password = "wrongpassword" };

            _userRepoMock.Setup(repo => repo.GetUserByCredentialsAsync(userCredential))
                         .ThrowsAsync(new UnauthorizedAccessException("Invalid credentials"));

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(userCredential));
        }

    }
}
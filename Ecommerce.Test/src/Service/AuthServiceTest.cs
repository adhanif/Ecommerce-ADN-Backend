using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.RepoAbstract;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Service.src.Service;
using Ecommerce.Service.src.ServiceAbstract;
using Moq;
using Xunit;
using System;

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
        public async Task LoginAsync_InvalidCredentials_WrongPassword_ThrowsException()
        {
            // Arrange
            var userCredential = new UserCredential { Email = "john@example.com", Password = "admin@123" };

            var user = new User { Name = "Admin1", Email = "john@example.com", UserRole = UserRole.Admin };
            _userRepoMock.Setup(repo => repo.GetUserByEmailAsync(userCredential.Email))
                         .ThrowsAsync(new UnauthorizedAccessException("Invalid password"));

            _passwordServiceMock.Setup(service => service.VerifyPassword(userCredential.Password, user.Password, user.Salt))
                           .Returns(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(userCredential));
            Assert.Equal("Invalid password", exception.Message);
        }

        [Fact]
        public async Task LoginAsync_InvalidCredentials_EmailNotFound_ThrowsException()
        {

            // Arrange
            var userCredential = new UserCredential { Email = "aohn@example.com", Password = "admin@123" };

            var user = new User { Name = "Admin1", Email = "john@example.com", UserRole = UserRole.Admin };

            _userRepoMock.Setup(repo => repo.GetUserByEmailAsync(userCredential.Email))
                         .ThrowsAsync(AppException.NotFound("Email is not registered"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<AppException>(() => _authService.LoginAsync(userCredential));
            Assert.Equal("Email is not registered", exception.Message);
        }



        [Fact]
        public async Task LogoutAsync_RevokesToken_ReturnsRevokedToken()
        {
            // Arrange
            var revokedToken = "revokedToken";

            _tokenServiceMock.Setup(service => service.InvalidateTokenAsync())
                              .ReturnsAsync(revokedToken);

            // Act
            var result = await _authService.LogoutAsync();
            // Assert
            Assert.Equal(revokedToken, result);
        }

        [Fact]
        public async Task GetCurrentProfileAsync_ValidCredentials_ReturnsProfile()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Name = "Admin1",
                Email = "john@example.com",
                UserRole = UserRole.Admin
            };
            var token = "dummytoken";

            _userRepoMock.Setup(repo => repo.GetUserByIdAsync(userId))
                     .ReturnsAsync(user);

            _tokenServiceMock.Setup(service => service.GetToken(user))
                         .Returns(token);

            // Act
            var result = await _authService.GetCurrentProfileAsync(userId);

            // Assert   
            Assert.NotNull(result); // Ensure result is not null
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.Name, result.UserName);
            Assert.Equal(user.Email, result.UserEmail);
            Assert.Equal(user.UserRole, result.UserRole);
        }

        [Fact]
        public async Task GetCurrentProfileAsync_UserNotFound_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _userRepoMock.Setup(repo => repo.GetUserByIdAsync(userId))
                         .ReturnsAsync((User)null);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<AppException>(() => _authService.GetCurrentProfileAsync(userId));

            // Assert
            Assert.Equal("Email is not registered", exception.Message);
        }

    }
}
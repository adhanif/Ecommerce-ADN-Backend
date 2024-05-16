
using Xunit;
using Moq;
using Ecommerce.Service.src.Service;
using Ecommerce.Core.src.RepoAbstract;
using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Core.src.Common;

// need to test:


namespace Ecommerce.Test.src.Service
{
    public class UserServiceTest
    {
        private readonly UserService _userService;
        private readonly Mock<IUserRepo> _userRepoMock = new();

        public UserServiceTest()
        {
            _userService = new UserService(_userRepoMock.Object);
            InitializeMockData();
        }

        private List<User> _users;
        private void InitializeMockData()
        {
            _users = [
            new User
        {
            Id = Guid.Parse("bbc0a48c-bade-7a31-9ded-914bcdb3cc6a"),
            Name = "Ade",
            Email = "akerwin0@ifeng.com",
            Password = "$2a$04$YQXmKAKPc.V7oTfEForoQeiru.d3kyysAz2dMtTfOYFN8/xyLtkcq",
            Avatar = "http://dummyimage.com/129x100.png/ff4444/ffffff",
            UserRole = UserRole.Customer,
            CreatedDate = new DateOnly(2023, 11, 25),
            UpdatedDate = new DateOnly(2023, 12, 5)
        },
        new User
        {
            Id = Guid.Parse("9e51cffe-7ddb-f351-aafd-beeb0a92df2d"),
            Name = "Sabra",
            Email = "swaything1@simplemachines.org",
            Password = "$2a$04$VyUJ7/w12oY6eJybzSosI.jeFSkWqQ9VZyhyIYThvl5bzNg/5ldnu",
            Avatar = "http://dummyimage.com/132x100.png/dddddd/000000",
            UserRole = UserRole.Customer,
            CreatedDate = new DateOnly(2023, 10, 22),
            UpdatedDate = new DateOnly(2023, 11, 1)
        },
        new User
        {
            Id = Guid.Parse("a64fcfda-cda2-75b6-a36e-ccbcc78c6fc7"),
            Name = "Virgie",
            Email = "vadamowitz2@telegraph.co.uk",
            Password = "$2a$04$WLSZ.S0c0ZwffMoUdjyf4OgwY/7ubkuKKbFoN9iwIuhODbIAaSEl6",
            Avatar = "http://dummyimage.com/235x100.png/ff4444/ffffff",
            UserRole = UserRole.Admin,
            CreatedDate = new DateOnly(2023, 12, 31),
            UpdatedDate = new DateOnly(2024, 1, 10)
        },
        new User
        {
            Id = Guid.Parse("ac1dbd1c-efed-8ecb-de0b-c0ee060ba5a1"),
            Name = "Juliane",
            Email = "jwildt3@census.gov",
            Password = "$2a$04$IyqNIbxpIS5xzcQdyQQC7uEKr9Z7DYx8YrKSYo8fb1eQl6DYO/9Am",
            Avatar = "http://dummyimage.com/230x100.png/ff4444/ffffff",
            UserRole = UserRole.Admin,
            CreatedDate = new DateOnly(2024, 1, 26),
            UpdatedDate = new DateOnly(2024, 2, 5)
        }];
        }


        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            // Arrange
            _userRepoMock.Setup(repo => repo.GetAllUsersAsync(It.IsAny<UserQueryOptions>()))
                         .ReturnsAsync(_users);

            // Act
            var result = await _userService.GetAllUsersAsync(new UserQueryOptions());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_users.Count, result.Count());
        }

        [Fact]
        public async Task GetAllUsersAsync_WithPagination_ShouldReturnCorrectSubset()
        {
            // Arrange
            var offset = 0;
            var limit = 3;
            var expectedSubset = _users.Skip(offset).Take(limit);

            _userRepoMock.Setup(repo => repo.GetAllUsersAsync(It.IsAny<UserQueryOptions>()))
                         .ReturnsAsync(expectedSubset);

            // Act
            var result = await _userService.GetAllUsersAsync(new UserQueryOptions { Offset = offset, Limit = limit });

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedSubset.Count(), result.Count());
        }


        [Fact]
        public async Task GetUserByIdAsync_ValidId_ShouldReturnUser()
        {
            // Arrange
            var userId = _users[0].Id;
            var expectedUser = _users[0];
            _userRepoMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.Name, result.UserName);
            Assert.Equal(expectedUser.Email, result.UserEmail);
        }

        [Fact]
        public async Task GetUserByIdAsync_InvalidId_ShouldReturnNull()
        {
            // Arrange
            var invalidUserId = Guid.NewGuid();
            _userRepoMock.Setup(repo => repo.GetUserByIdAsync(invalidUserId)).ThrowsAsync(new ArgumentException("User not found"));

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _userService.GetUserByIdAsync(invalidUserId));
        }

        [Fact]
        public async Task CreateUserAsync_ValidData_ShouldCreateUser()
        {
            // Arrange
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Name = "New User",
                Email = "newuser@example.com",
                Password = "password",
                Avatar = "http://example.com/avatar.jpg",
                UserRole = UserRole.Customer,
                CreatedDate = new DateOnly(2024, 4, 30),
                UpdatedDate = new DateOnly(2024, 4, 30)
            };
            var userCreateDto = new UserCreateDto
            {
                UserName = newUser.Name,
                UserEmail = newUser.Email,
                UserPassword = newUser.Password,
                UserAvatar = newUser.Avatar,
                UserRole = newUser.UserRole
            };
            var userReadDto = new UserReadDto();
            userReadDto.Transform(newUser);

            _userRepoMock.Setup(repo => repo.CreateUserAsync(newUser)).ReturnsAsync(newUser);

            // Act
            var result = await _userService.CreateUserAsync(userCreateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userReadDto, result);
        }

        [Fact]
        public async Task CreateUserAsync_InvalidData_ShouldThrowException()
        {
            // Arrange
            var invalidUserCreateDto = new UserCreateDto(); // Assuming required properties are not provided

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _userService.CreateUserAsync(invalidUserCreateDto));
        }

        [Fact]
        public async Task CreateUserAsync_ExistingEmail_ShouldThrowException()
        {
            // Arrange
            var existingEmail = _users[0].Email;
            var newUser = new UserCreateDto
            {
                UserName = "New User",
                UserEmail = existingEmail, // Using existing email
                UserPassword = "password",
                UserAvatar = "http://example.com/avatar.jpg",
                UserRole = UserRole.Customer
            };

            _userRepoMock.Setup(repo => repo.CreateUserAsync(It.IsAny<User>()))
                         .ThrowsAsync(new ArgumentException("Email already exists"));

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _userService.CreateUserAsync(newUser));
        }


        [Fact]
        public async Task UpdateUserByIdAsync_ValidIdAndData_ShouldUpdateUser()
        {
            // Arrange

            var userUpdateDto = new UserUpdateDto
            {
                UserName = "Updated User",

            };
            // we need to review the data shape again from DB to Repo to Service
            // because UpdatedDate should be generated and returned by DB
            var updatedUser = new User
            {
                Id = _users[0].Id,
                Name = userUpdateDto.UserName,
                Email = _users[0].Email,
                Password = _users[0].Password,
                Avatar = _users[0].Avatar,
                CreatedDate = _users[0].CreatedDate,
                UpdatedDate = _users[0].UpdatedDate,
            };

            var returnedUser = new User
            {
                Id = updatedUser.Id,
                Name = updatedUser.Name,
                Email = updatedUser.Email,
                Password = updatedUser.Password,
                Avatar = updatedUser.Avatar,
                UserRole = updatedUser.UserRole,
                CreatedDate = updatedUser.CreatedDate,
                UpdatedDate = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day)
            };


            var userReadDto = new UserReadDto();
            userReadDto.Transform(returnedUser);

            _userRepoMock.Setup(repo => repo.UpdateUserByIdAsync(updatedUser)).ReturnsAsync(returnedUser);

            // Act
            var result = await _userService.UpdateUserByIdAsync(_users[0].Id, userUpdateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userReadDto, result);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_ValidId_ShouldReturnTrue()
        {
            // Arrange
            var userId = _users[0].Id;
            _userRepoMock.Setup(repo => repo.DeleteUserByIdAsync(userId)).ReturnsAsync(true);

            // Act
            var result = await _userService.DeleteUserByIdAsync(userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_InValidId_ShouldReturnFalse()
        {
            // Arrange
            var invalidUserId = Guid.NewGuid();
            _userRepoMock.Setup(repo => repo.DeleteUserByIdAsync(invalidUserId)).ReturnsAsync(false);

            // Act
            var result = await _userService.DeleteUserByIdAsync(invalidUserId);

            // Assert
            Assert.False(result);
        }

    }
}
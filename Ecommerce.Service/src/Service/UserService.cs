using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.Common;
using Ecommerce.Service.src.ServiceAbstract;
using Ecommerce.Service.src.DTO;
using Ecommerce.Core.src.RepoAbstract;
using AutoMapper;
using System.Text.RegularExpressions;

namespace Ecommerce.Service.src.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;


        public UserService(IMapper mapper, IUserRepo userRepo, IPasswordService passwordService)
        {
            _mapper = mapper;
            _userRepo = userRepo;
            _passwordService = passwordService;
        }
        public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync(UserQueryOptions userQueryOptions)
        {
            try
            {
                var users = await _userRepo.GetAllUsersAsync(userQueryOptions);
                var UserReadDtos = users.Select(u => _mapper.Map<User, UserReadDto>(u));
                return UserReadDtos;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserReadDto> GetUserByIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new Exception("bad request");
            }
            try
            {
                var foundUser = await _userRepo.GetUserByIdAsync(userId);
                var foundUserDto = _mapper.Map<User, UserReadDto>(foundUser);
                return foundUserDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserReadDto> GetUserByEmailAsync(string email)
        {
            if (email == string.Empty)
            {
                throw AppException.BadRequest("Email is required");
            }
            try
            {
                var foundUser = await _userRepo.GetUserByEmailAsync(email);
                var foundUserDto = _mapper.Map<User, UserReadDto>(foundUser);
                return foundUserDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserReadDto> CreateUserAsync(UserCreateDto userCreateDto)
        {
            try
            {
                // validation
                if (string.IsNullOrEmpty(userCreateDto.UserName)) throw AppException.InvalidInputException("User name cannot be empty");
                if (userCreateDto.UserName.Length > 20) throw AppException.InvalidInputException("User name cannot be longer than 20 characters");

                string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                // Create Regex object
                Regex emailRegex = new(emailPattern);
                if (!emailRegex.IsMatch(userCreateDto.UserEmail)) throw AppException.InvalidInputException("Email is not valid");

                string imagePatten = @"^.*\.(jpg|jpeg|png|gif|bmp)$";
                Regex imageRegex = new(imagePatten);
                if (userCreateDto.UserAvatar is not null && !imageRegex.IsMatch(userCreateDto.UserAvatar)) throw AppException.InvalidInputException("Avatar can only be jpg|jpeg|png|gif|bmp");

                // Create a new User entity and populate its properties from the UserCreateDto

                var newUser = _mapper.Map<UserCreateDto, User>(userCreateDto);
                // Call the CreateUserAsync method of the repository to create the user

                // encrypt the password
                newUser.Password = _passwordService.HashPassword(newUser.Password, out byte[] salt);
                newUser.Salt = salt;

                var createdUser = await _userRepo.CreateUserAsync(newUser);

                var createdUserDto = _mapper.Map<User, UserReadDto>(createdUser);

                return createdUserDto;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<UserReadDto> UpdateUserByIdAsync(Guid userId, UserUpdateDto userUpdateDto)
        {
            try
            {
                var foundUser = await _userRepo.GetUserByIdAsync(userId);
                // validation
                if (userUpdateDto.UserName is not null && string.IsNullOrEmpty(userUpdateDto.UserName)) throw AppException.InvalidInputException("User name cannot be empty");
                if (userUpdateDto.UserName is not null && userUpdateDto.UserName.Length > 20) throw AppException.InvalidInputException("User name cannot be longer than 20 characters");

                string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                // Create Regex object
                Regex emailRegex = new(emailPattern);
                if (userUpdateDto.UserEmail is not null && !emailRegex.IsMatch(userUpdateDto.UserEmail)) throw AppException.InvalidInputException("Email is not valid");

                string imagePatten = @"^.*\.(jpg|jpeg|png|gif|bmp)$";
                Regex imageRegex = new(imagePatten);
                if (userUpdateDto.UserAvatar is not null && !imageRegex.IsMatch(userUpdateDto.UserAvatar)) throw AppException.InvalidInputException("Avatar can only be jpg|jpeg|png|gif|bmp");

                foundUser.Name = userUpdateDto.UserName ?? foundUser.Name;
                foundUser.Email = userUpdateDto.UserEmail ?? foundUser.Email;
                foundUser.Password = userUpdateDto.UserPassword ?? foundUser.Password;
                foundUser.Avatar = userUpdateDto.UserAvatar ?? foundUser.Avatar;
                foundUser.UserRole = userUpdateDto.UserRole ?? foundUser.UserRole;

                // Update the user entity with the new values
                var updateUser = await _userRepo.UpdateUserByIdAsync(foundUser);

                // Map the updated user entity back to a UserReadDto
                var updatedUserDto = _mapper.Map<User, UserReadDto>(updateUser);

                return updatedUserDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteUserByIdAsync(Guid userId)
        {
            try
            {
                // Delete the user entity from the repository
                var deleted = await _userRepo.DeleteUserByIdAsync(userId);
                if (!deleted)
                {
                    // If the user was not deleted for some reason, you can choose to throw an exception
                    throw new Exception("User deletion failed.");
                }
                // Return true to indicate successful deletion
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
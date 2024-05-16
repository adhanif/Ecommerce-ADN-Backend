
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.ValueObject;

namespace Ecommerce.Service.src.DTO
{

    public class UserReadDto : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Avatar { get; set; }
        public UserRole Role { get; set; }
        public IEnumerable<AddressReadDto>? Addresses { get; set; }

        public void Transform(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            Avatar = user.Avatar;
            Role = user.Role;
            CreatedDate = user.CreatedDate;
            UpdatedDate = user.UpdatedDate;
            // Transform Addresses
            Addresses = user.Addresses?.Select(a =>
            {
                var addressReadDto = new AddressReadDto();
                addressReadDto.Transform(a);
                return addressReadDto;
            }).ToList();
        }
    }

    public class UserCreateDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Avatar { get; set; }
        public UserRole Role { get; set; }
        public IEnumerable<AddressCreateDto>? Addresses { get; set; } = new List<AddressCreateDto>();


        public User CreateUser()
        {
            return new User
            {
                Name = Name,
                Email = Email,
                Password = Password,
                Avatar = Avatar,
                Role = Role,
                Addresses = Addresses?.Select(a => a.CreateAddress()).ToList(),
            };
        }
    }

    public class UserUpdateDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Avatar { get; set; }
        public UserRole? Role { get; set; }

        public User UpdateUser(User oldUser)
        {
            oldUser.Name = Name ?? oldUser.Name;
            oldUser.Email = Email ?? oldUser.Email;
            oldUser.Password = Password ?? oldUser.Password;
            oldUser.Avatar = Avatar ?? oldUser.Avatar;
            oldUser.Role = Role ?? oldUser.Role;
            return oldUser;
        }
    }
}
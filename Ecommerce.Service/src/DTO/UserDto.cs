
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.ValueObject;

namespace Ecommerce.Service.src.DTO
{

    public class UserReadDto : BaseEntity
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string? UserAvatar { get; set; }
        public UserRole UserRole { get; set; }
        public IEnumerable<AddressReadDto>? Addresses { get; set; }

        public void Transform(User user)
        {
            Id = user.Id;
            UserName = user.Name;
            UserEmail = user.Email;
            UserAvatar = user.Avatar;
            UserRole = user.UserRole;
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

    public class UserCreateDto : BaseEntity
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string? UserAvatar { get; set; }
        public UserRole UserRole { get; set; }
        public IEnumerable<AddressCreateDto>? Addresses { get; set; } = new List<AddressCreateDto>();


        public User CreateUser()
        {
            return new User
            {
                Name = UserName,
                Email = UserEmail,
                Password = UserPassword,
                Avatar = UserAvatar,
                UserRole = UserRole,
                Addresses = Addresses?.Select(a => a.CreateAddress()).ToList(),
                CreatedDate = CreatedDate,
                UpdatedDate = UpdatedDate
            };
        }
    }

    public class UserUpdateDto : BaseEntity
    {
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPassword { get; set; }
        public string? UserAvatar { get; set; }
        public UserRole? UserRole { get; set; }
        public User UpdateUser(User oldUser)
        {
            oldUser.Name = UserName ?? oldUser.Name;
            oldUser.Email = UserEmail ?? oldUser.Email;
            oldUser.Password = UserPassword ?? oldUser.Password;
            oldUser.Avatar = UserAvatar ?? oldUser.Avatar;
            oldUser.UserRole = UserRole ?? oldUser.UserRole;
            return oldUser;
        }
    }
}
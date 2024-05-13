using Ecommerce.Core.src.ValueObject;

namespace Ecommerce.Core.src.Common
{
    public class UserQueryOptions : BaseQueryOptions
    {
        public UserRole? SearchRole { get; set; } = null;
        public string SearchName { get; set; } = string.Empty;
    }
}
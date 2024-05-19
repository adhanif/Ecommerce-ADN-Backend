using System.Text.Json.Serialization;

namespace Ecommerce.Core.src.ValueObject
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserRole
    {
        Customer, Admin
    }
}
using System.Text.Json.Serialization;

namespace Ecommerce.Core.src.ValueObject
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatus
    {
        Shipped,
        Pending,
        AwaitingPayment,
        Processing,
        Shipping,
        Completed,
        Refunded,
        Cancelled
    }
}
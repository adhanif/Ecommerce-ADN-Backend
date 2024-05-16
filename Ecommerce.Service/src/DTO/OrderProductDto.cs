namespace Ecommerce.Service.src.DTO
{
    public class OrderProductReadDto
    {
        public Guid ProductId { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderProductCreateDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderProductUpdateDto
    {
        public Guid ProductId { get; set; }
        public string? Title { get; set; }
        public int? Price { get; set; }
        public int? Quantity { get; set; }
    }
}
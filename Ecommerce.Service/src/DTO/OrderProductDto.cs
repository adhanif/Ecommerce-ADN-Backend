namespace Ecommerce.Service.src.DTO
{
    public class OrderProductReadDto
    {
        public Guid ProductId { get; set; }
        public string ProductTitle { get; set; }
        public double ProductPrice { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderProductCreateDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderProductUpdateDto
    {
        public int Quantity { get; set; }
    }
}
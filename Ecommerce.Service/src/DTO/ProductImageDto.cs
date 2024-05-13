namespace Ecommerce.Service.src.DTO
{
    public class ProductImageCreateDto
    {
        public string Url { get; set; }
    }

    public class ProductImageReadDto
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
    }

    public class ProductImageUpdateDto
    {
        public Guid Id { get; set; } // The ID of the image to update
        public string Url { get; set; } 
       
    }
}
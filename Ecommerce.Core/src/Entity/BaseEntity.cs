namespace Ecommerce.Core.src.Entity
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateOnly? CreatedDate { get; set; }
        public DateOnly? UpdatedDate { get; set; }
    }
}
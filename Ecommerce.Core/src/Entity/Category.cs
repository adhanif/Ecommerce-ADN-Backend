namespace Ecommerce.Core.src.Entity
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }

        override public string ToString()
        {
            return $"Category Name: {Name}, Category Image: {Image}";
        }
    }
}
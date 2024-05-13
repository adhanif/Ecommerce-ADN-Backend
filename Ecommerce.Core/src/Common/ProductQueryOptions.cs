namespace Ecommerce.Core.src.Common
{
    public class ProductQueryOptions : BaseQueryOptions
    {
        public string Title { get; set; } = string.Empty;
        public decimal? Min_Price { get; set; }
        public decimal? Max_Price { get; set; }
        public Guid? Category_Id { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
    }
}
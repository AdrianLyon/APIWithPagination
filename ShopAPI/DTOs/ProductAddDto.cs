namespace ShopAPI.DTOs
{
    public class ProductAddDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public int CategoryId { get; set; }
    }
}
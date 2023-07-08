namespace ShopAPI.DTOs
{
    public class ProductUpdateDto{
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public int CategoryId { get; set; }
    }
}
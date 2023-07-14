using ShopAPI.Models;
using ShopAPI.Models.Common;

namespace ShopAPI.Models
{
    public class Product : BaseEnity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
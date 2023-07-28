namespace ShopAPI.Models.Common
{
    public class BaseEnity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Deleted { get; set; }
        public bool isDeleted { get; set; }
    }
}
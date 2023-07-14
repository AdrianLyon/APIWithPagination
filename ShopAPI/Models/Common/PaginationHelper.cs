namespace ShopAPI.Models.Common
{
    public static class PaginationHelper
    {
        public static PaginatedResponse<T> PaginateData<T>(IEnumerable<T> items, int page, int pageSize)
        {
            var totalItems = items.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var paginatedItems = items.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = new PaginatedResponse<T>
            {
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize,
                Items = paginatedItems
            };

            return response;
        }
    }
}
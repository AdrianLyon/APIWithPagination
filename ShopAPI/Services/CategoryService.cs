using Microsoft.EntityFrameworkCore;
using ShopAPI.Data;
using ShopAPI.DTOs;
using ShopAPI.Models;
using ShopAPI.Models.Common;

namespace ShopAPI.Services
{
    public interface ICategoryService
    {
        Task<PaginatedResponse<CategoryDto>> GetAllAsync(int page = 1, int pageSize = 10, string searchQuery = "");
        Task<CategoryDto> GetByIdAsync(int id);
        Task<Category> PostAsync(CategoryCreateDto category);
        Task<Category> UpdateAsync(int id, CategoryDto category);
        Task<bool> DeleteAsync(int id);
    }

    public class CategoryService : ICategoryService
    {
        private readonly ApplicationdbContext _db;
        public CategoryService(ApplicationdbContext db)
        {
            _db = db;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var recordToDelete = await _db.Categories.FindAsync(id);

            if (recordToDelete == null) throw new Exception("Category not found");

            recordToDelete.Deleted = DateTime.Now;
            recordToDelete.isDeleted = true;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<PaginatedResponse<CategoryDto>> GetAllAsync(int page = 1, int pageSize = 10, string searchQuery = "")
        {
            var entities = await _db.Categories
                                    .AsNoTracking()
                                    .Where(x => !x.isDeleted)
                                    .Select(x => new CategoryDto
                                    {
                                        Id = x.Id,
                                        Description = x.Description
                                    }).ToListAsync();

            var filteredProducts = string.IsNullOrEmpty(searchQuery)
        ? entities
        : entities.Where(p => p.Description.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));

            var totalItems = filteredProducts.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var paginatedProducts = filteredProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var paginationResult = new PaginatedResponse<CategoryDto>
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                Page = page,
                PageSize = pageSize,
                Items = paginatedProducts
            };

            return paginationResult;
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var entity = await _db.Categories
                                  .AsNoTracking()
                                  .Where(x => x.Id == id && x.isDeleted == false)
                                  .Select(x => new CategoryDto
                                  {
                                      Id = x.Id,
                                      Description = x.Description
                                  })
                                  .FirstOrDefaultAsync();
            if (entity == null) throw new Exception("Category not found");

            return entity;
        }

        public async Task<Category> PostAsync(CategoryCreateDto category)
        {
            var newEntity = new Category();
            if (newEntity.Description == category.Description) throw new Exception("Category already exists");
            newEntity.Description = category.Description;
            newEntity.Created = DateTime.Now;
            newEntity.isDeleted = false;
            _db.Categories.Add(newEntity);
            await _db.SaveChangesAsync();
            return newEntity;
        }

        public async Task<Category> UpdateAsync(int id, CategoryDto category)
        {
            var entity = await _db.Categories.FindAsync(id);
            if (entity == null) throw new Exception("Category not found");

            //if (entity.Id != category.Id) throw new Exception("Category id mismatch");
            entity.Description = category.Description;
            entity.Modified = DateTime.Now;
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
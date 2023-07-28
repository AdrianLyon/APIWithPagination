using Microsoft.EntityFrameworkCore;
using ShopAPI.Data;
using ShopAPI.DTOs;
using ShopAPI.Models;

namespace ShopAPI.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto> GetByIdAsync(int id);
        Task<Product> PostAsync(ProductAddDto product);
        Task<Product> UpdateAsync(int id, ProductUpdateDto product);
        Task<bool> DeleteAsync(int id);
    }
    public class ProductService : IProductService
    {
        private readonly ApplicationdbContext _db;
        public ProductService(ApplicationdbContext db)
        {
            _db = db;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var entityForDelete = await _db.Products.FindAsync(id);
            if (entityForDelete == null) throw new Exception("Product not found");
            entityForDelete.isDeleted = true;
            entityForDelete.Deleted = DateTime.Now;
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var entities = await _db.Products
                                    .AsNoTracking()
                                    .Include(x => x.Category)
                                    .Where(x => !x.isDeleted)
                                    .Select(x => new ProductDto
                                    {
                                        Id = x.Id,
                                        Name = x.Name,
                                        Price = x.Price,
                                        Quantity = x.Quantity,
                                        PricePerUnit = x.PricePerUnit,
                                        CategoryId = x.CategoryId,
                                        Category = new CategoryDto
                                        {
                                            Id = x.Category.Id,
                                            Description = x.Category.Description
                                        }
                                    }).ToListAsync();
            return entities;
        }
        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var entity = await _db.Products
                                  .Include(x => x.Category)
                                  .AsNoTracking()
                                  .Where(x => !x.isDeleted)
                                  .Select(x => new ProductDto
                                  {
                                      Id = x.Id,
                                      Name = x.Name,
                                      Quantity = x.Quantity,
                                      Price = x.Price,
                                      PricePerUnit = x.PricePerUnit,
                                      CategoryId = x.CategoryId,
                                      Category = new CategoryDto
                                      {
                                          Id = x.Category.Id,
                                          Description = x.Category.Description
                                      }
                                  }).FirstOrDefaultAsync();
            if (entity == null) throw new Exception("Product not found");
            return entity;
        }
        public async Task<Product> PostAsync(ProductAddDto product)
        {
            var newProduct = new Product();
            if (product == null) throw new Exception("Product not send");
            newProduct.Name = product.Name;
            newProduct.Quantity = product.Quantity;
            newProduct.Price = product.Price;
            newProduct.PricePerUnit = product.Price * product.Quantity;
            newProduct.CategoryId = product.CategoryId;
            _db.Products.Add(newProduct);
            await _db.SaveChangesAsync();
            return newProduct;
        }
        public async Task<Product> UpdateAsync(int id, ProductUpdateDto product)
        {
            var entityUpdate = await _db.Products.FindAsync(id);
            if (entityUpdate == null) throw new Exception("Product not found");

            entityUpdate.Name = product.Name;
            entityUpdate.Quantity = product.Quantity;
            entityUpdate.Price = product.Price;
            entityUpdate.PricePerUnit = product.Price * product.Quantity;
            entityUpdate.CategoryId = product.CategoryId;
            await _db.SaveChangesAsync();
            return entityUpdate;
        }
    }
}
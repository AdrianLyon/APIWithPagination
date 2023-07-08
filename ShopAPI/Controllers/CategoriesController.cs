using Microsoft.AspNetCore.Mvc;
using ShopAPI.DTOs;
using ShopAPI.Services;

namespace ShopAPI.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var model = await _categoryService.GetAllAsync();
            if (model.Any()) return NotFound();
            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var model = await _categoryService.GetByIdAsync(id);
            if (model == null) return NotFound();
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CategoryCreateDto category)
        {
            var model = await _categoryService.PostAsync(category);
            if (model == null) return NotFound();
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, CategoryDto category)
        {
            var model = await _categoryService.UpdateAsync(id, category);
            if (model == null) return NotFound();
            return Ok(model);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _categoryService.DeleteAsync(id);
            if (model == null) return NotFound();

            if (!ModelState.IsValid) return BadRequest();
            return NoContent();
        }
    }
}
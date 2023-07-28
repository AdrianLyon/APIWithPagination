using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.DTOs;
using ShopAPI.Models.Common;
using ShopAPI.Services;

namespace ShopAPI.Controllers
{
    [ApiController]
    [EnableCors]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
        public async Task<IActionResult> Get(int page = 1, int pageSize = 10)
        {
            var response = await _productService.GetAllAsync();
            if (response.Any()) NotFound();
            var paginations = PaginationHelper.PaginateData(response, page, pageSize);
            return Ok(paginations);
        }

        [HttpGet("{id}")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _productService.GetByIdAsync(id);
            if (response == null) NotFound();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProductAddDto product)
        {
            var request = await _productService.PostAsync(product);
            if (request == null) NotFound();
            return Ok(request);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ProductUpdateDto product)
        {
            var request = await _productService.UpdateAsync(id, product);
            if (request == null) NotFound();
            return Ok(request);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _productService.DeleteAsync(id);
            if (!ModelState.IsValid) NotFound();
            return Ok(response);
        }
    }
}
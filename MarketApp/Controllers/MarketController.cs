using MarketApp.DataTransferObjects;
using MarketApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarketApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketController : ControllerBase
    {
        private readonly IMarketService _marketService;

        public MarketController(IMarketService marketService)
        {
            _marketService = marketService;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = _marketService.GetAllProducts();
            return Ok(products);    
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct(ProductDto request)
        {
            await _marketService.AddProduct(request);

            return Ok("new product added");
        }

        [HttpDelete, Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _marketService.DeleteProduct(id);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPut, Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutProduct(int id, ProductDto request)
        {
            try
            {
                await _marketService.UpdateProduct(id, request);    
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok("Product updated");
        }
    }
}

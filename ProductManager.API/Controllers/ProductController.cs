using Microsoft.AspNetCore.Mvc;
using ProductManager.Business.DTOs;
using ProductManager.Business.Services.Abstractions;

namespace ProductManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult FindAll()
        {
            _logger.LogInformation("Fetching all products.");
            return Ok(_productService.FindAll());
        }

        [HttpGet("{id}")]
        public IActionResult FindById(Guid id)
        {
            _logger.LogInformation("Fetching product with ID {id}", id);
            var product = _productService.FindById(id);
            if (product == null)
            {
                _logger.LogWarning("Product with ID {id} not found", id);
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public IActionResult Create([FromBody] ProductDTO product)
        {
            _logger.LogInformation("Create new product: {Name}", product.Name);
            var createdProduct = _productService.Create(product);
            if (createdProduct == null)
            {
                _logger.LogError("Error creating product.");
                return NotFound();
            }
            return Ok(createdProduct);
        }

        [HttpPut]
        public IActionResult Update([FromBody] ProductDTO product)
        {
            _logger.LogInformation("Updating product with ID {id}", product.Id);
            var updatedProduct = _productService.Update(product);
            if (updatedProduct == null)
            {
                _logger.LogError("Failed to update product with ID {id}", product.Id);
                return NotFound();
            }

            _logger.LogDebug("Product update successfully: {Name}", product.Name);
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _logger.LogInformation("Fetching product with ID {id}", id);
            _productService.Delete(id);
            _logger.LogDebug("Product with ID {id} deleted successfully.", id);
            return NoContent();
        }
    }
}

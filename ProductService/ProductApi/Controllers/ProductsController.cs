using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(ILogger<ProductsController> logger) : ControllerBase
{
    private readonly List<Product> _products = [
        new Product( Id: 1,Name: "Samsung S23", Price: 39980.00, Stock: 5),
        new Product( Id: 2,Name: "Samsung Tab S9 FE Plus", Price: 29980.00, Stock: 8),
        new Product( Id: 3,Name: "Samsung Fold Z5", Price: 109980.00, Stock: 2),
        new Product( Id: 4,Name: "Apple 16 Pro", Price: 139980.00, Stock: 10),
        new Product( Id: 5,Name: "IPad Pro M2", Price: 99980.00, Stock: 12)
    ];

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            logger.LogInformation("Start Fetching Products.....");
            await Task.Delay(500);

            logger.LogInformation("Products Fetched Successfully");
            return Ok(_products);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            logger.LogInformation($"Start Fetching Product..... ID: {id}");

            var product = _products.FirstOrDefault(x => x.Id == id);

            if (product is null)
            {
                logger.LogWarning($"Product Not Found. ID: {id}");
                return NotFound();
            }

            logger.LogInformation($"Product Fetched successfully. ID: {product.Id}, Name: {product.Name}");
            return Ok(product);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return StatusCode(500, "Internal server error.");
        }
    }
}

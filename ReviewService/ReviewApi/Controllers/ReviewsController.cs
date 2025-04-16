using Microsoft.AspNetCore.Mvc;
using ReviewApi.Models;

namespace ReviewApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController(
    ILogger<ReviewsController> logger,
    IHttpClientFactory clientFactory) : ControllerBase
{
    private readonly List<Review> _reviews = [
        new Review(
            Id: 1,
            ProductId: 1,
            Rating: 5,
            Comment: "Samsung S23 is the overall very good phone in this budget. Camera quality is really very good with Good UI"),
        new Review(
            Id: 2,
            ProductId: 1,
            Rating: 4,
            Comment: "Camera quality is Superb. UI is clean and fluent. But battery quality is average"),
        new Review(
            Id: 3,
            ProductId: 2,
            Rating: 4,
            Comment: "Best android tablet in this price range. Very good UI optimization. Great for note taking"),
        new Review(
            Id: 4,
            ProductId: 4,
            Rating: 5,
            Comment: "Camera Video quality is Superb. Battery quality is great"),
    ];

    [HttpGet]
    [Route("{productId:int}")]
    public async Task<IActionResult> Get(int productId)
    {
        try
        {
            logger.LogInformation("Fetching Product Reviews ....Product Id: {id}", productId);
            await Task.Delay(500);

            var reviews = _reviews.Where(x => x.ProductId == productId).ToList();
            logger.LogInformation("Product Reviews Found. Reviews Count: {count}", reviews.Count);
            return Ok(reviews);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet]
    [Route("{productId:int}/details")]
    public async Task<IActionResult> GetDetails(int productId)
    {
        try
        {
            logger.LogInformation("Fetching Product details ....Product Id: {id}", productId);

            var httpClient = clientFactory.CreateClient("HttpClient");
            var response = await httpClient.GetAsync($"/gateway/products/{productId}");

            if (response.IsSuccessStatusCode == false)
            {
                logger.LogError("Product fetched failed. Status Code: {code}", response.StatusCode);
                return StatusCode((int)response.StatusCode);
            }

            var product = await response.Content.ReadFromJsonAsync<Product>();

            if (product == null)
            {
                logger.LogWarning("Product not found. Product Id: {id}", productId);
                return NotFound("Product with given id not found");
            }
            logger.LogInformation("Product fetched successfully. ID: {id}, Name: {name}", product.Id, product.Name);
            logger.LogInformation("Fetching Product Reviews ....Product Id: {id}", productId);

            await Task.Delay(250);
            var reviews = _reviews.Where(x => x.ProductId == productId).ToList();

            logger.LogInformation("Product Reviews Found. Reviews Count: {count}", reviews.Count);
            return Ok(new
            {
                product,
                reviews
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return StatusCode(500, "Internal server error.");
        }
    }
}

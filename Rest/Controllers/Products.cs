using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text.Json;
using WebshopLib.Model;
using WebshopLib.Services.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Rest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Products : ControllerBase
    {
        private IProductRepository _repo;

        public Products(IProductRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            var result = _repo.GetAll();
            return result.Any() ? Ok(result) : NoContent();
        }

        [Authorize]
        [HttpGet("GetSessionBasket")]
        public ActionResult<IEnumerable<Product>> GetBasket()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            var sessionBasket = HttpContext.Session.GetString("basket");
            if (string.IsNullOrEmpty(sessionBasket))
            {
                return NoContent();
            }

            try
            {
                // Deserialize session data into a List<Product>
                var basket = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(sessionBasket);
                return Ok(basket); // Return strongly typed data
            }
            catch (Exception ex)
            {
                // Handle potential deserialization errors
                return BadRequest($"Error deserializing basket: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("ReserveProduct{productId}")]
        public ActionResult<IEnumerable<Product>> ReserveProduct(string productId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            try
            {
                // Reserve the product
                var result = _repo.ReserveProduct(productId);
                if (!result)
                {
                    return NotFound("Product reservation failed.");
                }

                // Retrieve or create a session basket
                var item = _repo.GetById(productId);
                if (item == null)
                {
                    return NotFound("Product not found.");
                }

                var basketJson = HttpContext.Session.GetString("basket");
                var basket = string.IsNullOrEmpty(basketJson)
                    ? new List<Product>() // Create a new basket if none exists
                    : JsonSerializer.Deserialize<List<Product>>(basketJson);

                // Add the item to the basket
                basket.Add(item);

                // Save the updated basket back to the session
                HttpContext.Session.SetString("basket", JsonSerializer.Serialize(basket));

                // Return the updated basket
                return Ok(basket);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, "Database error: " + ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred: " + ex.Message);
            }
        }

        [Authorize]
        [HttpGet("RemoveReservedProduct{productId}")]
        public ActionResult<IEnumerable<Product>> RemoveReservedProduct(string productId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            try
            {
                // Reserve the product
                var result = _repo.RemoveReservedProduct(productId);
                if (!result)
                {
                    return NotFound("Product reservation failed.");
                }

                var basketJson = HttpContext.Session.GetString("basket");
                var basket = string.IsNullOrEmpty(basketJson)
                    ? null
                    : JsonSerializer.Deserialize<List<Product>>(basketJson);

                // Retrieve or create a session basket
                Product item = basket.FirstOrDefault(id => id.Varenummer == Guid.Parse(productId));
                if (item == null)
                {
                    return NotFound("Product not found.");
                }
                var done = basket.Remove(item);

                // Save the updated basket back to the session
                HttpContext.Session.SetString("basket", JsonSerializer.Serialize(basket));

                // Return the updated basket
                return Ok(basket);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, "Database error: " + ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred: " + ex.Message);
            }
        }

        [HttpGet("filterByName")]
        public ActionResult<IEnumerable<Product>> GetFilter([FromQuery] string filter)
        {
            var result = _repo.GetFiltered(filter);
            return result.Any() ? Ok(result) : NoContent();
        }

        //// GET api/<Products>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<Products>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<Products>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<Products>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}

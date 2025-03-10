using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
            ////Will be used to validate the session
            //var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //var sessionValid = HttpContext.Request.Cookies[userId];
            //if (sessionValid != null && HttpContext.Session.Id == sessionValid)
            //{
            //var result = _repo.GetAll();
            //return result.Any() ? Ok(result) : NoContent();
            //}
            //return NoContent();

            var result = _repo.GetAll();
            return result.Any() ? Ok(result) : NoContent();
        }

        [HttpGet("filterByName{filter}")]
        public ActionResult<IEnumerable<Product>> GetFilter(string filter)
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

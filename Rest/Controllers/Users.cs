using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using WebshopLib.Model;
using WebshopLib.Model.DTOs;
using WebshopLib.Services.Interfaces;
using WebshopLib.Services.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Rest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Users : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        private AuthManagerRepository _authRepo;
        private IUserRepository _userRepo;

        public Users(UserManager<IdentityUser> userManager, AuthManagerRepository authRepo, IUserRepository userRepo )
        {
            _userManager = userManager;
            _authRepo = authRepo;
            _userRepo = userRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentityUser>>> GetUsers()
        {
            var users = _userManager.Users;
            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UsersRequestWithEmail Dto)
        {
            try
            {
            await _authRepo.AddRoleToUser(Dto.Email);
            return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("GetById")]
        public async Task<ActionResult<Person>> getUser()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var response = _userRepo.GetById(userId);
                if (response == null)
                {
                    return NotFound();
                }
                return (await _authRepo.UserExists(response.Email)) == true ? Ok(response) : NotFound();


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("CreateUser")]
        public ActionResult<Person> addUser([FromBody] UsersRequestCreateUserWithAttributes Dto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var result = _userRepo.Add(new Person(0, Dto.FirstName, Dto.LastName, Dto.Email, Dto.PhoneNumber, new Address(0, Dto.AddressObj.Street, Dto.AddressObj.HouseNumber, new City(0, Dto.AddressObj.CityObj.Name))), userId);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebshopLib.Model.DTOs;
using WebshopLib.Services.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Rest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Users : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        private AuthManagerRepository _userRepository;

        public Users(UserManager<IdentityUser> userManager, AuthManagerRepository userrepo )
        {
            _userManager = userManager;
            _userRepository = userrepo;
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
            await _userRepository.AddRoleToUser(Dto.Email);
            return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
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
        private SignInManager<IdentityUser> _signInManager;

        private AuthManagerRepository _authRepo;
        private IUserRepository _userRepo;

        public Users( UserManager<IdentityUser> userManager, AuthManagerRepository authRepo, IUserRepository userRepo, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _authRepo = authRepo;
            _userRepo = userRepo;
            _signInManager = signInManager;
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
        [HttpGet("GetSession")]
        public async Task<ActionResult<Person>> getSession()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = _userRepo.GetById(userId);
                if (user == null)
                {
                    return NotFound();
                }
                return (await _authRepo.UserExists(user.Email)) == true ? Ok(HttpContext.Session.Get(userId)) : NotFound();


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("ValidateLoginAndSession")]
        public async Task<ActionResult<bool>> ValidateLoginAndSession()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = _userRepo.GetById(userId);
                if (user == null)
                {
                    return NotFound();
                }
                if (!(await _authRepo.UserExists(user.Email)))
                {
                    return NotFound();
                }
                var session = HttpContext.Session.Get(userId);
                if (session == null)
                {
                    return NotFound();
                }
                return true;

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] RequestLogin login, [FromQuery] bool? useCookies, [FromQuery] bool? useSessionCookies)
        {
            var useCookieScheme = (useCookies == true) || (useSessionCookies == true);
            var isPersistent = (useCookies == true) && (useSessionCookies != true);
            _signInManager.AuthenticationScheme = useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent, lockoutOnFailure: true);

            if (result.RequiresTwoFactor)
            {
                if (!string.IsNullOrEmpty(login.TwoFactorCode))
                {
                    result = await _signInManager.TwoFactorAuthenticatorSignInAsync(login.TwoFactorCode, isPersistent, rememberClient: isPersistent);
                }
                else if (!string.IsNullOrEmpty(login.TwoFactorRecoveryCode))
                {
                    result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(login.TwoFactorRecoveryCode);
                }
            }

            if (!result.Succeeded)
            {
                return Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized);
            }
            else
            {
                var user = await _authRepo.FindUser(login.Email);

                // Store the session ID in the session using the user's ID as the key
                HttpContext.Session.SetString(user.Id, HttpContext.Session.Id);

            }

            return Ok();
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            // Clear session and identity cookies
            Response.Cookies.Delete(".AspNetCore.Identity.Application");
            Response.Cookies.Delete(".AspNetCore.Session");

            // Optionally, clear other session data or tokens
            return Ok(new { message = "Logged out successfully" });
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

﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using WebshopLib.Model;
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

        private AuthManagerRepository _authRepo;
        private UserRepository _userRepo;

        public Users(UserManager<IdentityUser> userManager, AuthManagerRepository userrepo )
        {
            _userManager = userManager;
            _authRepo = userrepo;
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

        [HttpGet("GetByEmail")]
        public async Task<ActionResult<Person>> getUser([FromBody] UsersRequestWithEmail Dto)
        {
            try
            {
                return (await _authRepo.UserExists(Dto.Email)) == true ? Ok(_userRepo.GetByEmail(Dto.Email)) : NotFound();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost("CreateUser")]
        //public ActionResult<Person> addUser()
        //{
            
        //}
    }
}

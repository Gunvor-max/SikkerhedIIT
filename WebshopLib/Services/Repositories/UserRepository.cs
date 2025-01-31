using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopLib.Services.Repositories
{
    public class UserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserRepository(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task AddRoleToUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, "User");
                if (!result.Succeeded)
                {
                    // Handle errors here
                    throw new Exception("Failed to add user to role");
                }
            }
            else
            {
                throw new Exception("User not found");
            }
        }
    }
}

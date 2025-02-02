using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopLib.Services.Repositories
{
    public class AuthManagerRepository
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public AuthManagerRepository(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<bool> UserExists(string email)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var user = await userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    return true;
                }
                return false;
            }
        }

        public async Task AddRoleToUser(string email)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var user = await userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var result = await userManager.AddToRoleAsync(user, "User");
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
}

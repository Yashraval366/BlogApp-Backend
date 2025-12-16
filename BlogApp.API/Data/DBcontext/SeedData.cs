using BlogApp.API.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.API.Data.DBcontext
{
    public class SeedData
    {
        public static async Task AddRolesAndAdmin(IServiceProvider service)
        {
            var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = service.GetRequiredService<RoleManager<ApplicationRole>>();


            var roles = new List<string>()
            {
                "Admin",
                "User"
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole(role));
                }
            }

            var adminUser = new ApplicationUser
            {
                Email = "admin@site.com",
                UserName = "admin@site.com",
                FullName = "System Admin",
                EmailConfirmed = true,
            };

            var adminExist = await userManager.FindByEmailAsync(adminUser.Email);

            if (adminExist == null)
            {
                var result = await userManager.CreateAsync(adminUser, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}

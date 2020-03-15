using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var defaultUser = new ApplicationUser { UserName = "demouser@microsoft.com", Email = "demouser@microsoft.com", FirstName = "Александр", SecondName = "Иванов", 
                Country = "Республика Беларусь", City = "Минск", Street = "пер. Айвазовского", HomeNumber = 12, Flat = 5};
            await userManager.CreateAsync(defaultUser, "Pass@word1");
            string adminLogin = "admin";
            string adminEmail = "admin@gmail.com";
            string password = "_Aa123456";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                ApplicationUser admin = new ApplicationUser
                {
                    Email = adminEmail, UserName = adminLogin, FirstName = "Михаил", SecondName = "Петров",
                    Country = "Республика Беларусь",
                    City = "Минск",
                    Street = "ул. Карла Маркса",
                    HomeNumber = 23,
                    Flat = 12
                };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}

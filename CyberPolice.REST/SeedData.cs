using CyberPolice.Infrastructure;
using CyberPolice.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CyberPolice.REST
{
    public static class SeedData
    {
        public static readonly string[] Roles = { "Admin", "Investigator", "Analyst" };

        public static async Task InitializeAsync(IServiceProvider services)
        {
            var context = services.GetRequiredService<CyberPoliceContext>();
            await context.Database.MigrateAsync();

            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            foreach (var role in Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            const string adminEmail = "admin@cyberpolice.local";
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FullName = "Адміністратор"
                };
                var result = await userManager.CreateAsync(admin, "Admin123!");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
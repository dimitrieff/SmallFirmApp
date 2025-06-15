using Microsoft.AspNetCore.Identity;
using SmallFirmApp.Constants;

namespace SmallFirmApp.Data
{
    public class RoleSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(Roles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            }

            if (!await roleManager.RoleExistsAsync(Roles.StaffOperator))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.StaffOperator));
            }

            if (!await roleManager.RoleExistsAsync(Roles.StaffUser))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.StaffUser));
            }

        }
    }
}

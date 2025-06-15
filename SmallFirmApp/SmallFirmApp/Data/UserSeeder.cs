using Microsoft.AspNetCore.Identity;
using SmallFirmApp.Constants;

namespace SmallFirmApp.Data
{
    public class UserSeeder
    {
        public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            await CreateUserWithRole(userManager, "admin@admin", "Admin1!", Roles.Admin);

            await CreateUserWithRole(userManager, "op@op", "Admin1!", Roles.StaffOperator);

            await CreateUserWithRole(userManager, "staff@staff", "Admin1!", Roles.StaffUser);
        }

        private static async Task CreateUserWithRole(UserManager<IdentityUser> userManager, string email, string password, string role)
        {
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser
                {
                    Email = email,
                    EmailConfirmed = true,
                    UserName = email,
                };

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
                else
                {
                    throw new Exception($"Failed creating user {user.Email}. Errors: {string.Join(", ", result.Errors)}");
                }
            }
        }
    }
}

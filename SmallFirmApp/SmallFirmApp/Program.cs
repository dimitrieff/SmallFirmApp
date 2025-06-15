using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmallFirmApp.Data;
using SmallFirmApp.Repositories;

namespace SmallFirmApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Adding database
            builder.Services.AddDbContext<ApplicationDbContext>(options => {

                options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));


            });

            // Adding users and roles
            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
               .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;
            });

            builder.Services.AddScoped<IRepository<ApplicationDbContext>, SmallFirmAppRepository<ApplicationDbContext>>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //adding roles and users
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                RoleSeeder.SeedRolesAsync(services).Wait();
                UserSeeder.SeedUsersAsync(services).Wait();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //add razor pages for using account pages (login, register...)
            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=SmallFirmApp}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

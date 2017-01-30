using aspCart.Infrastructure;
using aspCart.Infrastructure.EFModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Web.Helpers
{
    public class DefaultDataProvider
    {
        public static void ApplyMigration(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();

            // apply migration
            context.Database.Migrate();
        }

        public static void Seed(IServiceProvider serviceProvider, IConfigurationRoot configuration)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();

            SeedAdminAccount(context, configuration);
        }

        private static async void SeedAdminAccount(ApplicationDbContext context, IConfigurationRoot configuration)
        {
            var user = new ApplicationUser()
            {
                UserName = configuration.GetValue<string>("AdminAccount:Email"),
                NormalizedUserName = configuration.GetValue<string>("AdminAccount:Email").ToUpper(),
                Email = configuration.GetValue<string>("AdminAccount:Email"),
                NormalizedEmail = configuration.GetValue<string>("AdminAccount:Email").ToUpper(),
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var roleStore = new RoleStore<IdentityRole>(context);

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
            }

            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var passwordHasher = new PasswordHasher<ApplicationUser>();
                var hashed = passwordHasher.HashPassword(user, configuration.GetValue<string>("AdminAccount:Password"));
                user.PasswordHash = hashed;
                var userStore = new UserStore<ApplicationUser>(context);
                await userStore.CreateAsync(user);
                await userStore.AddToRoleAsync(user, "Admin");
            }

            await context.SaveChangesAsync();
        }
    }
}

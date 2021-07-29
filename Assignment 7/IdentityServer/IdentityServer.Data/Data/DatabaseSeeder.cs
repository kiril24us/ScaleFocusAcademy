using IdentityServer.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IdentityServer.Data.Data
{
    public class DatabaseSeeder
    {
        public static void Seed(IServiceProvider applicationServices)
        {
            using (IServiceScope serviceScope = applicationServices.CreateScope())
            {
                DatabaseContext context = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
                if (context.Database.EnsureCreated())
                {
                    PasswordHasher<User> hasher = new PasswordHasher<User>();

                    User user = new User()
                    {
                        Id = Guid.NewGuid().ToString("D"),
                        Email = "kiril@test.test",
                        NormalizedEmail = "kiril@test.test".ToUpper(),
                        EmailConfirmed = true,
                        UserName = "admin",
                        NormalizedUserName = "admin".ToUpper(),
                        SecurityStamp = Guid.NewGuid().ToString("D")
                    };

                    user.PasswordHash = hasher.HashPassword(user, "adminpassword");

                    IdentityRole identityRoleAdmin = new IdentityRole()
                    {
                        Id = Guid.NewGuid().ToString("D"),
                        Name = "Admin",
                        NormalizedName = "Admin".ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString("D")
                    };

                    IdentityRole identityRoleRegular = new IdentityRole()
                    {
                        Id = Guid.NewGuid().ToString("D"),
                        Name = "Regular",
                        NormalizedName = "Regular".ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString("D")
                    };

                    IdentityUserRole<string> identityUserRole = new IdentityUserRole<string>() { RoleId = identityRoleAdmin.Id, UserId = user.Id };

                    context.Roles.Add(identityRoleAdmin);                   
                    context.Users.Add(user);
                    context.UserRoles.Add(identityUserRole);
                    context.Roles.Add(identityRoleRegular);

                    context.SaveChanges();
                }
            }
        }
    }

}

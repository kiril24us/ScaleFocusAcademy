using Microsoft.AspNetCore.Identity;
using ProjectManagementApplication.Data.Entities;
using System;

namespace ProjectManagementApplication.Data.Data
{
    public class SeedData
    {
        public static void SeedUserData(AppDbContext context)
        {
            PasswordHasher<User> hasher = new PasswordHasher<User>();

            User adminUser = new User()
            {
                Id = Guid.NewGuid().ToString("D"),
                Email = "admin@test.test",
                NormalizedEmail = "admin@test.test".ToUpper(),
                EmailConfirmed = true,
                UserName = "admin",
                FirstName = "",
                LastName = "",
                IsActive = true,
                NormalizedUserName = "admin".ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            User managerUser = new User()
            {
                Id = Guid.NewGuid().ToString("D"),
                Email = "manager@test.test",
                NormalizedEmail = "manager@test.test".ToUpper(),
                EmailConfirmed = true,
                UserName = "manager",
                FirstName = "",
                LastName = "",
                IsActive = true,
                NormalizedUserName = "manager".ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            adminUser.PasswordHash = hasher.HashPassword(adminUser, "adminpass");
            managerUser.PasswordHash = hasher.HashPassword(managerUser, "managerpass");

            IdentityRole identityRoleAdmin = new IdentityRole()
            {
                Id = Guid.NewGuid().ToString("D"),
                Name = "Admin",
                NormalizedName = "Admin".ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString("D")
            };

            IdentityRole identityRoleManager = new IdentityRole()
            {
                Id = Guid.NewGuid().ToString("D"),
                Name = "Manager",
                NormalizedName = "Manager".ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString("D")
            };

            IdentityUserRole<string> identityUserRole = new IdentityUserRole<string>() { RoleId = identityRoleAdmin.Id, UserId = adminUser.Id };
            IdentityUserRole<string> identityManagerRole = new IdentityUserRole<string>() { RoleId = identityRoleManager.Id, UserId = managerUser.Id };

            context.Roles.Add(identityRoleAdmin);
            context.Users.Add(adminUser);
            context.UserRoles.Add(identityUserRole);
            context.Roles.Add(identityRoleManager);
            context.Users.Add(managerUser);
            context.UserRoles.Add(identityManagerRole);

            context.SaveChanges();
        }
    }
}

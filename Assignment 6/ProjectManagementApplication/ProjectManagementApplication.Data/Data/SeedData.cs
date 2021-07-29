using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.Data.Enums;
using System;

namespace ProjectManagementApplication.Data.Data
{
    public class SeedData
    {
        public static void SeedUserData(AppDbContext context)
        {
            context.Users.Add(new User()
            {
                Username = "admin",
                Password = "adminpass",
                FirstName = "",
                LastName = "",
                Role = Enum.Parse<Role>("1"),
                IsActive = true
            });

            context.SaveChanges();
        }

    }
}

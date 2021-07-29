using System;
using ToDoApplication.Data.Data;
using ToDoApplication.Data.Entities;
using ToDoApplication.Data.Enum;

namespace ToDoApplication
{
    public class SeedData
    {
        public static void SeedUserData(AppDbContext context)
        {
            context.Users.Add(new User()
            {
                CreatedById = 0,
                Username = "admin",
                Password = "adminpassword",
                FirstName = "",
                LastName = "",
                Role = Enum.Parse<Role>("1"),
                LastModifiedById = 0,
                LastModifiedOn = DateTime.Now,
            });

            context.SaveChanges();
        }
    }
}

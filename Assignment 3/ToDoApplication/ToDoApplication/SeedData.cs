using System;
using ToDoApplication.DAL.Data;
using ToDoApplication.DAL.Entities;
using ToDoApplication.DAL.Enum;

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

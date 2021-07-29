using Microsoft.EntityFrameworkCore;
using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.Data.Enums;
using System;

namespace ProjectManagementApplication.Data.Data
{
    public class InMemoryContext
    {
        public static AppDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("InMemoryDb").Options;
            var context = new AppDbContext(options);

            User user = new User()
            {
                Username = "test",
                Password = "test",
                FirstName = "test",
                LastName = "test",
                Role = Enum.Parse<Role>("1"),
                IsActive = true
            };

            context.Users.Add(user);
            context.SaveChanges();

            return context;
        }

        public static int LoginNotAdminUser(AppDbContext context)
        {
            User user = new User()
            {
                Username = "a",
                Password = "a",
                FirstName = "a",
                LastName = "a",
                Role = Enum.Parse<Role>("2"),
                IsActive = true
            };

            context.Users.Add(user);
            context.SaveChanges();

            return user.Id;
        }

        public static void AddInMemoryTeam(AppDbContext context)
        {
            Team team = new Team
            {
                Name = "test",
                IsActive = true
            };

            context.Teams.Add(team);
            context.SaveChanges();
        }
    }
}

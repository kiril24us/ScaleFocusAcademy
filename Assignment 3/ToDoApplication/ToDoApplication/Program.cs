using Microsoft.EntityFrameworkCore;
using ToDoApplication.DAL.Data;
using ToDoApplication.Views;

namespace ToDoApplication
{
    class Program
    {
        private static AppDbContext _applicationContext = new AppDbContext();

        static void Main()
        {
            InitializeApplication();

            bool shouldExit = false;
            MainMenu mainMenu = new MainMenu();

            while (!shouldExit)
            {
                shouldExit = mainMenu.MainMenuView();
            }
        }

        static void InitializeApplication()
        {
            if (!_applicationContext.Database.CanConnect())
            {
                //Create new database and tables
                _applicationContext.Database.Migrate();
                //Seed data with default user
                SeedData.SeedUserData(_applicationContext);
            }
        }       
    }
}

using System;

namespace ToDoApplication.ApplicationMethods
{
    public class CommonMethods
    {
        public static int ReadAnIntFromTheConsole()
        {
            int number;
            bool result;
            result = int.TryParse(Console.ReadLine(), out number);
            while (!result)
            {
                Console.WriteLine("Write a number");
                result = int.TryParse(Console.ReadLine(), out number);
            }
            return number;
        }

        public static string CheckIfTitleIsCorrect(string title)
        {
            while (title == "")
            {
                Console.WriteLine("Please enter a title!");
                title = Console.ReadLine();
            }

            return title;
        }

        public static void ToDoListNotExist(int toDoListId)
        {
            Console.WriteLine($"To Do list with id {toDoListId} was not found!");
        }

        public static bool ReadingBooleanFromConsole()
        {
            string result = Console.ReadLine();

            while (result != "Y" && result != "N" && result != "y" && result != "n")
            {
                Console.WriteLine("Write Y or N");
                result = Console.ReadLine();
            }
            if (result == "Y" || result == "y")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

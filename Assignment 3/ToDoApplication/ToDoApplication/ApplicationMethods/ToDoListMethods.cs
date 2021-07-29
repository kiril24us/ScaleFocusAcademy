using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ToDoApplication.BLL.Services;
using ToDoApplication.DAL.Entities;

namespace ToDoApplication.ApplicationMethods
{
    public class ToDoListMethods
    {
        private static IToDoListService _toDoListService;
        private static IUserService _userService;

        public ToDoListMethods(IToDoListService toDoListService, IUserService userService)
        {
            _toDoListService = toDoListService;
            _userService = userService;
        }

        public void ListAllToDoListsOfUser()
        {
            List<ToDoList> createdToDoLists = _toDoListService.GetAllListsCreatedByUser(_userService.CurrentUser.UserId);
            List<int> allIds = _toDoListService.GetAllIdsOfSharedToDoListsOfTheUser(_userService.CurrentUser.UserId);

            if (createdToDoLists.Any())
            {
                Console.WriteLine($"Next lines are all To Do Lists created by user with id {_userService.CurrentUser.UserId}");
                foreach (ToDoList toDoList in createdToDoLists)
                {
                    Console.WriteLine($"========================={toDoList.ToDoListId}===========================");
                    Console.WriteLine($"ToDo list title is {toDoList.Title}");
                    Console.WriteLine($"The ToDo list was created on {toDoList.CreatedOn.ToString("D", CultureInfo.InvariantCulture)}");
                    Console.WriteLine($"The last change of the ToDo list was on {toDoList.LastModifiedOn.ToString("D", CultureInfo.InvariantCulture)} and was done by user with id {toDoList.LastModifiedById}");
                    Console.WriteLine($"{toDoList.Tasks.Count} Tasks");
                    Console.WriteLine("=======================================================");

                }
            }
            else
            {
                Console.WriteLine("=======================================================");
                Console.WriteLine($"No lists created by user with id {_userService.CurrentUser.UserId} was found!");
                Console.WriteLine("=======================================================");

            }

            if (allIds.Count > 0)
            {
                Console.WriteLine($"Next lines are all To Do Lists shared with user with id {_userService.CurrentUser.UserId}");
                foreach (int id in allIds)
                {
                    ToDoList toDoList = _toDoListService.GetToDoListFromDatabaseById(id);
                    Console.WriteLine($"========================={toDoList.ToDoListId}===========================");
                    Console.WriteLine($"ToDo list title is {toDoList.Title}");
                    Console.WriteLine($"The ToDo list was created on {toDoList.CreatedOn}");
                    Console.WriteLine($"The last change of the ToDo list was on {toDoList.LastModifiedOn} and was done by user with id {toDoList.LastModifiedById}");
                    Console.WriteLine($"{toDoList.Tasks.Count} Tasks");
                    Console.WriteLine("=======================================================");

                }
            }
            else
            {
                Console.WriteLine("=======================================================");
                Console.WriteLine($"No shared lists for user with id {_userService.CurrentUser.UserId} was found!");
                Console.WriteLine("=======================================================");

            }
        }

        public void EditToDoList()
        {
            Console.WriteLine("Write Id of the ToDo List which want to be edited!");
            int toDoListId = CommonMethods.ReadAnIntFromTheConsole();
            bool isExist = _toDoListService.CheckIfToDoListExistInTheDatabase(toDoListId);
            if (isExist)
            {
                Console.WriteLine("Write the new title of the ToDo list ");
                string title = Console.ReadLine();
                CommonMethods.CheckIfTitleIsCorrect(title);

                bool isSuccess = _toDoListService.EditToDoList(toDoListId, _userService.CurrentUser.UserId, title);
                if (isSuccess)
                {
                    Console.WriteLine($"You successfully edited the ToDo list with id {toDoListId}");
                }
            }
            else
            {
                CommonMethods.ToDoListNotExist(toDoListId);
            }
        }

        /// <summary>
        /// Shares a To Do List with other User
        /// </summary>
        public void ShareToDoList()
        {
            Console.WriteLine("Write Id of To Do list which you want to be shared");
            int toDoListId = CommonMethods.ReadAnIntFromTheConsole();
            bool isExist = _toDoListService.CheckIfToDoListIsCreatedByUser(toDoListId, _userService.CurrentUser.UserId);

            if (isExist)
            {
                Console.WriteLine("Write Id of the user with whom you want to share the To Do List");
                int userId = CommonMethods.ReadAnIntFromTheConsole();
                if (userId == _userService.CurrentUser.UserId)
                {
                    Console.WriteLine("You can not share a To Do List with you, created by you!");
                }
                else
                {
                    bool isUserExist = _userService.CheckIfUserExistById(userId);
                    if (isUserExist)
                    {
                        bool isSuccess = _toDoListService.ShareToDoList(toDoListId, userId);
                        if (isSuccess)
                        {
                            Console.WriteLine($"You successfully shared To Do list with id {toDoListId} with user with id {userId}!");
                        }
                        else
                        {
                            Console.WriteLine($"You already shared To Do list with id {toDoListId} with user with id {userId}!");
                        }

                    }
                    else
                    {
                        Console.WriteLine($"User with id {userId} not exist");
                    }
                }
            }
            else
            {
                CommonMethods.ToDoListNotExist(toDoListId);
            }
        }

        public void DeleteToDoList()
        {
            Console.WriteLine("Write Id of the ToDo List which you want to be deleted!");
            int toDoListId = CommonMethods.ReadAnIntFromTheConsole();

            bool isSuccess = _toDoListService.DeleteToDolist(toDoListId, _userService.CurrentUser.UserId);
            if (isSuccess)
            {
                Console.WriteLine($"ToDo List with id {toDoListId} was successfully deleted!");
            }
            else
            {
               CommonMethods.ToDoListNotExist(toDoListId);
            }
        }

        public void CreateToDoList()
        {
            Console.WriteLine("Write the title of your ToDo list");
            string title = Console.ReadLine();
            title = CommonMethods.CheckIfTitleIsCorrect(title);

            bool isSuccess = _toDoListService.CreateToDoList(title, _userService.CurrentUser.UserId);
            if (isSuccess)
            {
                Console.WriteLine($"ToDo List with title {title} was successfully created!");
            }
            else
            {
                Console.WriteLine($"Already exists ToDo List with title {title}!");
            }
        }       
    }
}

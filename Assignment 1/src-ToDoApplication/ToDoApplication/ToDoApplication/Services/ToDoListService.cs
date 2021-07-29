using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToDoApplication.Data;
using ToDoApplication.Entities;

namespace ToDoApplication.Services
{
    /// <summary>
    /// Manages ToDoList related functionality
    /// </summary>
    public class ToDoListService : IToDoListService
    {       
        private const string StoreFileName = "Users.json";
        private const string StoreFileNameForCounterToDoLists = "CounterToDoList.json";
        private int counterForCreateToDoLists;
        private readonly FileDatabase _storage;
        public static IUserService _userService = new UserService();

        public ToDoListService()
        {
            _storage = new FileDatabase();
            int counterToDoListsFromFile = _storage.ReadNumber(StoreFileNameForCounterToDoLists);
            counterForCreateToDoLists = counterToDoListsFromFile;
        }

        public bool CheckIfToDoListExistInTheUser(int toDoListId, int id)
        {
            User user = _userService.FindUserById(id);
            return user.ToDoLists.Any(x => x.Id == toDoListId);
        }

        public bool CheckIfSharedToDoListExistInTheUser(int sharedToDoListId, int id)
        {
            User user = _userService.FindUserById(id);
            return user.SharedToDoLists.Any(x => x.Id == sharedToDoListId);
        }

        /// <summary>
        /// Creates new ToDolist
        /// </summary>
        /// <param name="name">The title of the new ToDoList</param>
        /// <returns>True if list is created otherwise false</returns>
        public bool CreateToDoList(string title, int creatorId)
        {
            User user = _userService.FindUserById(creatorId);

            if (user.ToDoLists.Any(x => x.Title == title))
            {
                return false;
            }
           
            ToDoList toDoList = new ToDoList
            {
                CreatorId = creatorId,
                Id = counterForCreateToDoLists,
                DateOfCreation = DateTime.Now,
                Title = title,
            };
            counterForCreateToDoLists++;
            user.ToDoLists.Add(toDoList);
            _storage.Write(StoreFileName, UserService._applicationUsers);
            _storage.Write(StoreFileNameForCounterToDoLists, counterForCreateToDoLists);
            return true;
        }


        /// <summary>
        /// Delete ToDo List
        /// </summary>
        /// <param name="toDoListId"></param>
        /// <returns>True if ToDo List is deleted otherwise false</returns>
        public bool DeleteToDolist(int toDoListId, int creatorId)
        {
            bool isExist = CheckIfToDoListExistInTheUser(toDoListId, creatorId);

            bool isExistInOtherCollection = IsExistInSharedToDoListInTheUser(toDoListId, creatorId);
            if (!isExist && !isExistInOtherCollection)
            {
                return false;
            }

            User user = _userService.FindUserById(creatorId);
            if (isExist)
            {
                ToDoList toDoList = FindToDoListInHisCreator(toDoListId, creatorId);
                bool isToDoListSharedByOtherUsers = UserService._applicationUsers.Any(x => x.SharedToDoLists.Any(x => x.CreatorId == creatorId && x.Title == toDoList.Title));
                if(isToDoListSharedByOtherUsers)
                {
                    List<User> usersWithTheList = UserService._applicationUsers.FindAll(x => x.SharedToDoLists.Any(x => x.CreatorId == creatorId && x.Title == toDoList.Title));
                    foreach (User user_ in usersWithTheList)
                    {
                        user_.SharedToDoLists.Remove(toDoList);
                    }
                }
                user.ToDoLists.Remove(toDoList);
                _storage.Write(StoreFileName, UserService._applicationUsers);
            }
            else
            {
                ToDoList toDoList = FindSharedToDoListById(toDoListId, creatorId);
                user.SharedToDoLists.Remove(toDoList);
                _storage.Write(StoreFileName, UserService._applicationUsers);
            }

            return true;
        }

        public bool IsExistInSharedToDoListInTheUser(int toDoListId, int userId)
        {
            User user = _userService.FindUserById(userId);
            if (!user.SharedToDoLists.Any(x => x.Id == toDoListId))
            {
                return false;
            }
            return true;
        }

        public ToDoList FindToDoListInHisCreator(int toDoListId, int creatorId)
        {
            User user = _userService.FindUserById(creatorId);
            return user.ToDoLists.FirstOrDefault(x => x.Id == toDoListId);
        }

        public ToDoList FindSharedToDoListById(int toDoListId, int creatorId)
        {
            User user = _userService.FindUserById(creatorId);
            return user.SharedToDoLists.FirstOrDefault(x => x.Id == toDoListId);
        }

        /// <summary>
        /// Share To Do List with other user
        /// </summary>
        /// <param name="toDoListId"></param>
        /// <param name="userIdToBeShared"></param>
        /// <returns>True if ToDo List is deleted otherwise false</returns>
        public bool ShareToDoList(int toDoListId, int userIdToBeShared, int creatorId)
        {
            ToDoList toDoList = FindToDoListInHisCreator(toDoListId, creatorId);
            User user = _userService.FindUserById(userIdToBeShared);
            bool isAlreadyShared = user.SharedToDoLists.Any(x => x.Title == toDoList.Title);
            if (isAlreadyShared)
            {
                return false;
            }

            user.SharedToDoLists.Add(toDoList);
            _storage.Write(StoreFileName, UserService._applicationUsers);        
            return true;
        }

        public static bool CheckIfToDoListIsCreatedByMe(ToDoList toDoList, User user)
        {
            return user.ToDoLists.Any(x => x.Title == toDoList.Title);
        }

        public bool EditToDoList(int toDoListId, int userId, string title)
        {
            bool isExist = CheckIfToDoListExistInTheUser(toDoListId, userId);
            bool isExistInOtherCollection = IsExistInSharedToDoListInTheUser(toDoListId, userId);
            if (!isExist && !isExistInOtherCollection)
            {
                return false;
            }           

            if (isExist)
            {
                ToDoList toDoList = FindToDoListInHisCreator(toDoListId, userId);
                AssignValues(userId, title, toDoList);
                _storage.Write(StoreFileName, UserService._applicationUsers);
            }
            else
            {
                ToDoList toDoList = FindSharedToDoListById(toDoListId, userId);
                AssignValues(userId, title, toDoList);
                _storage.Write(StoreFileName, UserService._applicationUsers);
            }        
            return true;
        }

        private static void AssignValues(int userId, string title, ToDoList toDoList)
        {
            DateTime dateOfChange = DateTime.Now;

            toDoList.Title = title;
            toDoList.DateOfLastChange = dateOfChange;
            toDoList.IdOfUserLastChange = userId;
        }

        public List<ToDoList> GetAllListsCreatedByUser(int userId)
        {
            User user = _userService.FindUserById(userId);
            return user.ToDoLists.ToList();
        }

        public List<ToDoList> GetAllListsSharedByUser(int userId)
        {
            User user = _userService.FindUserById(userId);
            return user.SharedToDoLists.ToList();
        }

        public ToDoList FindToDoListWhichContainsTask(int taskId)
        {
            ToDoList toDoList = UserService._applicationUsers.SelectMany(x => x.ToDoLists).FirstOrDefault(x => x.Tasks.Any(x => x.Id == taskId));
            return toDoList;
        }
       
        public ToDoList FindSharedToDoListWhichContainsTask(int taskId)
        {
            ToDoList sharedToDoList = UserService._applicationUsers.SelectMany(x => x.SharedToDoLists).FirstOrDefault(x => x.Tasks.Any(x => x.Id == taskId));
            return sharedToDoList;
        }

        
    }
}

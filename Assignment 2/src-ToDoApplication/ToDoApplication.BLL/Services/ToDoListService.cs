using System;
using System.Collections.Generic;
using System.Linq;
using ToDoApplication.DAL.Data;
using ToDoApplication.DAL.Entities;

namespace ToDoApplication.BLL.Services
{
    /// <summary>
    /// Manages ToDoList related functionality
    /// </summary>
    public class ToDoListService : IToDoListService
    {            
        private readonly ToDoListDatabase _toDoListDatabase;

        public ToDoListService(ToDoListDatabase toDoListDatabase)
        {
            _toDoListDatabase = toDoListDatabase;
        }
       
        /// <summary>
        /// Creates new ToDolist
        /// </summary>
        /// <param name="name">The title of the new ToDoList</param>
        /// <returns>True if list is created otherwise false</returns>
        public bool CreateToDoList(string title, int creatorId)
        {
            bool isExist = _toDoListDatabase.CheckIfToDoListExistsByTitle(title);

            if (isExist)
            {
                return false;
            }

            User user = _toDoListDatabase.GetUserById(creatorId);
            DateTime dateOfCreation = DateTime.Now;
            ToDoList toDoList = new ToDoList
            {
                Title = title,
                CreatorId = creatorId,
                DateOfCreation = dateOfCreation,
                DateOfLastChange = dateOfCreation,
                IdOfUserLastChange = creatorId
            };

            return _toDoListDatabase.CreateToDoList(toDoList);
        }


        /// <summary>
        /// Delete ToDo List
        /// </summary>
        /// <param name="toDoListId"></param>
        /// <returns>True if ToDo List is deleted otherwise false</returns>
        public bool DeleteToDolist(int toDoListId, int userId)
        {
            bool isExist = _toDoListDatabase.CheckIfToDoListExistInTheUser(toDoListId, userId);
            bool IsExistInSharedToDoListsInTheUser = _toDoListDatabase.CheckIfToDoListExistInSharedToDoListsOfUser(toDoListId, userId);

            if (!isExist && !IsExistInSharedToDoListsInTheUser)
            {
                return false;
            }

            if (IsExistInSharedToDoListsInTheUser)
            {          
                bool isToDoListSharedByOtherUsers = _toDoListDatabase.CheckIfToDoListIsSharedByOtherUsers(toDoListId);
                if (isToDoListSharedByOtherUsers)
                {
                    List<int> usersIdsWithSharedList = _toDoListDatabase.GetUsersIdsWithSharedList(toDoListId);
                    foreach (int _userId in usersIdsWithSharedList)
                    {
                        if(_userId == userId)
                        {
                            return _toDoListDatabase.DeleteSharedToDoList(toDoListId, userId);
                        }                        
                    }
                }
                else
                {
                    return _toDoListDatabase.DeleteSharedToDoList(toDoListId, userId);
                }
            }
            else
            {
                return _toDoListDatabase.DeleteToDoList(toDoListId, userId);
            }
            return true;
        }

        /// <summary>
        /// Share To Do List with other user
        /// </summary>
        /// <param name = "toDoListId" ></ param >
        /// < param name="userIdToBeShared"></param>
        /// <returns>True if ToDo List is shared otherwise false</returns>
        public bool ShareToDoList(int toDoListId, int userIdToBeShared)
        {
            bool isAlreadyShared = _toDoListDatabase.CheckIfToDoListIsAlreadySharedWithThatUser(toDoListId, userIdToBeShared);
            if (isAlreadyShared)
            {
                return false;
            }

            return _toDoListDatabase.ShareToDoList(toDoListId, userIdToBeShared);           
        }

        public bool EditToDoList(int toDoListId, int userId, string title)
        {
            DateTime dateOfChange = DateTime.Now;
            return _toDoListDatabase.EditToDoList(toDoListId, dateOfChange, userId, title);
        }

        public List<ToDoList> GetAllListsCreatedByUser(int userId)
        {
            return _toDoListDatabase.GetAllListsCreatedByUser(userId);
        }

        public List<ToDoList> GetAllListsSharedByUser(List<int> allIds)
        {
            return _toDoListDatabase.GetAllListsSharedByUser(allIds);
        }

        public List<int> GetAllIdsOfSharedToDoListsOfTheUser(int userId)
        {
            return _toDoListDatabase.GetAllIdsOfSharedToDoListsOfTheUser(userId);

        }

        public int GetToDoListIdWhichContainsTask(int taskId)
        {
            return _toDoListDatabase.GetToDoListIdWhichContainsTask(taskId);
        }

        public bool CheckIfToDoListExistInTheUser(int toDoListId, int userId)
        {
            return _toDoListDatabase.CheckIfToDoListExistInTheUser(toDoListId, userId);
        }

        public bool CheckIfToDoListExistInTheDatabase(int toDoListId)
        {
            return _toDoListDatabase.CheckIfToDoListExistInTheDatabase(toDoListId);
        }

        public bool CheckIfToDoListExistInSharedListsOfUser(int toDoListId, int userId)
        {
            return _toDoListDatabase.CheckIfToDoListIsAlreadySharedWithThatUser(toDoListId, userId);
        }
    }
}

using System;
using System.Collections.Generic;
using ToDoApplication.Data.Entities;
using ToDoApplication.Data.Interfaces;
using ToDoApplication.Services.Interfaces;

namespace ToDoApplication.Services.Services
{
    /// <summary>
    /// Manages ToDoList related functionality
    /// </summary>
    public class ToDoListService : IToDoListService
    {            
        private readonly IToDoListRepository _toDoListRepository;
        private readonly ITaskRepository _taskRepository;

        public ToDoListService(IToDoListRepository toDoListRepository, ITaskRepository taskRepository)
        {
            _toDoListRepository = toDoListRepository;
            _taskRepository = taskRepository;
        }
       
        /// <summary>
        /// Creates new ToDolist
        /// </summary>
        /// <param name="name">The title of the new ToDoList</param>
        /// <returns>True if list is created otherwise false</returns>
        public bool CreateToDoList(string title, int creatorId)
        {
            bool isExist = _toDoListRepository.CheckIfToDoListExistByTitle(title);

            if (isExist)
            {
                return false;
            }

            DateTime dateOfCreation = DateTime.Now;
            ToDoList toDoList = new ToDoList
            {
                Title = title,
                CreatedById = creatorId,
                CreatedOn = dateOfCreation,
                LastModifiedOn = dateOfCreation,
                LastModifiedById = creatorId
            };

            return _toDoListRepository.Create(toDoList);
        }


        /// <summary>
        /// Delete ToDo List
        /// </summary>
        /// <param name="toDoListId"></param>
        /// <returns>True if ToDo List is deleted otherwise false</returns>
        public bool DeleteToDolist(int toDoListId, int userId)
        {
            bool isExist = _toDoListRepository.CheckIfToDoListIsCreatedByUser(toDoListId, userId);
            bool IsExistInSharedToDoListsInTheUser = _toDoListRepository.CheckIfToDoListExistInSharedToDoListsOfUser(toDoListId, userId);

            if (!isExist && !IsExistInSharedToDoListsInTheUser)
            {
                return false;
            }

            if (isExist)
            {               
                ToDoList toDoList = _toDoListRepository.GetToDoListById(toDoListId);
                
                    return _toDoListRepository.Delete(toDoList);                
            }
            if(IsExistInSharedToDoListsInTheUser)
            {                
                UserToDoList userToDoList = _toDoListRepository.GetSharedList(toDoListId, userId);

                return _toDoListRepository.DeleteSharedToDoList(userToDoList);
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
            bool isAlreadyShared = _toDoListRepository.CheckIfToDoListIsAlreadySharedWithThatUser(toDoListId, userIdToBeShared);
            if (isAlreadyShared)
            {
                return false;
            }
            UserToDoList userToDoList = new UserToDoList
            {
                ToDoListId = toDoListId,
                UserId = userIdToBeShared
            };

            return _toDoListRepository.ShareToDoList(userToDoList);           
        }

        public bool EditToDoList(int toDoListId, int userId, string title)
        {
            DateTime dateOfChange = DateTime.Now;
            ToDoList toDoListToEdit = _toDoListRepository.GetToDoListById(toDoListId);
            toDoListToEdit.LastModifiedOn = dateOfChange;
            toDoListToEdit.LastModifiedById = userId;
            toDoListToEdit.Title = title;

            return _toDoListRepository.Edit();
        }

        public List<ToDoList> GetAllListsCreatedByUser(int userId)
        {
            return _toDoListRepository.GetAll(userId);
        }

        public List<int> GetAllIdsOfSharedToDoListsOfTheUser(int userId)
        {
            return _toDoListRepository.GetAllIdsOfSharedToDoListsOfTheUser(userId);
        }

        public ToDoList GetToDoListFromDatabaseById(int toDoListId)
        {
            return _toDoListRepository.GetToDoListById(toDoListId);
        }

        public int GetToDoListIdWhichContainsTask(int taskId)
        {
            return _toDoListRepository.GetToDoListIdWhichContainsTask(taskId);
        }

        public ToDoList GetToDoListByTitle(string title)
        {
            return _toDoListRepository.GetToDoListByTitle(title);
        }

        public bool CheckIfToDoListExistInTheDatabase(int toDoListId)
        {
            return _toDoListRepository.CheckIfToDoListExistInTheDatabase(toDoListId);
        }

        public bool CheckIfToDoListExistInSharedToDoListsOfUser(int toDoListId, int userId)
        {
            return _toDoListRepository.CheckIfToDoListExistInSharedToDoListsOfUser(toDoListId, userId);
        }

        public bool CheckIfToDoListIsCreatedByUser(int toDoListId, int userId)
        {
            return _toDoListRepository.CheckIfToDoListIsCreatedByUser(toDoListId, userId);
        }
    }
}

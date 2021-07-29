using System;
using System.Collections.Generic;
using ToDoApplication.DAL.Entities;
using ToDoApplication.DAL.Repositories;

namespace ToDoApplication.BLL.Services
{
    /// <summary>
    /// Manages ToDoList related functionality
    /// </summary>
    public class ToDoListService : IToDoListService
    {
        private readonly IToDoListRepository _toDoListRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITaskRepository _taskRepository;

        public ToDoListService(IToDoListRepository toDoListRepository, IUserRepository userRepository, ITaskRepository taskRepository)
        {
            _toDoListRepository = toDoListRepository;
            _userRepository = userRepository;
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

            User user = _userRepository.GetUserById(creatorId);
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
                //bool isToDoListSharedByOtherUsers = _toDoListRepository.CheckIfToDoListIsSharedByOtherUsers(toDoListId);
                ToDoList toDoList = _toDoListRepository.GetToDoListById(toDoListId);
                //if (isToDoListSharedByOtherUsers)
                //{
                //    List<int> usersIdsWithSharedList = _toDoListRepository.GetUsersIdsWithSharedList(toDoListId);
                //    foreach (int _userId in usersIdsWithSharedList)
                //    {
                //        if (_userId == userId)
                //        {
                //            UserToDoList userToDoList = _toDoListRepository.GetSharedList(toDoListId, _userId);
                //            //Delete Shared To Do List 
                //            _toDoListRepository.DeleteSharedToDoList(userToDoList);
                //        }
                //    }
                //    return _toDoListRepository.Delete(toDoList);
                //}
                //else
                //{
                    return _toDoListRepository.Delete(toDoList);
                //}
            }
            if (IsExistInSharedToDoListsInTheUser)
            {
                ////Check if there are any shared tasks with the to do list they should be delete also
                //bool isExistTasks = _toDoListRepository.CheckIfThereAreCreatedTasksToToDoList(toDoListId);
                //if (isExistTasks)
                //{
                //    List<int> allTaskIds = _taskRepository.GetAllTaskIdsCreatedToToDoList(toDoListId);
                //    if (allTaskIds.Count > 0)
                //    {
                //        foreach (int taskId in allTaskIds)
                //        {
                //            bool isExistAssignedTask = _taskRepository.CheckIfThereIsAssignedTaskToTheUser(userId, taskId);
                //            if (isExistAssignedTask)
                //            {
                //                UserTask userTask = _taskRepository.GetSharedTaskById(userId, taskId);
                //                _taskRepository.DeleteAssignTask(userTask);
                //            }
                //        }
                //    }
                //}
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
            User user = _userRepository.GetUserById(userId);
            return _toDoListRepository.GetAllListsCreatedByUser(userId);
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

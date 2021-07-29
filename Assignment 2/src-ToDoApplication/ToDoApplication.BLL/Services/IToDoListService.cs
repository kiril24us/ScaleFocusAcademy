using System;
using System.Collections.Generic;
using ToDoApplication.DAL.Entities;

namespace ToDoApplication.BLL.Services
{
    public interface IToDoListService
    {
        public bool CreateToDoList(string title, int creator);

        public bool DeleteToDolist(int toDoListId, int creatorId);

        public bool CheckIfToDoListExistInTheUser(int toDoListId, int userId);

        public bool CheckIfToDoListExistInTheDatabase(int toDoListId);

        public bool ShareToDoList(int toDoListId, int userIdToBeShared);

        public bool EditToDoList(int toDoListId, int userId, string title);

        public bool CheckIfToDoListExistInSharedListsOfUser(int toDoListId, int userId);

        public List<ToDoList> GetAllListsCreatedByUser(int userId);

        public List<ToDoList> GetAllListsSharedByUser(List<int> allIds);

        public List<int> GetAllIdsOfSharedToDoListsOfTheUser(int userId);

        public int GetToDoListIdWhichContainsTask(int taskId);

    }
}

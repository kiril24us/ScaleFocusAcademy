using System;
using System.Collections.Generic;
using System.Text;
using ToDoApplication.Entities;

namespace ToDoApplication.Services
{
    public interface IToDoListService
    {
        public bool CreateToDoList(string title, int creator);

        public bool DeleteToDolist(int toDoListId, int creatorId);

        public bool CheckIfToDoListExistInTheUser(int toDoListId, int creatorId);

        public bool CheckIfSharedToDoListExistInTheUser(int sharedToDoListId, int userId);

        public ToDoList FindToDoListInHisCreator(int toDoListId, int creatorId);

        public ToDoList FindSharedToDoListById(int toDoListId, int creatorId);

        public bool ShareToDoList(int toDoListId, int userIdToBeShared, int creatorId);

        public bool EditToDoList(int toDoListId, int userId, string title);

        public bool IsExistInSharedToDoListInTheUser(int toDoListId, int userId);

        public List<ToDoList> GetAllListsCreatedByUser(int userId);

        public List<ToDoList> GetAllListsSharedByUser(int userId);

        public ToDoList FindToDoListWhichContainsTask(int taskId);

        public ToDoList FindSharedToDoListWhichContainsTask(int taskId);

    }
}

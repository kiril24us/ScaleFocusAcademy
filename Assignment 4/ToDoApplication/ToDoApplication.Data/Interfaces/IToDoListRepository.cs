using System.Collections.Generic;
using ToDoApplication.Data.Entities;

namespace ToDoApplication.Data.Interfaces
{
    public interface IToDoListRepository : IRepository<ToDoList>
    {
        bool DeleteSharedToDoList(UserToDoList sharedList);

        bool CheckIfToDoListExistByTitle(string title);

        bool CheckIfToDoListIsCreatedByUser(int toDoListId, int userId);

        bool CheckIfToDoListExistInSharedToDoListsOfUser(int toDoListId, int userId);

        bool CheckIfToDoListIsAlreadySharedWithThatUser(int toDoListId, int userIdToBeShared);

        bool CheckIfToDoListExistInTheDatabase(int toDoListId);

        bool ShareToDoList(UserToDoList userToDoList);

        int GetToDoListIdWhichContainsTask(int taskId);
      
        List<int> GetAllIdsOfSharedToDoListsOfTheUser(int userId);

        UserToDoList GetSharedList(int toDoListId, int userId);

        ToDoList GetToDoListById(int toDoListId);

        ToDoList GetToDoListByTitle(string title);
    }
}